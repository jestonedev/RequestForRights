using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels.Request
{
    public class RequestViewModel<T>
        where T: RequestUserModel
    {
        public RequestModel<T> RequestModel { get; set; }
        public IEnumerable<RequestExtComment> Comments { get; set; }
        public IEnumerable<RequestExecutorModel> Executors { get; set; }
        public IEnumerable<AclUser> WaitAgreementUsers { get; set; }
        public IEnumerable<RequestAgreement> SuccessAgreements { get; set; }
        public IEnumerable<RequestAgreement> CancelAgreements { get; set; }
        public IEnumerable<RequestAgreement> ExcludedAgreements { get; set; }
    }
}