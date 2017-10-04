using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Extensions;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class ReportService: IReportService
    {
        private readonly IRightService _rightService;
        private readonly IUserService _userService;
        private readonly IReportRepository _reportRepository;
        private readonly IReportSecurityService _reportSecurityService;

        public ReportService(
            IRightService rightService, 
            IUserService userService,
            IReportRepository reportRepository,
            IReportSecurityService reportSecurityService)
        {
            if (rightService == null)
            {
                throw new ArgumentNullException("rightService");
            }
            _rightService = rightService;
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
            if (reportRepository == null)
            {
                throw new ArgumentNullException("reportRepository");
            }
            _reportRepository = reportRepository;
            if (reportSecurityService == null)
            {
                throw new ArgumentNullException("reportSecurityService");
            }
            _reportSecurityService = reportSecurityService;
        }

        public RequestUser FindUser(ReportUserRightsOptions options)
        {
            return _userService.FindUser(new RequestUser
            {
                Login = options.Login,
                Snp = options.Snp,
                Department = options.Department,
                Unit = options.Unit
            });
        }

        public IEnumerable<Resource> GetResources()
        {
            return _reportSecurityService.FilterResources(
                _reportRepository.GetResources())
                .OrderBy(r => r.ResourceGroup.Name)
                .ThenBy(r => r.Name).ToList();
        }

        public IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(ReportUserRightsOptions options, int idRequestUser)
        {
            if (options.Date == null)
            {
                return null;
            }
            var rights = _reportSecurityService.FilterResourceRights(
                _rightService.GetUserRightsOnDate(options.Date.Value, idRequestUser));
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.ResourceName)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
        }

        public IEnumerable<ResourceUserRightHistoryModel> GetUserRightsHistoryOnDate(ReportUserRightsHistoryOptions options, int idRequestUser)
        {
            if (options.DateFrom == null || options.DateTo == null)
            {
                return null;
            }
            return _rightService.GetUserRightsHistoryOnDate(options.DateFrom.Value, options.DateTo.Value.AddDays(1).AddSeconds(-1), idRequestUser);
        }

        public IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(ReportResourceRightsOptions options)
        {
            if (options.Date == null)
            {
                return null;
            }
            if (options.IdResource == null)
            {
                return null;
            }
            if (!_reportRepository.GetResources().Select(r => r.IdResource).
                Contains(options.IdResource.Value))
            {
                return null;
            }
            var rights = _reportSecurityService.FilterResourceRights(
                _rightService.GetResourceRightsOnDate(options.Date.Value, options.IdResource));
            
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.RequestUserSnp)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
        }


        public IEnumerable<ResourceUserRightModel> GetDepartmentRightsOnDate(ReportDepartmentRightsOptions options)
        {
            if (options.Date == null)
            {
                return null;
            }
            if (options.Department == null)
            {
                return null;
            }
            var department = options.Department;
            string unit = null;
            if (options.Department.Contains("//"))
            {
                department = options.Department.Split(new[] {"//"}, StringSplitOptions.None)[0];
                unit = options.Department.Split(new[] { "//" }, StringSplitOptions.None)[1];
            }
            if (!_reportRepository.GetDepartments().Select(r => r.Name).
                Contains(department))
            {
                return null;
            }
            var rights = _reportSecurityService.FilterResourceRights(
                _rightService.GetDepartmentRightsOnDate(options.Date.Value, department, unit));
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.RequestUserSnp)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
        }

        public IEnumerable<ResourceUserRightModel> GetDepartmentAndResourceRightsOnDate(ReportDepartmentAndResourceRightsOptions options)
        {
            if (options.Date == null || options.Department == null || options.IdResource == null)
            {
                return null;
            }
            if (!_reportRepository.GetResources().Select(r => r.IdResource).
                Contains(options.IdResource.Value))
            {
                return null;
            }
            var department = options.Department;
            string unit = null;
            if (options.Department.Contains("//"))
            {
                department = options.Department.Split(new[] { "//" }, StringSplitOptions.None)[0];
                unit = options.Department.Split(new[] { "//" }, StringSplitOptions.None)[1];
            }
            if (!_reportRepository.GetDepartments().Select(r => r.Name).
                Contains(department))
            {
                return null;
            }
            var rights = _reportSecurityService.FilterResourceRights(
                _rightService.GetDepartmentAndResourceRightsOnDate(options.Date.Value, department, unit, options.IdResource));
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.RequestUserSnp)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
        }

        public IEnumerable<Department> GetAllowedDepartments()
        {
            return _reportSecurityService.FilterDepartments(
                _reportRepository.GetDepartments())
                .OrderBy(r => r.Name).ToList();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _reportRepository.GetDepartments()
                .OrderBy(r => r.Name).ToList();
        }

        public IEnumerable<ResourceOperatorModel> GetResourceOperatorInfo(int? idDepartment)
        {
            return from resourceRow in _reportRepository.GetResources()
                join departmentRow in _reportRepository.GetDepartments()
                    on resourceRow.IdOperatorDepartment equals departmentRow.IdDepartment
                join operatorPersonRow in _reportRepository.GetOperatorPersons()
                    on resourceRow.IdResource equals operatorPersonRow.IdResource into op
                from opRow in op.DefaultIfEmpty()
                join operatorPersonActRow in _reportRepository.GetOperatorPersonActs()
                    on opRow.IdResourceOperatorPerson equals operatorPersonActRow.IdResourceOperatorPerson into opa
                from opaRow in opa.DefaultIfEmpty()
                where idDepartment == 0 || 
                    resourceRow.IdOperatorDepartment == idDepartment || 
                    resourceRow.OperatorDepartment.IdParentDepartment == idDepartment
                orderby departmentRow.Name, resourceRow.Name, opRow.Surname, opRow.Name, opRow.Patronimic, opaRow.ActDate, opaRow.ActNumber
                select new ResourceOperatorModel
                {
                    Department = departmentRow.Name,
                    IdResource = resourceRow.IdResource,
                    ResourceName = resourceRow.Name,
                    ResourceDescription = resourceRow.Description,
                    Surname = opRow.Surname,
                    Name = opRow.Name,
                    Patronymic = opRow.Patronimic,
                    Post = opRow.Post,
                    ActType = opaRow.ActType,
                    ActName = opaRow.ActName,
                    ActDate = opaRow.ActDate,
                    ActNumber = opaRow.ActNumber,
                    IdFile = opaRow.IdFile
                };
        }
    }
}