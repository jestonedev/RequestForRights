using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace RequestsForRights.Infrastructure.Utilities.EmailNotify
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
            using (var smtp = new SmtpClient(_smtpHost, _smtpPort))
            {
                foreach (var message in messages)
                {
                    try
                    {
                        message.SubjectEncoding = Encoding.Default;
                        smtp.Send(message);
                    }
                    catch (SmtpException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
