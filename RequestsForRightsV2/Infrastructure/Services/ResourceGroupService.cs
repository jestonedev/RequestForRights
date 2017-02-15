using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Extensions;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Infrastructure.Services
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

        public IQueryable<ResourceGroup> GetVisibleResourceGroups(FilterOptions filterOptions,
            IQueryable<ResourceGroup> filteredResourceGroups)
        {
            return filteredResourceGroups.OrderBy(filterOptions.SortDirection, filterOptions.SortField).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public ResourceGroupIndexViewModel GetResourceGroupIndexModelView(FilterOptions filterOptions, IQueryable<ResourceGroup> filteredResourceGroups)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "Name";
            }
            var resourceGroup = GetVisibleResourceGroups(filterOptions, filteredResourceGroups).ToList();
            if (!resourceGroup.Any())
            {
                filterOptions.PageIndex = 0;
                resourceGroup = GetVisibleResourceGroups(filterOptions, filteredResourceGroups).ToList();
            }
            return new ResourceGroupIndexViewModel
            {
                VisibleResourceGroups = resourceGroup, FilterOptions = filterOptions, ResourceGroupCount = filteredResourceGroups.Count()
            };
        }

        public IQueryable<ResourceGroup> GetFilteredResourceGroups(string filter)
        {
            var resourceGroups = _resourceGroupRepository.GetResourceGroups();
            if (string.IsNullOrEmpty(filter))
            {
                return resourceGroups;
            }
            return resourceGroups.Where(r => r.Name.ToLower().Contains(filter) || r.Description.ToLower().Contains(filter));
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