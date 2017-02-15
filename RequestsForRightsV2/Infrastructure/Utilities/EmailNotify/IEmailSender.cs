using System.Collections.Generic;
using System.Net.Mail;

namespace RequestsForRights.Web.Infrastructure.Utilities.EmailNotify
{
    public interface IEmailSender
    {
        bool Send(IEnumerable<MailMessage> messages);
    }
}
