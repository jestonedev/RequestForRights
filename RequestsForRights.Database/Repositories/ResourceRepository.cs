using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ResourceRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IQueryable<Resource> GetResources()
        {
            return _databaseContext.Resources
                .Include(r => r.ResourceGroup)
                .Include(r => r.ResourceRights)
                .Include(r => r.RequestAllowedDepartments)
                .Where(r => !r.Deleted);
        }

        public IQueryable<ResourceRight> GetResourceRights()
        {
            return _databaseContext.ResourceRights.Include(r => r.Resource).Where(r => !r.Deleted &&
                                                                                       !r.Resource.Deleted);
        }

        public Resource DeleteResource(int idResource)
        {
            var resource = GetResourceById(idResource);
            if (resource == null) return null;
            resource.Deleted = true;
            return resource;
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        public ActFile GetActFile(int idFile)
        {
            return _databaseContext.ActFiles.Find(idFile);
        }

        public Resource GetResourceById(int id)
        {
            return _databaseContext.Resources.Include(r => r.OwnerDepartment)
                .Include(r => r.OperatorDepartment)
                .Include(r => r.ResourceAuthorityActs)
                .Include(r => r.ResourceOperatorActs)
                .Include(r => r.ResourceUsingActs)
                .Include(r => r.ResourceOperatorPersons)
                .Include(r => r.ResourceOwnerPersons)
                .Include(r => r.ResourceOwnerPersons.Select(p => p.Acts))
                .Include(r => r.ResourceOperatorPersons.Select(p => p.Acts))
                .Include(r => r.ResourceRights)
                .Include(r => r.ResourceInternetAddresses)
                .Include(r => r.ResourceDeviceAddresses)
                .FirstOrDefault(r => r.IdResource == id);
        }

        public Resource UpdateResource(Resource resource)
        {
            if (resource.OwnerDepartment != null)
            {
                resource.OwnerDepartment.IdDepartment = resource.IdOwnerDepartment;
                UpdateDepartmentExtInfo(resource.OwnerDepartment);
                resource.OwnerDepartment = null;
            }
            if (resource.OperatorDepartment != null)
            {
                resource.OperatorDepartment.IdDepartment = resource.IdOperatorDepartment;
                UpdateDepartmentExtInfo(resource.OperatorDepartment);
                resource.OperatorDepartment = null;
            }
            var res = _databaseContext.Resources.Find(resource.IdResource);
            _databaseContext.Entry(res).CurrentValues.SetValues(resource);
            UpdateResoureceRights(
                res.ResourceRights.Where(r => !r.Deleted), 
                resource.ResourceRights, resource.IdResource);
            UpdateResourceInternetAddresses(
                res.ResourceInternetAddresses.Where(r => !r.Deleted),
                resource.ResourceInternetAddresses, resource.IdResource);
            UpdateResourceDeviceAddresses(
                res.ResourceDeviceAddresses.Where(r => !r.Deleted),
                resource.ResourceDeviceAddresses, resource.IdResource);
            UpdateResourceAuthorityActs(
                res.ResourceAuthorityActs.Where(r => !r.Deleted),
                resource.ResourceAuthorityActs, resource.IdResource);
            UpdateResourceUsingActs(
                res.ResourceUsingActs.Where(r => !r.Deleted),
                resource.ResourceUsingActs, resource.IdResource);
            UpdateResourceOperatorActs(
                res.ResourceOperatorActs.Where(r => !r.Deleted),
                resource.ResourceOperatorActs, resource.IdResource);
            UpdateResourceOwnerPersons(
                res.ResourceOwnerPersons.Where(r => !r.Deleted),
                resource.ResourceOwnerPersons, resource.IdResource);
            UpdateResourceOperatorPersons(
                res.ResourceOperatorPersons.Where(r => !r.Deleted),
                resource.ResourceOperatorPersons, resource.IdResource);
            UpdateRequestAllowedDepartments(res.RequestAllowedDepartments, resource.RequestAllowedDepartments);
            return res;
        }

        private void UpdateRequestAllowedDepartments(IList<Department> actualAllowedDepartments, IList<Department> newAllowedDepartments)
        {
            var newAllowedDepartmentIds =
                newAllowedDepartments.Select(r => r.IdDepartment).ToList();
            foreach (var actualAllowedDepartment in actualAllowedDepartments.ToList())
            {
                if (newAllowedDepartmentIds.Contains(actualAllowedDepartment.IdDepartment))
                {
                    newAllowedDepartmentIds.Remove(actualAllowedDepartment.IdDepartment);
                    continue;
                }
                actualAllowedDepartments.Remove(actualAllowedDepartment);
            }
            foreach (var newAllowedDepartmentId in newAllowedDepartmentIds)
            {
                actualAllowedDepartments.Add(_databaseContext.Departments.Find(newAllowedDepartmentId));
            }
        }

        private void UpdateResourceOperatorPersons(IEnumerable<ResourceOperatorPerson> oldPersons,
            IEnumerable<ResourceOperatorPerson> newPersons, int idResource)
        {
            var newPersonsList = newPersons == null ? new List<ResourceOperatorPerson>() : newPersons.ToList();
            newPersonsList.ForEach(r => r.IdResource = idResource);
            foreach (var person in oldPersons)
            {
                if (newPersonsList.Any(r => r.IdResourceOperatorPerson == person.IdResourceOperatorPerson))
                    continue;
                person.Deleted = true;
                _databaseContext.ResourceOperatorPersons.Attach(person);
                _databaseContext.Entry(person).State = EntityState.Modified;
            }

            foreach (var person in newPersonsList)
            {
                if (person.IdResourceOperatorPerson == default(int))
                {
                    _databaseContext.ResourceOperatorPersons.Add(person);
                }
                else
                {
                    var findedPerson = _databaseContext.ResourceOperatorPersons.Find(person.IdResourceOperatorPerson);
                    _databaseContext.Entry(findedPerson).CurrentValues.SetValues(person);
                    UpdateResourceOperatorPersonsActs(
                        findedPerson.Acts.Where(r => !r.Deleted), person.Acts,
                        findedPerson.IdResourceOperatorPerson);
                }
            }
        }

        private void UpdateResourceOperatorPersonsActs(IEnumerable<ResourceOperatorPersonAct> oldActs,
            IEnumerable<ResourceOperatorPersonAct> newActs, int idResourceOperatorPerson)
        {
            var newActsList = newActs == null ? new List<ResourceOperatorPersonAct>() : newActs.ToList();
            newActsList.ForEach(r => r.IdResourceOperatorPerson = idResourceOperatorPerson);
            foreach (var act in oldActs)
            {
                if (newActsList.Any(r => r.IdResourceOperatorPerson == act.IdResourceOperatorPerson))
                    continue;
                act.Deleted = true;
                _databaseContext.ResourceOperatorPersonActs.Attach(act);
                _databaseContext.Entry(act).State = EntityState.Modified;
            }

            foreach (var act in newActsList)
            {
                if (act.IdResourceOperatorPersonAct == default(int))
                {
                    _databaseContext.ResourceOperatorPersonActs.Add(act);
                }
                else
                {
                    var findedAct = _databaseContext.ResourceOperatorPersonActs.Find(act.IdResourceOperatorPersonAct);
                    _databaseContext.Entry(findedAct).CurrentValues.SetValues(act);
                    if (act.File != null)
                    {
                        findedAct.File = _databaseContext.ActFiles.Add(act.File);
                    }
                }
            }
        }

        private void UpdateResourceOwnerPersons(IEnumerable<ResourceOwnerPerson> oldPersons,
            IEnumerable<ResourceOwnerPerson> newPersons, int idResource)
        {
            var newPersonsList = newPersons == null ? new List<ResourceOwnerPerson>() : newPersons.ToList();
            newPersonsList.ForEach(r => r.IdResource = idResource);
            foreach (var person in oldPersons)
            {
                if (newPersonsList.Any(r => r.IdResourceOwnerPerson == person.IdResourceOwnerPerson))
                    continue;
                person.Deleted = true;
                _databaseContext.ResourceOwnerPersons.Attach(person);
                _databaseContext.Entry(person).State = EntityState.Modified;
            }

            foreach (var person in newPersonsList)
            {
                if (person.IdResourceOwnerPerson == default(int))
                {
                    _databaseContext.ResourceOwnerPersons.Add(person);
                }
                else
                {
                    var findedPerson = _databaseContext.ResourceOwnerPersons.Find(person.IdResourceOwnerPerson);
                    _databaseContext.Entry(findedPerson).CurrentValues.SetValues(person);
                    UpdateResourceOwnerPersonsActs(findedPerson.Acts.Where(r => !r.Deleted), person.Acts,
                        findedPerson.IdResourceOwnerPerson);
                }
            }
        }

        private void UpdateResourceOwnerPersonsActs(IEnumerable<ResourceOwnerPersonAct> oldActs,
            IEnumerable<ResourceOwnerPersonAct> newActs, int idResourceOwnerPerson)
        {
            var newActsList = newActs == null ? new List<ResourceOwnerPersonAct>() : newActs.ToList();
            newActsList.ForEach(r => r.IdResourceOwnerPerson = idResourceOwnerPerson);
            foreach (var act in oldActs)
            {
                if (newActsList.Any(r => r.IdResourceOwnerPersonAct == act.IdResourceOwnerPersonAct))
                    continue;
                act.Deleted = true;
                _databaseContext.ResourceOwnerPersonActs.Attach(act);
                _databaseContext.Entry(act).State = EntityState.Modified;
            }

            foreach (var act in newActsList)
            {
                if (act.IdResourceOwnerPersonAct == default(int))
                {
                    _databaseContext.ResourceOwnerPersonActs.Add(act);
                }
                else
                {
                    var findedAct = _databaseContext.ResourceOwnerPersonActs.Find(act.IdResourceOwnerPersonAct);
                    _databaseContext.Entry(findedAct).CurrentValues.SetValues(act);
                    if (act.File != null)
                    {
                        findedAct.File = _databaseContext.ActFiles.Add(act.File);
                    }
                }
            }
        }

        private void UpdateResourceAuthorityActs(IEnumerable<ResourceAuthorityAct> oldActs,
            IEnumerable<ResourceAuthorityAct> newActs, int idResource)
        {
            var newActsList = newActs == null ? new List<ResourceAuthorityAct>() : newActs.ToList();
            newActsList.ForEach(r => r.IdResource = idResource);
            foreach (var act in oldActs)
            {
                if (newActsList.Any(r => r.IdResourceAuthorityAct == act.IdResourceAuthorityAct))
                    continue;
                act.Deleted = true;
                _databaseContext.ResourceAuthorityActs.Attach(act);
                _databaseContext.Entry(act).State = EntityState.Modified;
            }

            foreach (var act in newActsList)
            {
                if (act.IdResourceAuthorityAct == default(int))
                {
                    _databaseContext.ResourceAuthorityActs.Add(act);
                }
                else
                {
                    var findedAct = _databaseContext.ResourceAuthorityActs.Find(act.IdResourceAuthorityAct);
                    _databaseContext.Entry(findedAct).CurrentValues.SetValues(act);
                    if (act.File != null)
                    {
                        findedAct.File = _databaseContext.ActFiles.Add(act.File);
                    }
                }
            }
        }

        private void UpdateResourceUsingActs(IEnumerable<ResourceUsingAct> oldActs,
            IEnumerable<ResourceUsingAct> newActs, int idResource)
        {
            var newActsList = newActs == null ? new List<ResourceUsingAct>() : newActs.ToList();
            newActsList.ForEach(r => r.IdResource = idResource);
            foreach (var act in oldActs)
            {
                if (newActsList.Any(r => r.IdResourceUsingAct == act.IdResourceUsingAct))
                    continue;
                act.Deleted = true;
                _databaseContext.ResourceUsingActs.Attach(act);
                _databaseContext.Entry(act).State = EntityState.Modified;
            }

            foreach (var act in newActsList)
            {
                if (act.IdResourceUsingAct == default(int))
                {
                    _databaseContext.ResourceUsingActs.Add(act);
                }
                else
                {
                    var findedAct = _databaseContext.ResourceUsingActs.Find(act.IdResourceUsingAct);
                    _databaseContext.Entry(findedAct).CurrentValues.SetValues(act);
                    if (act.File != null)
                    {
                        findedAct.File = _databaseContext.ActFiles.Add(act.File);
                    }
                }
            }
        }

        private void UpdateResourceOperatorActs(IEnumerable<ResourceOperatorAct> oldActs,
            IEnumerable<ResourceOperatorAct> newActs, int idResource)
        {
            var newActsList = newActs == null ? new List<ResourceOperatorAct>() : newActs.ToList();
            newActsList.ForEach(r => r.IdResource = idResource);
            foreach (var act in oldActs)
            {
                if (newActsList.Any(r => r.IdResourceOperatorAct == act.IdResourceOperatorAct))
                    continue;
                act.Deleted = true;
                _databaseContext.ResourceOperatorActs.Attach(act);
                _databaseContext.Entry(act).State = EntityState.Modified;
            }

            foreach (var act in newActsList)
            {
                if (act.IdResourceOperatorAct == default(int))
                {
                    _databaseContext.ResourceOperatorActs.Add(act);
                }
                else
                {
                    var findedAct = _databaseContext.ResourceOperatorActs.Find(act.IdResourceOperatorAct);
                    _databaseContext.Entry(findedAct).CurrentValues.SetValues(act);
                    if (act.File != null)
                    {
                        findedAct.File = _databaseContext.ActFiles.Add(act.File);
                    }
                }
            }
        }

        private void UpdateResourceDeviceAddresses(IEnumerable<ResourceDeviceAddress> oldAddresses,
            IEnumerable<ResourceDeviceAddress> newAddresses, int idResource)
        {
            var newAddressesList = newAddresses == null ? new List<ResourceDeviceAddress>() : newAddresses.ToList();
            newAddressesList.ForEach(r => r.IdResource = idResource);
            foreach (var address in oldAddresses)
            {
                if (newAddressesList.Any(r => r.IdResourceDeviceAddress == address.IdResourceDeviceAddress))
                    continue;
                address.Deleted = true;
                _databaseContext.ResourceDeviceAddresses.Attach(address);
                _databaseContext.Entry(address).State = EntityState.Modified;
            }

            foreach (var address in newAddressesList)
            {
                if (address.IdResourceDeviceAddress == default(int))
                {
                    _databaseContext.ResourceDeviceAddresses.Add(address);
                }
                else
                {
                    var addr = _databaseContext.ResourceDeviceAddresses.Find(address.IdResourceDeviceAddress);
                    _databaseContext.Entry(addr).CurrentValues.SetValues(address);
                }
            }
        }

        private void UpdateResourceInternetAddresses(IEnumerable<ResourceInternetAddress> oldAddresses,
            IEnumerable<ResourceInternetAddress> newAddresses, int idResource)
        {
            var newAddressesList = newAddresses == null ? new List<ResourceInternetAddress>() : newAddresses.ToList();
            newAddressesList.ForEach(r => r.IdResource = idResource);
            foreach (var address in oldAddresses)
            {
                if (newAddressesList.Any(r => r.IdResourceInternetAddress == address.IdResourceInternetAddress)) 
                    continue;
                address.Deleted = true;
                _databaseContext.ResourceInternetAddresses.Attach(address);
                _databaseContext.Entry(address).State = EntityState.Modified;
            }

            foreach (var address in newAddressesList)
            {
                if (address.IdResourceInternetAddress == default(int))
                {
                    _databaseContext.ResourceInternetAddresses.Add(address);
                }
                else
                {
                    var addr = _databaseContext.ResourceInternetAddresses.Find(address.IdResourceInternetAddress);
                    _databaseContext.Entry(addr).CurrentValues.SetValues(address);
                }
            }
        }

        private void UpdateResoureceRights(IEnumerable<ResourceRight> oldRights, IEnumerable<ResourceRight> newRights, int idResource)
        {
            var newRightsList = newRights == null ? new List<ResourceRight>() : newRights.ToList();
            newRightsList.ForEach(r => r.IdResource = idResource);
            foreach (var resourceRight in oldRights)
            {
                if (newRightsList.Any(r => r.IdResourceRight == resourceRight.IdResourceRight)) continue;
                resourceRight.Deleted = true;
                _databaseContext.ResourceRights.Attach(resourceRight);
                _databaseContext.Entry(resourceRight).State = EntityState.Modified;
            }

            foreach (var resourceRight in newRightsList)
            {
                if (resourceRight.IdResourceRight == default(int))
                {
                    _databaseContext.ResourceRights.Add(resourceRight);
                }
                else
                {
                    var resRight = _databaseContext.ResourceRights.Find(resourceRight.IdResourceRight);
                    _databaseContext.Entry(resRight).CurrentValues.SetValues(resourceRight);
                }
            }
        }

        public Resource InsertResource(Resource resource)
        {
            if (resource.OwnerDepartment != null)
            {
                resource.OwnerDepartment.IdDepartment = resource.IdOwnerDepartment;
                UpdateDepartmentExtInfo(resource.OwnerDepartment);
                resource.OwnerDepartment = null;
            }
            if (resource.OperatorDepartment != null)
            {
                resource.OperatorDepartment.IdDepartment = resource.IdOperatorDepartment;
                UpdateDepartmentExtInfo(resource.OperatorDepartment);
                resource.OperatorDepartment = null;
            }
            resource.RequestAllowedDepartments = resource.RequestAllowedDepartments.Select(r => 
                _databaseContext.Departments.Find(r.IdDepartment)).ToList();
            return _databaseContext.Resources.Add(resource);
        }

        private void UpdateDepartmentExtInfo(Department department)
        {
            var dep = _databaseContext.Departments.
                FirstOrDefault(r => !r.Deleted && r.IdDepartment == department.IdDepartment);
            if (dep == null)
            {
                throw new DbUpdateException(string.Format("Не удалось найти департамент {0}", department.Name));
            }
            department.Name = dep.Name;
            department.IdParentDepartment = dep.IdParentDepartment;
            department.IdDepartment = dep.IdDepartment;
            _databaseContext.Entry(dep).CurrentValues.SetValues(department);
        }


        public IQueryable<ResourceGroup> GetResourceGroups()
        {
            return _databaseContext.ResourceGroups.Where(r => !r.Deleted);
        }

        public IQueryable<ResourceInformationType> GetResourceInformationTypes()
        {
            return _databaseContext.ResourceInformationTypes;
        }

        public IQueryable<Department> GetDepartments()
        {
            return _databaseContext.Departments.Where(r => r.IdParentDepartment == null || r.IdParentDepartment == 1).Where(r => !r.Deleted);
        }
    }
}
