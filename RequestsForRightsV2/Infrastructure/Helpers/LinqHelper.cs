using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Interfaces;
using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Infrastructure.Helpers
{
    public static class LinqHelper
    {
        private static T GetProperty<T>(object sender, string propertyName)
        {
            var result = sender;
            foreach (var part in propertyName.Split('.'))
            {
                if (result == null)
                    return default(T);
                var prop = result.GetType().GetProperty(part);
                result = prop != null ? prop.GetValue(result) : default(T);
            }
            return (T)result;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string sortField, 
            SortDirection sortDirection)
        {
            switch (sortDirection)
            {
                case SortDirection.Asc:
                    return entities.OrderBy(e => GetProperty<object>(e, sortField));
                case SortDirection.Desc:
                    return entities.OrderByDescending(e => GetProperty<object>(e, sortField));
                default:
                    return entities;
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> entities, string filter)
            where T: IStringMatchable
        {
            return string.IsNullOrEmpty(filter) ? entities : entities.Where(e => e.Match(filter));
        }
    }
}