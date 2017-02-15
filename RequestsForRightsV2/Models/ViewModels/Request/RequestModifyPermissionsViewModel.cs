using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels.Request
{
    public class RequestModifyPermissionsViewModel : RequestViewModel<RequestUserModel>
    {
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceRight> ResourceRights { get; set; }
        public IEnumerable<RequestRightGrantType> RequestRightGrantTypes { get; set; }
    }
}