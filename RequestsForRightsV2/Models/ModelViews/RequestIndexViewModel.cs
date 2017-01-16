using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Models.ModelViews
{
    public class RequestIndexViewModel
    {
        public IEnumerable<Request> VisibleRequests { get; set; }
        public int RequestCount { get; set; }
        public FilterOptions.RequestsFilterOptions FilterOptions { get; set; }
        public IEnumerable<RequestStateType> RequestStateTypes { get; set; }
        public IEnumerable<RequestType> RequestTypes { get; set; }
        public IEnumerable<RequestCategoryModel> RequestCatogories { get; set; }
    }
}