using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ReportOptions;

namespace RequestsForRights.Models.ViewModels
{
    public class ReportUserRightsViewModel
    {
        public ReportUserRightsOptions Options { get; set; }
        public IEnumerable<ResourceUserRightModel> Rights { get; set; }
        public RequestUser RequestUser { get; set; }
    }
}