using System;
using System.Linq;
using System.Linq.Expressions;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Utilities.EntitySorter.Interfaces;

namespace RequestsForRights.Web.Infrastructure.Utilities.EntitySorter
{
    internal class OrderBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly Expression<Func<T, TKey>> _keySelector;
        private readonly SortDirection _direction;

        public OrderBySorter(Expression<Func<T, TKey>> selector,
            SortDirection direction)
        {
            _keySelector = selector;
            _direction = direction;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> col)
        {
            return _direction == SortDirection.Asc ? 
                col.OrderBy(_keySelector) : 
                col.OrderByDescending(_keySelector);
        }
    }
}