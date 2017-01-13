using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Models.ModelViews
{
    public class ResourceGroupIndexModelView
    {
        public IEnumerable<ResourceGroup> VisibleResourceGroups { get; set; }
        public int ResourceGroupCount { get; set; }
        public FilterOptions.FilterOptions FilterOptions { get; set; }
    }
}