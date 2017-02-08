﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Extensions;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels;

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
            return filteredResources.OrderBy(filterOptions.SortDirection, filterOptions.SortField).
                Skip(filterOptions.PageSize*filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public ResourceIndexViewModel GetResourceIndexModelView(FilterOptions filterOptions,
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
            return new ResourceIndexViewModel
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
                ResourceGroups = _resourceRepository.GetResourceGroups().OrderBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name)
            };
        }

        public ResourceViewModel GetResourceViewModelBy(Resource resource)
        {
            return new ResourceViewModel
            {
                Resource = resource,
                ResourceGroups = _resourceRepository.GetResourceGroups().OrderBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name)
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
                    },
                    HasNotInternetAccess = true,
                    OperatorDepartment = GetDepartmentInfo(24),
                    IdOperatorDepartment = 24
                },
                ResourceGroups = _resourceRepository.GetResourceGroups().OrderBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name)
            };
        }

        public Resource DeleteResource(int idResource)
        {
            return _resourceRepository.DeleteResource(idResource);
        }

        public Resource UpdateResource(Resource resource, ResourceActFilesModel files)
        {
            PreInsertAndBindFiles(resource, files);
            return _resourceRepository.UpdateResource(resource);
        }

        public Resource InsertResource(Resource resource, ResourceActFilesModel files)
        {
            PreInsertAndBindFiles(resource, files);
            return _resourceRepository.InsertResource(resource);
        }

        private static void PreInsertAndBindFiles(Resource resource, ResourceActFilesModel files)
        {
            if (resource.ResourceAuthorityActs != null)
            {
                for (var i = 0; i < resource.ResourceAuthorityActs.Count; i++)
                {
                    var act = resource.ResourceAuthorityActs[i];
                    if (act.IdFile != null)
                    {
                        continue;
                    }
                    if (files.ResourceAuthorityActs.Count <= i)
                    {
                        break;
                    }
                    act.File = CreateActFile(files.ResourceAuthorityActs[i]);
                }
            }
            if (resource.ResourceOperatorActs != null)
            {
                for (var i = 0; i < resource.ResourceOperatorActs.Count; i++)
                {
                    var act = resource.ResourceOperatorActs[i];
                    if (act.IdFile != null)
                    {
                        continue;
                    }
                    if (files.ResourceOperatorActs.Count <= i)
                    {
                        break;
                    }
                    act.File = CreateActFile(files.ResourceOperatorActs[i]);
                }
            }
            if (resource.ResourceUsingActs != null)
            {
                for (var i = 0; i < resource.ResourceUsingActs.Count; i++)
                {
                    var act = resource.ResourceUsingActs[i];
                    if (act.IdFile != null)
                    {
                        continue;
                    }
                    if (files.ResourceUsingActs.Count <= i)
                    {
                        break;
                    }
                    act.File = CreateActFile(files.ResourceUsingActs[i]);
                }       
            }
            if (resource.ResourceOwnerPersons != null)
            {
                for (var i = 0; i < resource.ResourceOwnerPersons.Count; i++)
                {
                    var person = resource.ResourceOwnerPersons[i];
                    if (files.ResourceOwnerPersons[i] == null)
                    {
                        continue;
                    }
                    var filePerson = files.ResourceOwnerPersons[i];
                    if (person.Acts == null) continue;
                    for (var j = 0; j < person.Acts.Count; j++)
                    {
                        var act = person.Acts[j];
                        if (act.IdFile != null)
                        {
                            continue;
                        }
                        if (filePerson.Acts.Count <= j)
                        {
                            break;
                        }
                        act.File = CreateActFile(filePerson.Acts[j]);
                    }
                }
            }
            if (resource.ResourceOperatorPersons != null)
            {
                for (var i = 0; i < resource.ResourceOperatorPersons.Count; i++)
                {
                    var person = resource.ResourceOperatorPersons[i];
                    if (files.ResourceOperatorPersons[i] == null)
                    {
                        continue;
                    }
                    var filePerson = files.ResourceOperatorPersons[i];
                    if (person.Acts == null) continue;
                    for (var j = 0; j < person.Acts.Count; j++)
                    {
                        var act = person.Acts[j];
                        if (act.IdFile != null)
                        {
                            continue;
                        }
                        if (filePerson.Acts.Count <= j)
                        {
                            break;
                        }
                        act.File = CreateActFile(filePerson.Acts[j]);
                    }
                }
            }
        }

        private static ActFile CreateActFile(HttpPostedFileBase fileBase)
        {
            if (fileBase == null)
            {
                return null;
            }
            var file = new ActFile
            {
                FileContentType = fileBase.ContentType,
                FileOriginalName = fileBase.FileName
            };
            using (var reader = new BinaryReader(fileBase.InputStream))
            {
                file.FileContent = reader.ReadBytes(fileBase.ContentLength);
            }
            return file;
        }

        public int SaveChanges()
        {
            return _resourceRepository.SaveChanges();
        }

        public ActFile GetActFile(int idFile)
        {
            return _resourceRepository.GetActFile(idFile);
        }

        public Department GetDepartmentInfo(int idDepartment)
        {
            return _resourceRepository.GetDepartments()
                .FirstOrDefault(r => !r.Deleted && r.IdDepartment == idDepartment);
        }
    }
}