using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Utilities.EmailNotify
{
    public class EmailBuilder: IEmailBuilder
    {
        private readonly MailAddress _from;
        private readonly IRequestService<RequestUserModel, 
            RequestViewModel<RequestUserModel>> _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _requestSecurityService;

        public EmailBuilder(MailAddress from, 
            IRequestService<RequestUserModel, RequestViewModel<RequestUserModel>> requestService,
            IRequestSecurityService<RequestUserModel> requestSecurityService)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            _from = from;
            if (requestService == null)
            {
                throw new ArgumentNullException("requestService");
            }
            _requestService = requestService;
            if (requestSecurityService == null)
            {
                throw new ArgumentNullException("requestSecurityService");
            }
            _requestSecurityService = requestSecurityService;
        }

        private string GetRequestDescriptionPart(Request request)
        {
            return !string.IsNullOrEmpty(request.Description) ?
                string.Format("<br><br><b>Описание:</b><br>{0}", request.Description.Replace("\n", "<br>")) : "";
        }

        private string GetRequestLink(Request request)
        {
            var host = "rqrights";
            var port = 80;
            if (HttpContext.Current != null)
            {
                host = HttpContext.Current.Request.Url.Host;
                port = HttpContext.Current.Request.Url.Port;
            }
            return
                string.Format(
                    "<br><br><b>Ссылка: </b><a href=\"http://{1}{2}/Request/Detail/{0}\">http://{1}{2}/Request/Detail/{0}</a>",
                    request.IdRequest, host, port == 80 ? "" : ":"+port);
        }

        private MailMessage CreateRequestRequesterEmail(Request request, 
            IList<AclUser> waitAgreementUsers)
        {
            var requester = request.User;
            var subject = string.Format("Ваша заявка №{0} {1} успешно создана",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}.", requester.Snp, subject);
            body += GetRequestDescriptionPart(request);
            if (waitAgreementUsers.Any())
            {
                body += "<br><br><b>Ожидается согласование следующих сотрудников:</b>";
                foreach (var user in waitAgreementUsers)
                {
                    body += "<br><b>ФИО: </b>" + user.Snp;
                    body += "<br><b>Департамент: </b>" + user.Department.Name;
                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        body += "<br><b>Телефон: </b>" + user.Phone;
                    }
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        body += string.Format("<br><b>Почтовый адрес: </b><a href=\"mailto:{0}\">{0}</a>", user.Email);
                    }
                    body += "<br>";
                }
            }
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(requester.Email));
            return message;
        }

        private MailMessage CreateRequestCoordinatorEmail(Request request, AclUser user)
        {
            var subject = string.Format("Создана заявка №{0} {1}", 
                request.IdRequest, 
                request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}, требующая вашего согласования.", 
                user.Snp, subject);
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        private MailMessage CreateRequestDispatcherEmail(Request request, AclUser user)
        {
            var subject = string.Format("Создана заявка №{0} {1}", 
                request.IdRequest,
                request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}. Данная заявка является автоматически согласованной.",
                user.Snp, subject);
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        public IEnumerable<MailMessage> CreateRequestEmails(Request request)
        {
            var messages = new List<MailMessage>();

            if (request.User.Roles.Any(r => r.IdRole == 1))
            {
                return messages;
            }
            
            var requester = request.User;
            var waitAgreementUsers = _requestService.GetWaitAgreementUsers(request.IdRequest,
                _requestService.GetRequestAgreements(request.IdRequest).ToList()).ToList();
            if (!string.IsNullOrEmpty(requester.Email))
            {
                var message = CreateRequestRequesterEmail(request, waitAgreementUsers);
                messages.Add(message);
            }
            foreach (var user in waitAgreementUsers)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    continue;
                }
                var message = CreateRequestCoordinatorEmail(request, user);
                messages.Add(message);
            }
            if (waitAgreementUsers.Any())
            {
                return messages;
            }
            
            var users = _requestSecurityService.GetUsersBy(AclRole.Dispatcher)
                .Union(_requestSecurityService.GetUsersBy(AclRole.Registrar));
            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    continue;
                }
                var message = CreateRequestDispatcherEmail(request, user);
                messages.Add(message);
            }
            return messages;
        }

        private MailMessage UpdateRequestRequesterEmail(Request request,
            IList<AclUser> waitAgreementUsers)
        {

            var requester = request.User; 
            var subject = string.Format("Ваша заявка №{0} {1} успешно изменена",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}.", requester.Snp, subject);
            body += GetRequestDescriptionPart(request);
            if (waitAgreementUsers.Any())
            {
                body += "<br><br><b>Ожидается согласование следующих сотрудников:</b>";
                foreach (var user in waitAgreementUsers)
                {
                    body += "<br><b>ФИО: </b>" + user.Snp;
                    body += "<br><b>Департамент: </b>" + user.Department.Name;
                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        body += "<br><b>Телефон: </b>" + user.Phone;
                    }
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        body += string.Format("<br><b>Почтовый адрес: </b><a href=\"mailto:{0}\">{0}</a>", user.Email);
                    }
                    body += "<br>";
                }
            }
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(requester.Email));
            return message;
        }

        private MailMessage UpdateRequestCoordinatorEmail(Request request, AclUser user)
        {
            var subject = string.Format("Заявка №{0} {1} была изменена",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1} и требует вашего согласования.",
                user.Snp, subject);
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        private MailMessage UpdateRequestDispatcherEmail(Request request, AclUser user)
        {
            var subject = string.Format("Создана заявка №{0} {1}",
                request.IdRequest,
                request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}. Данная заявка является автоматически согласованной.",
                user.Snp, subject);
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        public IEnumerable<MailMessage> UpdateRequestEmails(Request request)
        {
            var messages = new List<MailMessage>();

            if (request.User.Roles.Any(r => r.IdRole == 1))
            {
                return messages;
            }

            if (_requestSecurityService.InRole(AclRole.Administrator))
            {
                return messages;
            }
            var requester = request.User; 
            var waitAgreementUsers = _requestService.GetWaitAgreementUsers(request.IdRequest,
                _requestService.GetRequestAgreements(request.IdRequest).ToList()).ToList();
            if (!string.IsNullOrEmpty(requester.Email))
            {
                var message = UpdateRequestRequesterEmail(request, waitAgreementUsers);
                messages.Add(message);
            }
            foreach (var user in waitAgreementUsers)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    continue;
                }
                var message = UpdateRequestCoordinatorEmail(request, user);
                messages.Add(message);
            }
            if (request.RequestStates.OrderByDescending(r => r.IdRequestState).First().IdRequestStateType == 2)
            {
                var users = _requestSecurityService.GetUsersBy(AclRole.Dispatcher)
                    .Union(_requestSecurityService.GetUsersBy(AclRole.Registrar));
                foreach (var user in users)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        continue;
                    }
                    var message = UpdateRequestDispatcherEmail(request, user);
                    messages.Add(message);
                }
            }

            return messages;
        }

        private MailMessage DeleteRequestCoordinatorEmail(Request request, AclUser user)
        {
            var subject = string.Format("Заявка №{0} {1} была удалена",
                request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}.", user.Snp, subject);
            body += GetRequestDescriptionPart(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        private MailMessage DeleteRequestRequesterEmail(Request request)
        {
            var requester = request.User;
            var subject = string.Format("Ваша заявка №{0} {1} успешно удалена",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}.", requester.Snp, subject);
            body += GetRequestDescriptionPart(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(requester.Email));
            return message;
        }

        public IEnumerable<MailMessage> DeleteRequestEmails(Request request)
        {
            var messages = new List<MailMessage>();
            var requester = request.User;
            var waitAgreementUsers = _requestService.GetWaitAgreementUsers(request.IdRequest,
                _requestService.GetRequestAgreements(request.IdRequest).ToList()).ToList();
            if (!string.IsNullOrEmpty(requester.Email))
            {
                var message = DeleteRequestRequesterEmail(request);
                messages.Add(message);
            }
            foreach (var user in waitAgreementUsers)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    continue;
                }
                var message = DeleteRequestCoordinatorEmail(request, user);
                messages.Add(message);
            }
            return messages;
        }

        private MailMessage SetRequestStateRequesterEmail(Request request, 
            RequestStateType requestStateType, 
            string agreementReason)
        {
            var requester = request.User;
            var subject = string.Format("Изменен статус заявки №{0} {1}",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1} на <b>«{2}»</b>.",
                requester.Snp, subject, RequestHelper.VerbRequestState(requestStateType.Name).ToLower());
            if (!string.IsNullOrEmpty(agreementReason) && requestStateType.IdRequestStateType == 5)
            {
                body += "<br><br><b>Причина: </b>" + agreementReason;
            }
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(requester.Email));
            return message;
        }

        private MailMessage SetRequestStateDispatcherEmail(Request request,
            RequestStateType requestStateType,
            string agreementReason, AclUser user)
        {
            var subject = string.Format("Изменен статус заявки №{0} {1}",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1} на <b>«{2}»</b>.", user.Snp, subject,
                RequestHelper.VerbRequestState(requestStateType.Name).ToLower());
            if (requestStateType.IdRequestStateType == 2)
            {
                if (request.RequestAgreements.Any(r => r.IdAgreementType == 2) &&
                    request.RequestAgreements.Where(r => r.IdAgreementType == 2).
                    All(r => r.IdAgreementState != 1))
                {
                    subject = string.Format("По заявке №{0} {1} завершено дополнительное согласование",
                        request.IdRequest, request.RequestType.Name.ToLower());
                }
                else
                {
                    subject = string.Format("Поступила заявка №{0} {1}",
                        request.IdRequest, request.RequestType.Name.ToLower());
                }
                body = string.Format("Здравствуйте, {0}!<br>{1}.", user.Snp, subject);
            }
            if (!string.IsNullOrEmpty(agreementReason) && requestStateType.IdRequestStateType == 5)
            {
                body += "<br><br><b>Причина: </b>" + agreementReason;
            }
            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(user.Email));
            return message;
        }

        public IEnumerable<MailMessage> SetRequestStateEmails(Request request, int idRequestStateType, string agreementDescription)
        {
            var messages = new List<MailMessage>();

            if (request.User.Roles.Any(r => r.IdRole == 1))
            {
                return messages;
            }

            var lastRequestState = request.RequestStates.OrderByDescending(r => r.IdRequestState).
                FirstOrDefault();
            if (lastRequestState == null ||
                lastRequestState.IdRequestStateType == 4 ||
                (lastRequestState.IdRequestStateType != idRequestStateType && 
                !(lastRequestState.IdRequestStateType == 2 && idRequestStateType == 5)))
            {
                return messages;
            }
            var requester = request.User;
            if (!string.IsNullOrEmpty(requester.Email))
            {
                var message = SetRequestStateRequesterEmail(request, 
                    lastRequestState.RequestStateType, agreementDescription);
                messages.Add(message);
            }
            if (idRequestStateType == 2 ||
                (new[] {4, 5}.Contains(idRequestStateType) &&
                 request.RequestStates.Any(r => !r.Deleted && r.IdRequestStateType == 2)))
            {
                var users = _requestSecurityService.GetUsersBy(AclRole.Dispatcher)
                    .Union(_requestSecurityService.GetUsersBy(AclRole.Registrar));
                var userInfo = _requestSecurityService.GetUserInfo();
                foreach (var user in users)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        continue;
                    }
                    if (user.IdUser == userInfo.IdUser)
                    {
                        continue;
                    }
                    var message = SetRequestStateDispatcherEmail(request, 
                        lastRequestState.RequestStateType, agreementDescription, user);
                    messages.Add(message);
                }
            }
            return messages;
        }

        private MailMessage AddCoordinatorRequesterEmail(Request request, Coordinator coordinator)
        {
            var requester = request.User;
            var subject = string.Format("Ваша заявка №{0} {1} отправлена на дополнительное согласование",
                    request.IdRequest, request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}.", requester.Snp, subject);
            body += GetRequestDescriptionPart(request);
            
            body += "<br><br><b>Ожидается согласование от:</b>";
            body += "<br><b>ФИО: </b>" + coordinator.Snp;
            body += "<br><b>Департамент: </b>" + coordinator.Department;
            if (!string.IsNullOrEmpty(coordinator.Phone))
            {
                body += "<br><b>Телефон: </b>" + coordinator.Phone;
            }
            if (!string.IsNullOrEmpty(coordinator.Email))
            {
                body += string.Format("<br><b>Почтовый адрес: </b><a href=\"mailto:{0}\">{0}</a>", coordinator.Email);
            }
            body += "<br>";

            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(requester.Email));
            return message;
        }

        private MailMessage AddCoordinatorEmail(Request request, Coordinator coordinator, string sendDescription)
        {
            var subject = string.Format("Заявка №{0} {1} отправлена на дополнительное согласование",
                request.IdRequest,
                request.RequestType.Name.ToLower());
            var body = string.Format("Здравствуйте, {0}!<br>{1}. Требуется ваше согласование.",
                coordinator.Snp, subject);
            if (!string.IsNullOrEmpty(sendDescription))
            {
                body += string.Format("<br><br><b>Комментарий диспетчера:</b> {0}", sendDescription);
            }

            body += GetRequestDescriptionPart(request);
            body += GetRequestLink(request);
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = _from,
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(coordinator.Email));
            return message;
        }

        public IEnumerable<MailMessage> AddCoordinatorEmails(Request request, Coordinator coordinator, string sendDescription)
        {
            var messages = new List<MailMessage>();
            if (!string.IsNullOrEmpty(request.User.Email))
            {
                var message = AddCoordinatorRequesterEmail(request, coordinator);
                messages.Add(message);
            }
            if (!string.IsNullOrEmpty(coordinator.Email))
            {
                var message = AddCoordinatorEmail(request, coordinator, sendDescription);
                messages.Add(message);
            }
            return messages;
        }

        public IEnumerable<MailMessage> CreateSendTransferUserEmails(string requesterSnp, string requesterDepartment, string transferUserSnp,
            string transferToDepartment, string transferToUnit, string transferFromDepartment, string transferFromUnit)
        {
            var messages = new List<MailMessage>();
            var allRequesters = _requestSecurityService.GetUsersBy(AclRole.Requester).Include(r => r.Department)
                .Include(r => r.Department.ParentDepartment);
            List<AclUser> requesters;
            if (!string.IsNullOrEmpty(transferFromUnit))
            {
                requesters = allRequesters.Where(r => r.Department.Name == transferFromUnit &&
                                                   r.Department.ParentDepartment.Name == transferFromDepartment).ToList();
                if (!requesters.Any())
                {
                    requesters = allRequesters.Where(r => r.Department.Name == transferFromDepartment).ToList();
                }
            }
            else
            {
                requesters = allRequesters.Where(r => r.Department.Name == transferFromDepartment).ToList();
            }
            foreach (var requester in requesters)
            {
                if (string.IsNullOrEmpty(requester.Email))
                {
                    continue;
                }
                var transferFrom = string.Format("«{0}»", transferFromDepartment);
                if (!string.IsNullOrEmpty(transferFromUnit))
                {
                    transferFrom += string.Format(" («{0}»)", transferFromUnit);
                }
                var transferTo = string.Format("«{0}»", transferToDepartment);
                if (!string.IsNullOrEmpty(transferToUnit))
                {
                    transferTo += string.Format(" («{0}»)", transferToUnit);
                }

                const string subject = "Уведомление о необходимости отключения сотрудника";
                var body = string.Format(
                    "Здравствуйте, {3}!<br>"+
                    "В связи с переводом сотрудника «{0}» из организации {1} в организацию {2} " +
                    "убедительная просьба <a href=\"http://rqrights/Request/Create?IdRequestType=3\">подать заявку на отключение</a> " +
                    "данного сотрудника от информационных ресурсов вашей организации.<br>" +
                    "При подаче заявки в поле <strong>Примечание</strong> просьба указать <strong>«В связи с переводом в другую организацию»</strong><br><br>" +
                    "<strong style=\"color:#b94a48\">В случае игнорирования данного сообщение у " +
                    "сотрудника останется доступ к информационным ресурсам вашей организации.</strong><br><br>" +
                    "В случае, если данное письмо пришло вам по ошибке, просьба сообщить диспетчеру по телефону 349-671",
                    transferUserSnp, transferFrom,
                    transferTo, requester.Snp);
                var message = new MailMessage
                {
                    IsBodyHtml = true,
                    From = _from,
                    Subject = subject,
                    Body = body
                };
                message.To.Add(new MailAddress(requester.Email));
                messages.Add(message);
            }
            return messages;
        }
    }
}
