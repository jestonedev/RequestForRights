﻿using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IResourceGroupService
    {
        IQueryable<ResourceGroup> GetVisibleResourceGroups(FilterOptions filterOptions,
            IQueryable<ResourceGroup> filteredResourceGroups);
        IQueryable<ResourceGroup> GetFilteredResourceGroups(string filter);
        ResourceGroupIndexViewModel GetResourceGroupIndexModelView(FilterOptions filterOptions,
            IQueryable<ResourceGroup> filteredResourceGroups);
        ResourceGroup DeleteResourceGroup(int idResourceGroup);
        ResourceGroup GetResourceGroupBy(int id);
        ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup);
        ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup);
        int SaveChanges();
    }
}
