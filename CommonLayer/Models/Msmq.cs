using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class Msmq
    {
        MessageQueue messageQueue = new MessageQueue();
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
            catch (Exception)
            {

                throw;
            }
        }

        //Delegate
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
                mailMessage.Body = token;
                mailMessage.Subject = "Fundoo Notes Password Reset Link";
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
