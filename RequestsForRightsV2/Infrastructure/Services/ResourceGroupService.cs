using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;
using RequestsForRightsV2.Models.ModelViews;

namespace RequestsForRightsV2.Infrastructure.Services
{
    public class ResourceGroupService : IResourceGroupService
    {
        private readonly IResourceGroupRepository _resourceGroupRepository;

        public ResourceGroupService(IResourceGroupRepository resourceGroupRepository)
        {
            if (resourceGroupRepository == null)
            {
                throw new ArgumentNullException("resourceGroupRepository");
            }
            _resourceGroupRepository = resourceGroupRepository;
        }

        public IEnumerable<ResourceGroup> GetVisibleResourceGroups(FilterOptions filterOptions)
        {
            return GetFilteredResourceGroups(filterOptions.Filter).
                OrderBy(filterOptions.SortField, filterOptions.SortDirection).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public ResourceGroupIndexModelView GetResourceGroupIndexModelView(FilterOptions filterOptions)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "Name";
            }
            return new ResourceGroupIndexModelView
            {
                VisibleResourceGroups = GetVisibleResourceGroups(filterOptions),
                FilterOptions = filterOptions,
                ResourceGroupCount = GetFilteredResourceGroups(filterOptions.Filter).Count()
            };
        }

        private IEnumerable<ResourceGroup> GetFilteredResourceGroups(string filter)
        {
            return _resourceGroupRepository.GetResourceGroups().Where(filter);
        }

        public ResourceGroup DeleteResourceGroup(int idResourceGroup)
        {
            return _resourceGroupRepository.DeleteResourceGroup(idResourceGroup);
        }

        public int SaveChanges()
        {
            return _resourceGroupRepository.SaveChanges();
        }

        public ResourceGroup GetResourceGroupBy(int id)
        {
            return _resourceGroupRepository.GetResourceGroupById(id);
        }

        public ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup)
        {
            return _resourceGroupRepository.UpdateResourceGroup(resourceGroup);
        }

        public ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup)
        {
            return _resourceGroupRepository.InsertResourceGroup(resourceGroup);
        }
    }
}