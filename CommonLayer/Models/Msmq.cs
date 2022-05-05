using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class for microsoft messaging queue
    /// </summary>
    public class Msmq
    {
        /// <summary>
        /// The message queue object
        /// </summary>
        MessageQueue messageQueue = new MessageQueue();

        /// <summary>
        /// Method to Send the message.
        /// </summary>
        /// <param name="token">The token.</param>
        public void SendMessage(string token)
        {
            messageQueue.Path = @".\Private$\Token";
            try
            {
                if (!MessageQueue.Exists(messageQueue.Path))
                {
                    MessageQueue.Create(messageQueue.Path);
                }
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
                messageQueue.Send(token);
                messageQueue.BeginReceive();
                messageQueue.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Handles the ReceiveCompleted event of the MessageQueue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReceiveCompletedEventArgs"/> instance containing the event data.</param>
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("guruprasad.testmail@gmail.com", "Testmail@36")
                };
                mailMessage.From = new MailAddress("guruprasad.testmail@gmail.com");
                mailMessage.To.Add(new MailAddress("guruprasad.testmail@gmail.com"));

                //mailMessage.Body = "Please click on the link to Reset your password: \n\n"+ "http://localhost:4200/reset-password" + "\n" + token +
                //                   "\n\nThe link is valid for 1 hour";

                mailMessage.Body = $"<!DOCTYPE html>" +
                                   $"<html>" +
                                   $"<html lang=\"en\">" +
                                    $"<head>" +
                                    $"<meta charset=\"UTF - 8\">" +
                                    $"</head>" +
                                    $"<body>" +
                                    $"<h2> Dear Fundoo User, </h2>\n" +
                                    $"<h3> Please click on the below link to reset password</h3>" +
                                    $"<a href='http://localhost:4200/reset-password/{token}'> ClickHere </a>\n " +
                                    $"<h3 style = \"color: #EA4335\"> \nThe link is valid for 24 hour </h3>" +
                                    $"</body>" +
                                   $"</html>";


                mailMessage.Subject = "Fundoo Notes Password Reset Link";
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
