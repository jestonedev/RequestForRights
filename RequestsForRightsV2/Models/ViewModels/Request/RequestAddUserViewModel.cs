using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels.Request
{
    public class RequestAddUserViewModel : RequestViewModel<RequestUserModel>
    {
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Department> Units { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceRight> ResourceRights { get; set; }
    }
}