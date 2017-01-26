using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Services
{
    public class ReportService: IReportService
    {
        private readonly IRightService _rightService;
        private readonly IUserService _userService;
        private readonly IReportRepository _reportRepository;

        public ReportService(
            IRightService rightService, 
            IUserService userService,
            IReportRepository reportRepository)
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
        }

        public RequestUser FindUser(RequestUser requestUser)
        {
            return _userService.FindUser(requestUser);
        }

        public IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date, int idRequestUser)
        {
            return _rightService.GetUserRightsOnDate(date, idRequestUser).OrderBy(r => r.ResourceName)
                .ThenBy(r => r.ResourceRightName);
        }

        public IEnumerable<Resource> GetResources()
        {
            return _reportRepository.GetResources().OrderBy(r => r.ResourceGroup.Name)
                .ThenBy(r => r.Name).ToList();
        }

        public IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int idResource)
        {
            return _rightService.GetResourceRightsOnDate(date, idResource).OrderBy(r => r.RequestUserSnp)
                .ThenBy(r => r.ResourceRightName);
        }
    }
}