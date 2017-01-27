using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ReportOptions;

namespace RequestsForRights.Models.ViewModels
{
    public class ReportResourceRightsViewModel
    {
        public ReportResourceRightsOptions Options { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<ResourceUserRightModel> Rights { get; set; }
    }
}