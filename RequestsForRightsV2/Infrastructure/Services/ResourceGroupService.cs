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
        private readonly ISecurityService _securityService;

        public ResourceGroupService(IResourceGroupRepository resourceGroupRepository, ISecurityService securityService)
        {
            if (resourceGroupRepository == null)
            {
                throw new ArgumentNullException("resourceGroupRepository");
            }
            _resourceGroupRepository = resourceGroupRepository;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
        }

        public IEnumerable<ResourceGroup> GetResourceGroups(FilterOptions filterOptions)
        {
            return _resourceGroupRepository.GetResourceGroups().Where(filterOptions.Filter).
                OrderBy(filterOptions.SortField, filterOptions.SortDirection).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public ResourceGroupIndexModelView GetResourceGroupIndexModelView(FilterOptions filterOptions)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "IdResourceGroup";
            }
            return new ResourceGroupIndexModelView
            {
                ResourceGroups = GetResourceGroups(filterOptions),
                FilterOptions = filterOptions
            };
        }

        public ResourceGroup DeleteResourceGroup(int idResourceGroup)
        {
            return _resourceGroupRepository.DeleteResourceGroup(idResourceGroup);
        }

        public int SaveChanges()
        {
            return _resourceGroupRepository.SaveChanges();
        }
    }
}