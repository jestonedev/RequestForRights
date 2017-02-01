using System.Collections.Generic;
using System.Net.Mail;

namespace RequestsForRights.Infrastructure.Utilities.EmailNotify
{
    public interface IEmailSender
    {
        bool Send(IEnumerable<MailMessage> messages);
    }
}
