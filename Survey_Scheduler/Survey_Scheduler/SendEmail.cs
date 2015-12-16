/*
* PURPOSE: SendEmail Class
* AUTHOR:  
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.IO;
using System.Net;

/// <summary>
/// Summary description for SendEmail
/// </summary>
/// 
namespace Survey_Scheduler
{
    public static class SendEmail
    {
        #region Public Methods
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="To">to</param>
        /// <param name="From">From</param>
        /// <param name="strCC">CC</param>
        /// <param name="strBCC">BCC</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="IsBodyHtml">Is Body Html</param>
        /// <param name="emailImage">email Image</param>
        /// <returns></returns>
        public static bool Send(string To, string From, string strCC, string strBCC, string Subject, string Body, bool IsBodyHtml, string emailImage)
        {
            string error = "";

            if (String.IsNullOrEmpty(From))
                From = ConfigurationSettings.AppSettings["SmtpMailFrom"].ToString();

            try
            {
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(From);
                message.From = fromAddress;
                if (To != null && To != "")
                {
                    message.To.Add(To);
                }
                if (strCC != null && strCC != "")
                {
                    message.CC.Add(strCC);
                }
                if (strBCC != null && strBCC != "")
                {
                    message.Bcc.Add(strBCC);
                }
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsBodyHtml;

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                message.Headers.Add("Disposition-Notification-To", From);

                if (emailImage != "")
                {
                    ////Add Image to Email
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                    LinkedResource logo = new LinkedResource(emailImage);
                    logo.ContentId = "companylogo";
                    htmlView.LinkedResources.Add(logo);

                    //add the views
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body.Replace("<img src=cid:companylogo>", "");
                }

                SmtpClient client = new SmtpClient();

                if (ConfigurationSettings.AppSettings["HostingServer"].ToString() != "0")
                {
                    NetworkCredential Authcredentials = new NetworkCredential(ConfigurationSettings.AppSettings["SmtpUserId"].ToString(), ConfigurationSettings.AppSettings["SmtpPassword"].ToString());
                    client.UseDefaultCredentials = false;
                    client.Credentials = Authcredentials;
                }

                client.Host = ConfigurationSettings.AppSettings["SmtpServer"].ToString();
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["SmtpServerPort"]);

                client.Send(message);
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                error = ex.Message;
                Survey_LookUp.HandleException(ex);
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Survey_LookUp.HandleException(ex);
                return false;
            }
        }

        #endregion
    }
}
