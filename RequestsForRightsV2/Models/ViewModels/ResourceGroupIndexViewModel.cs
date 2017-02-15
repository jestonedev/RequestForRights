using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ResourceGroupIndexViewModel
    {
        public IEnumerable<ResourceGroup> VisibleResourceGroups { get; set; }
        public int ResourceGroupCount { get; set; }
        public FilterOptions.FilterOptions FilterOptions { get; set; }
    }
}