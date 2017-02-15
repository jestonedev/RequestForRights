using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ResourceIndexViewModel
    {
        public IEnumerable<Resource> VisibleResources { get; set; }
        public int ResourceCount { get; set; }
        public FilterOptions.FilterOptions FilterOptions { get; set; }
    }
}