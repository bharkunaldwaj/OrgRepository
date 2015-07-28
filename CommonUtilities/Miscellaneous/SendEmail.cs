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
using System.Web;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.IO;
using System.Net;

/// <summary>
/// Summary description for SendEmail
/// </summary>
/// 
namespace Miscellaneous
{
    public static class SendEmail
    {
        const string Attachments = "";

        static Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        static MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

        #region Public Methods

        /// <summary>
        /// Sends email by reading System.Net configurations.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="toList"></param>
        public static void Send(string p_subject, string p_body, string p_toList, string attachement = "")
        {
            Send(p_subject, p_body, p_toList, null, null, true, attachement);
        }

        /// <summary>
        /// Sends email by reading System.Net configurations.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        /// <param name="bccList"></param>
        /// <param name="isHtml"></param>
        public static void Send(string p_subject, string p_body, string p_toList, string p_ccList, string p_bccList, bool p_isBodyHtml, string attachement)
        {
            try
            {



                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                if (!string.IsNullOrEmpty(p_toList))
                {
                    foreach (string to in p_toList.Split(';'))
                    {
                        message.To.Add(new MailAddress(to));
                    }
                }

                if (!string.IsNullOrEmpty(p_ccList))
                {
                    foreach (string cc in p_ccList.Split(';'))
                    {
                        message.CC.Add(new MailAddress(cc));
                    }
                }

                if (!string.IsNullOrEmpty(p_bccList))
                {
                    foreach (string bcc in p_bccList.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(bcc));
                    }
                }

                message.IsBodyHtml = p_isBodyHtml;
                message.Body = p_body;
                message.Subject = p_subject;
              
                if (!string.IsNullOrEmpty(attachement))
                    message.Attachments.Add(new System.Net.Mail.Attachment(attachement));

                //MailAddress maddr = new MailAddress("sklakhiwal@gmail.com","Sumnesh Lakhiwal");
                //message.From = maddr;

                client.Host = mailSettings.Smtp.Network.Host;
                client.Port = mailSettings.Smtp.Network.Port;

                client.Send(message);
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteLogForSendEmailError(ex, null);
            }
        }

        /// <summary>
        /// Created By :: 
        /// function return true if  mail sent successfully else it returns  false
        /// </summary>
        /// <param name="To">
        /// <param name="From">
        /// <param name="strCC">
        /// <param name="strBCC">
        /// <param name="Subject">
        /// <param name="Body">
        /// <param name="IsBodyHtml">
        /// <returns>bool</returns
        /*  public static bool Send(string p_to, string p_from, string p_CC, string p_BCC, string p_subject, string p_body, bool p_isBodyHtml)
            {
            

                if (String.IsNullOrEmpty(p_from))
                    p_from = "shinig@damcogroup.com";
                try
                {
                    MailMessage message = new MailMessage();
                    MailAddress fromAddress = new MailAddress(p_from);
                    message.From = fromAddress;
                    if (p_to != null && p_to != "")
                    {
                        message.To.Add(p_to);
                    }
                    if (p_CC != null && p_CC != "")
                    {
                        message.CC.Add(p_CC);
                    }
                    if (p_BCC != null && p_BCC != "")
                    {
                        message.Bcc.Add(p_BCC);
                    }
                    message.Subject = p_subject;
                    message.Body = p_body;
                    message.IsBodyHtml = p_isBodyHtml;
                    SmtpClient client = new SmtpClient();
                
                    if (AppSetting.SmtpServer != "0")
                    {
                        NetworkCredential Authcredentials = new NetworkCredential(AppSetting.SmtpUserId, AppSetting.SmtpPassword);
                        client.UseDefaultCredentials = false;
                        client.Credentials = Authcredentials;
                    }
                    client.Host = AppSetting.SmtpServer.ToString();
                    client.Port = Convert.ToInt16(AppSetting.SmtpServerPort);
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    GeneralFunctions.WriteErrorToLog("SendMail.cs", "Send", DateTime.Now, "Too many", ex.Message.ToString());
                    return false;
                }
            }*/

        public static void Send(string p_subject, string p_body, string p_toList, MailAddress maddr, string emailimagepath)
        {
            Send(p_subject, p_body, p_toList, null, null, true, maddr, emailimagepath);
        }

        public static void Send(string p_subject, string p_body, string p_toList, string p_ccList, string p_bccList, bool p_isBodyHtml, MailAddress maddr, string emailimagepath)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                if (!string.IsNullOrEmpty(p_toList))
                {
                    foreach (string to in p_toList.Split(';'))
                    {
                        message.To.Add(new MailAddress(to));
                    }
                }

                if (!string.IsNullOrEmpty(p_ccList))
                {
                    foreach (string cc in p_ccList.Split(';'))
                    {
                        message.CC.Add(new MailAddress(cc));
                    }
                }

                if (!string.IsNullOrEmpty(p_bccList))
                {
                    foreach (string bcc in p_bccList.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(bcc));
                    }
                }

                message.IsBodyHtml = p_isBodyHtml;
                message.Body = p_body;
                message.Subject = p_subject;

                if (emailimagepath != "")
                {
                    ////Add Image to Email
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(p_body, null, "text/html");

                    try
                    {
                        LinkedResource logo = new LinkedResource(emailimagepath);
                        logo.ContentId = "companylogo";
                        htmlView.LinkedResources.Add(logo);
                    }
                    catch(Exception ex)
                    {
                    }

                    //add the views
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body.Replace("<img src=cid:companylogo>", "");
                }

                //MailAddress maddr = new MailAddress("sklakhiwal@gmail.com", "Sumnesh Lakhiwal");

                message.From = maddr;

                client.Host = mailSettings.Smtp.Network.Host;
                client.Port = mailSettings.Smtp.Network.Port;

                client.Send(message);
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteLogForSendEmailError(ex, null);
            }
        }

        public static void SendMailAsync(string p_subject, string p_body, string p_toList,
           MailAddress maddr, string emailImagepath, string attachement = Attachments)
        {
            SendAsync(p_subject, p_body, p_toList, null, null, true, maddr, attachement, emailImagepath);
        }

        private static void SendAsync(string p_subject, string p_body, string p_toList,
            string p_ccList, string p_bccList, bool p_isBodyHtml, MailAddress maddr, string attachement, string emailImagepath)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                if (!string.IsNullOrEmpty(p_toList))
                {
                    foreach (string to in p_toList.Split(';'))
                    {
                        message.To.Add(new MailAddress(to));
                    }
                }

                if (!string.IsNullOrEmpty(p_ccList))
                {
                    foreach (string cc in p_ccList.Split(';'))
                    {
                        message.CC.Add(new MailAddress(cc));
                    }
                }

                if (!string.IsNullOrEmpty(p_bccList))
                {
                    foreach (string bcc in p_bccList.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(bcc));
                    }
                }

                if (maddr != null)
                {
                    message.From = maddr;
                }

                if (emailImagepath != string.Empty)
                {
                    ////Add Image to Email
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(p_body, null, "text/html");

                    try
                    {
                        LinkedResource logo = new LinkedResource(emailImagepath);
                        logo.ContentId = "companylogo";
                        htmlView.LinkedResources.Add(logo);
                    }
                    catch(Exception ex)
                    {
                    }

                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body.Replace("<img src=cid:companylogo>", "");
                }

                message.IsBodyHtml = p_isBodyHtml;
                message.Body = p_body;
                message.Subject = p_subject;

                if (!string.IsNullOrEmpty(attachement))
                {
                    message.Attachments.Add(new System.Net.Mail.Attachment(attachement));
                }

                object userSetate = message;
                client.Host = mailSettings.Smtp.Network.Host;
                client.Port = mailSettings.Smtp.Network.Port;

                client.SendAsync(message, userSetate);
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteLogForSendEmailError(ex, null);
            }
        }

        static void EmbedImages()
        {
            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress("me@mycompany.com");
            mail.To.Add("you@yourcompany.com");

            //set the content
            mail.Subject = "This is an email";

            //first we create the Plain Text part
            AlternateView plainView = AlternateView.CreateAlternateViewFromString("This is my plain text content, viewable by those clients that don't support html", null, "text/plain");

            //then we create the Html part
            //to embed images, we need to use the prefix 'cid' in the img src value
            //the cid value will map to the Content-Id of a Linked resource.
            //thus <img src='cid:companylogo'> will map to a LinkedResource with a ContentId of 'companylogo'
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString("Here is an embedded image.<img src=cid:companylogo>", null, "text/html");

            //create the LinkedResource (embedded image)
            LinkedResource logo = new LinkedResource("c:\\temp\\logo.gif");
            logo.ContentId = "companylogo";
            //add the LinkedResource to the appropriate view
            htmlView.LinkedResources.Add(logo);

            //add the views
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);


            //send the message
            SmtpClient smtp = new SmtpClient("127.0.0.1"); //specify the mail server address
            smtp.Send(mail);
        }
        #endregion
    }
}
