using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels.Request
{
    public class RequestDelegatePermissionsViewModel : RequestViewModel<RequestDelegatePermissionsUserModel>
    {
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceRight> ResourceRights { get; set; }
    }
}