using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Models.ViewModels.Request
{
    public class RequestModifyPermissionsViewModel : RequestViewModel<RequestUserModel>
    {
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceRight> ResourceRights { get; set; }
        public IEnumerable<RequestRightGrantType> RequestRightGrantTypes { get; set; }
    }
}