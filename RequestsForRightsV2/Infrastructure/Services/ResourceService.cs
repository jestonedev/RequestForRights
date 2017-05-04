using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Extensions;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Infrastructure.Services
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
            var departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name).ToList();
            var resource = _resourceRepository.GetResourceById(id);
            return new ResourceViewModel
            {
                Resource = resource,
                ResourceGroups =
                    _resourceRepository.GetResourceGroups().OrderBy(r => r.IdResourceGroup == 5).ThenBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = departments,
                RequestPermissionsDepartments = departments.Where(r => !r.IsAlienDepartment).Select(r =>
                    new RequestPermissionsDepartmentsModel
                    {
                        IdDepartment = r.IdDepartment,
                        DepartmentName = r.Name,
                        RequestsAllowed = resource.RequestAllowedDepartments.Select(rd => rd.IdDepartment).Contains(r.IdDepartment)
                    }).ToList()
            };
        }

        public ResourceViewModel GetResourceViewModelBy(Resource resource, IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
        {
            return new ResourceViewModel
            {
                Resource = resource,
                ResourceGroups = _resourceRepository.GetResourceGroups().OrderBy(r => r.IdResourceGroup == 5).ThenBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name).ToList(),
                RequestPermissionsDepartments = requestPermissionsDepartments
            };
        }

        public ResourceViewModel GetEmptyResourceViewModel()
        {
            var departments = _resourceRepository.GetDepartments().OrderBy(r => r.Name).ToList();

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
                    IdOperatorDepartment = 24,
                    OwnerDepartment = GetDepartmentInfo(1),
                    IdOwnerDepartment = 1
                },
                ResourceGroups = _resourceRepository.GetResourceGroups().OrderBy(r => r.IdResourceGroup == 5).ThenBy(r => r.Name),
                ResourceInformationTypes = _resourceRepository.GetResourceInformationTypes().OrderBy(r => r.Name),
                Departments = departments,
                RequestPermissionsDepartments = departments.Select(
                    r => new RequestPermissionsDepartmentsModel
                    {
                        IdDepartment = r.IdDepartment,
                        DepartmentName = r.Name,
                        RequestsAllowed = false
                    }).ToList()
            };
        }

        public Resource DeleteResource(int idResource)
        {
            return _resourceRepository.DeleteResource(idResource);
        }

        public Resource UpdateResource(Resource resource, ResourceActFilesModel files, IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
        {
            PreInsertAndBindFiles(resource, files);
            UpdateRequestPermissionsDepartments(resource, requestPermissionsDepartments);
            return _resourceRepository.UpdateResource(resource);
        }

        public Resource InsertResource(Resource resource, ResourceActFilesModel files, IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
        {
            PreInsertAndBindFiles(resource, files);
            UpdateRequestPermissionsDepartments(resource, requestPermissionsDepartments);
            return _resourceRepository.InsertResource(resource);
        }

        private static void UpdateRequestPermissionsDepartments(Resource resource, IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
        {
            var newAllowedDepartmentIds =
                requestPermissionsDepartments.Where(r => r.RequestsAllowed).Select(r => r.IdDepartment).Distinct().ToList();
            if (resource.RequestAllowedDepartments == null)
            {
                resource.RequestAllowedDepartments = new List<Department>();
            }
            foreach (var allowedDepartment in resource.RequestAllowedDepartments)
                {
                    if (newAllowedDepartmentIds.Contains(allowedDepartment.IdDepartment))
                    {
                        newAllowedDepartmentIds.Remove(allowedDepartment.IdDepartment);
                        continue;
                    }
                    resource.RequestAllowedDepartments.Remove(allowedDepartment);
                }
            foreach (var newAllowedDepartmentId in newAllowedDepartmentIds)
            {
                resource.RequestAllowedDepartments.Add(new Department
                {
                    IdDepartment = newAllowedDepartmentId
                });
            }
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