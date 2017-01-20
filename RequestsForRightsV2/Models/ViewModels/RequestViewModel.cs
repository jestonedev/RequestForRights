using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Models.ViewModels
{
    public class RequestViewModel<T>
        where T: RequestUserModel
    {
        public RequestModel<T> RequestModel { get; set; }
        public IEnumerable<RequestExtComment> Comments { get; set; }
        public IEnumerable<AclUser> WaitAgreementUsers { get; set; }
        public IEnumerable<RequestAgreement> SuccessAgreements { get; set; }
        public IEnumerable<RequestAgreement> CancelAgreements { get; set; }
    }
}