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

        public IEnumerable<Resource> GetResources(FilterOptions filterOptions)
        {
            return _resourceRepository.GetResources().Where(filterOptions.Filter).
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
                FilteredResources = GetResources(filterOptions),
                FilterOptions = filterOptions,
                ResourceCount = _resourceRepository.GetResources().Count()
            };
        }

        public Resource DeleteResource(int idResource)
        {
            return _resourceRepository.DeleteResource(idResource);
        }

        public int SaveChanges()
        {
            return _resourceRepository.SaveChanges();
        }

        public Resource GetResourceById(int id)
        {
            return _resourceRepository.GetResourceById(id);
        }

        public Resource UpdateResource(Resource resource)
        {
            return _resourceRepository.UpdateResource(resource);
        }

        public Resource InsertResource(Resource resource)
        {
            return _resourceRepository.InsertResource(resource);
        }
    }
}