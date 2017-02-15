using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ReportResourceRightsViewModel
    {
        public ReportResourceRightsOptions Options { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceUserRightModel> Rights { get; set; }
    }
}