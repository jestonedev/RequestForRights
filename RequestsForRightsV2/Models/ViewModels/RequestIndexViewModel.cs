using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class RequestIndexViewModel
    {
        public IEnumerable<Domain.Entities.Request> VisibleRequests { get; set; }
        public int RequestCount { get; set; }
        public FilterOptions.RequestsFilterOptions FilterOptions { get; set; }
        public IEnumerable<RequestStateType> RequestStateTypes { get; set; }
        public IEnumerable<RequestType> RequestTypes { get; set; }
        public IEnumerable<RequestCategoryModel> RequestCatogories { get; set; }
    }
}