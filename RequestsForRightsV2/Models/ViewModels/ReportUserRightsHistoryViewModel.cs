using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ReportUserRightsHistoryViewModel
    {
        public ReportUserRightsHistoryOptions Options { get; set; }
        public IEnumerable<ResourceUserRightHistoryModel> Rights { get; set; }
        public RequestUser RequestUser { get; set; }
    }
}