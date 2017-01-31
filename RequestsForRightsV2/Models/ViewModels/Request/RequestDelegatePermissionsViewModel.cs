using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Models.ViewModels.Request
{
    public class RequestDelegatePermissionsViewModel : RequestViewModel<RequestDelegatePermissionsUserModel>
    {
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceRight> ResourceRights { get; set; }
    }
}