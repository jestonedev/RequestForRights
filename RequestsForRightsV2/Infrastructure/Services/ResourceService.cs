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
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
        }

        public IEnumerable<Resource> GetVisibleResources(FilterOptions filterOptions)
        {
            return GetFilteredResources(filterOptions.Filter).
                OrderBy(filterOptions.SortField, filterOptions.SortDirection).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public ResourceIndexModelView GetResourceIndexModelView(FilterOptions filterOptions)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "Name";
            }
            return new ResourceIndexModelView
            {
                VisibleResources = GetVisibleResources(filterOptions),
                FilterOptions = filterOptions,
                ResourceCount = GetFilteredResources(filterOptions.Filter).Count()
            };
        }

        private IEnumerable<Resource> GetFilteredResources(string filter)
        {
            return _resourceRepository.GetResources().Where(filter);
        }

        public Resource GetResourceBy(int id)
        {
            return _resourceRepository.GetResourceById(id);
        }

        public ResourceViewModel GetResourceViewModelBy(int id)
        {
            return new ResourceViewModel
            {
                Resource = _resourceRepository.GetResourceById(id),
                ResourceGroups = _resourceRepository.GetResourceGroups(),
                OwnerDepartments = _resourceRepository.GetOwnerDepartments()
            };
        }

        public ResourceViewModel GetResourceViewModelBy(Resource resource)
        {
            return new ResourceViewModel
            {
                Resource = resource,
                ResourceGroups = _resourceRepository.GetResourceGroups(),
                OwnerDepartments = _resourceRepository.GetOwnerDepartments()
            };
        }

        public ResourceViewModel GetEmptyResourceViewModel()
        {
            return new ResourceViewModel
            {
                Resource = new Resource
                {
                    ResourceRights = new List<ResourceRight>
                    {
                        new ResourceRight()
                    }
                },
                ResourceGroups = _resourceRepository.GetResourceGroups(),
                OwnerDepartments = _resourceRepository.GetOwnerDepartments()
            };
        }

        public Resource DeleteResource(int idResource)
        {
            return _resourceRepository.DeleteResource(idResource);
        }

        public Resource UpdateResource(Resource resource)
        {
            return _resourceRepository.UpdateResource(resource);
        }

        public Resource InsertResource(Resource resource)
        {
            return _resourceRepository.InsertResource(resource);
        }

        public int SaveChanges()
        {
            return _resourceRepository.SaveChanges();
        }
    }
}