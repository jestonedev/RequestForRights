﻿using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Models.ModelViews
{
    public class ResourceViewModel
    {
        public Resource Resource { get; set; }
        public IEnumerable<ResourceGroup> ResourceGroups { get; set; }
        public IEnumerable<Department> OwnerDepartments { get; set; }
        public ResourceViewModel()
        {
            Resource = new Resource();
        }
    }
}