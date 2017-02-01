using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace RequestsForRights.Notification
{
    public class EmailSender : IEmailSender
    {
        private readonly int _smtpPort;
        private readonly string _smtpHost;

        public EmailSender(int smtpPort, string smtpHost)
        {
            _smtpPort = smtpPort;
            if (smtpHost == null)
            {
                throw new ArgumentNullException("smtpHost");
            }
            _smtpHost = smtpHost;
        }

        public bool Send(IEnumerable<MailMessage> messages)
        {
            var smtp = new SmtpClient(_smtpHost, _smtpPort);
            foreach (var message in messages)
            {
                try
                {
                    smtp.Send(message); 
                }
                catch (SmtpException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
