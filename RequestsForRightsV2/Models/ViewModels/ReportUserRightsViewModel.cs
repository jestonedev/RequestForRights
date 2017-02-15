using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ReportUserRightsViewModel
    {
        public ReportUserRightsOptions Options { get; set; }
        public IEnumerable<ResourceUserRightModel> Rights { get; set; }
        public RequestUser RequestUser { get; set; }
    }
}