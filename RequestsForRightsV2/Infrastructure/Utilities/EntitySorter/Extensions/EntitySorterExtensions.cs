using System;
using System.Linq.Expressions;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Utilities.EntitySorter.Interfaces;

namespace RequestsForRights.Infrastructure.Utilities.EntitySorter.Extensions
{
    public static class EntitySorterExtensions
    {
        public static IEntitySorter<T> OrderBy<T, TKey>(
            this IEntitySorter<T> sorter,
            Expression<Func<T, TKey>> keySelector)
        {
            return EntitySorter<T>.OrderBy(keySelector);
        }

        public static IEntitySorter<T> OrderByDescending<T, TKey>(
            this IEntitySorter<T> sorter,
            Expression<Func<T, TKey>> keySelector)
        {
            return EntitySorter<T>.OrderByDescending(keySelector);
        }

        public static IEntitySorter<T> ThenBy<T, TKey>(
            this IEntitySorter<T> sorter,
            Expression<Func<T, TKey>> keySelector)
        {
            return new ThenBySorter<T, TKey>(sorter,
                keySelector, SortDirection.Asc);
        }

        public static IEntitySorter<T> ThenByDescending<T, TKey>(
            this IEntitySorter<T> sorter,
            Expression<Func<T, TKey>> keySelector)
        {
            return new ThenBySorter<T, TKey>(sorter,
                keySelector, SortDirection.Desc);
        }

        public static IEntitySorter<T> ThenBy<T>(
            this IEntitySorter<T> sorter, string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName) {Direction = SortDirection.Asc};
            return builder.BuildThenByEntitySorter(sorter);
        }

        public static IEntitySorter<T> ThenByDescending<T>(
            this IEntitySorter<T> sorter, string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName) {Direction = SortDirection.Desc};
            return builder.BuildThenByEntitySorter(sorter);
        }
    }
}