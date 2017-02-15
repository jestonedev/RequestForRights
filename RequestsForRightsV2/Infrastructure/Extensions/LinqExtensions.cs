using System.Linq;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Utilities.EntitySorter;

namespace RequestsForRights.Web.Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> entities, SortDirection sortDirection, string sortField)
        {
            switch (sortDirection)
            {
                case SortDirection.Asc:
                    return EntitySorter<T>.OrderBy(sortField).Sort(entities);
                case SortDirection.Desc:
                    return EntitySorter<T>.OrderByDescending(sortField).Sort(entities);
                default:
                    return entities;
            }
        }
    }
}