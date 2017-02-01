using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Notification
{
    public class EmailBuilder: IEmailBuilder
    {
        private readonly MailAddress _from;
        private readonly IRequestRepository _requestRepository;

        public EmailBuilder(MailAddress from, IRequestRepository requestRepository)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            _from = from;
            if (requestRepository == null)
            {
                throw new ArgumentNullException("requestRepository");
            }
            _requestRepository = requestRepository;
        }

        private string RequestDescriptionPart(Request request)
        {
            return !string.IsNullOrEmpty(request.Description) ? 
                string.Format("<br><b>Описание:</b><br>{0}", request.Description) : "";
        }

        public IEnumerable<MailMessage> CreateRequestEmails(Request request)
        {
            var messages = new List<MailMessage>();
            var requester = request.User;
            if (!string.IsNullOrEmpty(requester.Email))
            {
                var requestType = _requestRepository.GetRequestTypes().
                    Where(r => r.IdRequestType == request.IdRequestType);
                var requesterSubject =
                    string.Format("Ваша заявка №{0} {1} успешно создана",
                        request.IdRequest, requestType);
                var requesterBody = "Здравствуйте, {0}!<br>" +
                                    "Ваша заявка №{0} {1} успешно создана." + RequestDescriptionPart(request);
                var message = new MailMessage
                {
                    IsBodyHtml = true,
                    From = _from,
                    Subject = requesterSubject,
                    Body = requesterBody
                };
                message.To.Add(new MailAddress(requester.Email));
                messages.Add(message);
            }
            return messages;
        }

        public IEnumerable<MailMessage> UpdateRequestEmails(Request request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MailMessage> DeleteRequestEmails(Request request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MailMessage> SetRequestStateEmails(Request request, int idRequestStateType, string agreementReason)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MailMessage> AddCoordinatorEmails(Request request, Coordinator coordinator)
        {
            throw new NotImplementedException();
        }
    }
}
