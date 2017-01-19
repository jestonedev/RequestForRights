using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Models.ModelViews
{
    public class RequestViewModel<T>
        where T: RequestUserModel
    {
        public RequestModel<T> RequestModel { get; set; }
        public IEnumerable<RequestExtComment> Comments { get; set; }
        public IEnumerable<RequestAgreement> Agreements { get; set; }
    }
}