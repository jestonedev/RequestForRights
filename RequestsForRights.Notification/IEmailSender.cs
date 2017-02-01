using System.Collections.Generic;
using System.Net.Mail;

namespace RequestsForRights.Notification
{
    public interface IEmailSender
    {
        bool Send(IEnumerable<MailMessage> messages);
    }
}
