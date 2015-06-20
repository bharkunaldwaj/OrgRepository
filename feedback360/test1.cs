using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

/// <summary>
/// Summary description for Sending_Emails
/// </summary>
public class Sending_Emails
{
	public Sending_Emails()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bool sending_Mail(send_Mail_Entity obj)
    {
        try
        {
            if (obj.From == "" || obj.From == null || obj.To == "" || obj.To == null)
                return false;

            else
            {
                //MailMessage msgMail = new MailMessage();
                //msgMail.To = obj.To;
                //msgMail.From = obj.From;
                //msgMail.Subject = obj.subject;

                //msgMail.BodyFormat = MailFormat.Html;


                //msgMail.Body = obj.Body;
                //if (obj.Cc != "" || obj.Cc != null)
                //    msgMail.Cc = obj.Cc;
                //if (obj.Attachment != "" && obj.Attachment != null)
                //    msgMail.Attachments.Add(new MailAttachment(HttpContext.Current.Server.MapPath(obj.Attachment)));
                //SmtpMail.Send(msgMail);


                // second code

                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(obj.From);
                message.From = fromAddress;
                message.To.Add(obj.To);
                if (obj.Cc != "" && obj.Cc != null)
                {
                    message.CC.Add(obj.Cc);
                }
                message.Subject = obj.subject;
                message.Body = obj.Body;

                if (obj.Attachment != "" && obj.Attachment != null)
                {
                    string[] arrAtt = obj.Attachment.Split(',');

                    foreach (string fileAtt in arrAtt)
                    {
                        try
                        {
                            //Attachment att = new Attachment(fileAtt);
                            message.Attachments.Add(new Attachment(fileAtt));
                        }
                        catch
                        {

                        }
                    }
                }
                message.IsBodyHtml = obj.IsBodyHtml;
                SmtpClient client = new SmtpClient();
                client.Host = "172.29.8.75";
                client.Send(message);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
