using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Infrastructure.Services
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

        public IQueryable<Resource> GetVisibleResources(FilterOptions filterOptions,
            IQueryable<Resource> filteredResources)
        {
            return Order(filteredResources, filterOptions.SortField, filterOptions.SortDirection).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        private static IQueryable<Resource> Order(IQueryable<Resource> resources,
            string sortField, SortDirection sortDirection)
        {
            switch (sortField)
            {
                case "Name":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return resources.OrderBy(r => r.Name);
                        case SortDirection.Desc:
                            return resources.OrderByDescending(r => r.Name);
                        default:
                            return resources;
                    }
                case "Description":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return resources.OrderBy(r => r.Description);
                        case SortDirection.Desc:
                            return resources.OrderByDescending(r => r.Description);
                        default:
                            return resources;
                    }
                case "ResourceGroup.Name":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return resources.OrderBy(r => r.ResourceGroup.Name);
                        case SortDirection.Desc:
                            return resources.OrderByDescending(r => r.ResourceGroup.Name);
                        default:
                            return resources;
                    }
                default:
                    return resources;
            }
        }

        public ResourceIndexModelView GetResourceIndexModelView(FilterOptions filterOptions,
            IQueryable<Resource> filteredResources)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "Name";
            }
            var resources = GetVisibleResources(filterOptions, filteredResources).ToList();
            if (!resources.Any())
            {
                filterOptions.PageIndex = 0;
                resources = GetVisibleResources(filterOptions, filteredResources).ToList();
            }
            return new ResourceIndexModelView
            {
                VisibleResources = resources,
                FilterOptions = filterOptions,
                ResourceCount = filteredResources.Count()
            };
        }

        public IQueryable<Resource> GetFilteredResources(string filter)
        {
            var resources = _resourceRepository.GetResources();
            if (string.IsNullOrEmpty(filter))
            {
                return resources;
            }
            return resources.Where(r => 
                r.Name.ToLower().Contains(filter) ||
                r.Description.ToLower().Contains(filter) ||
                r.ResourceGroup.Name.ToLower().Contains(filter));
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