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
            var rights = _rightService.GetUserRightsOnDate(options.Date.Value, idRequestUser);
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.ResourceName)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
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
            var rights = _rightService.GetResourceRightsOnDate(options.Date.Value, options.IdResource);
            if (options.ReportDisplayStyle == ReportDisplayStyle.Cards)
            {
                return rights
                    .OrderBy(r => r.RequestUserSnp)
                    .ThenBy(r => r.ResourceRightName);
            }
            return rights.AsQueryable().OrderBy(options.SortDirection, options.SortField);
        }
    }
}