using System.Linq;

namespace RequestsForRights.Web.Infrastructure.Utilities.EntitySorter.Interfaces
{
    public interface IEntitySorter<TEntity>
    {
        IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> collection);
    }
}
