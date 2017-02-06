using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Models.ViewModels
{
    public class ResourceViewModel
    {
        public Resource Resource { get; set; }
        public IEnumerable<ResourceGroup> ResourceGroups { get; set; }
        public IEnumerable<ResourceInformationType> ResourceInformationTypes { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public ResourceViewModel()
        {
            Resource = new Resource();
        }
    }
}