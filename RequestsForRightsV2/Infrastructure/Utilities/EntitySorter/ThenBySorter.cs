using System;
using System.Linq;
using System.Linq.Expressions;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Utilities.EntitySorter.Interfaces;

namespace RequestsForRights.Web.Infrastructure.Utilities.EntitySorter
{
    internal sealed class ThenBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly IEntitySorter<T> _baseSorter;
        private readonly Expression<Func<T, TKey>> _keySelector;
        private readonly SortDirection _direction;

        public ThenBySorter(IEntitySorter<T> baseSorter,
            Expression<Func<T, TKey>> selector, SortDirection direction)
        {
            _baseSorter = baseSorter;
            _keySelector = selector;
            _direction = direction;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> col)
        {
            var sorted = _baseSorter.Sort(col);

            return _direction == SortDirection.Asc ? 
                sorted.ThenBy(_keySelector) : 
                sorted.ThenByDescending(_keySelector);
        }
    }
}