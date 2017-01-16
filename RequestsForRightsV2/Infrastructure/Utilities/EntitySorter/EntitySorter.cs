using System;
using System.Linq;
using System.Linq.Expressions;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Utilities.EntitySorter.Interfaces;

namespace RequestsForRights.Infrastructure.Utilities.EntitySorter
{
    public static class EntitySorter<T>
    {
        public static IEntitySorter<T> AsQueryable()
        {
            return new EmptyEntitySorter();
        }

        public static IEntitySorter<T> OrderBy<TKey>(
            Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector,
                SortDirection.Asc);
        }

        public static IEntitySorter<T> OrderByDescending<TKey>(
            Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector,
                SortDirection.Desc);
        }

        public static IEntitySorter<T> OrderBy(string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName) {Direction = SortDirection.Asc};
            return builder.BuildOrderByEntitySorter();
        }

        public static IEntitySorter<T> OrderByDescending(
            string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName) {Direction = SortDirection.Desc};
            return builder.BuildOrderByEntitySorter();
        }

        private sealed class EmptyEntitySorter : IEntitySorter<T>
        {
            public IOrderedQueryable<T> Sort(
                IQueryable<T> collection)
            {
                string exceptionMessage = "OrderBy should be called.";

                throw new InvalidOperationException(exceptionMessage);
            }
        }
    }
}