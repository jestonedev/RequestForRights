﻿using System.Collections.Generic;
using System.Net.Mail;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Infrastructure.Utilities.EmailNotify
{
    public interface IEmailBuilder
    {
        IEnumerable<MailMessage> CreateRequestEmails(Request request);
        IEnumerable<MailMessage> UpdateRequestEmails(Request request);
        IEnumerable<MailMessage> DeleteRequestEmails(Request request);
        IEnumerable<MailMessage> SetRequestStateEmails(Request request, int idRequestStateType, string agreementDescription);
        IEnumerable<MailMessage> AddCoordinatorEmails(Request request, Coordinator coordinator, string sendDescription);
        IEnumerable<MailMessage> CreateSendTransferUserEmails(string requesterSnp, 
            string requesterDepartment, string transferUserSnp, 
            string transferToDepartment, string transferToUnit, 
            string transferFromDepartment, string transferFromUnit);
    }
}
