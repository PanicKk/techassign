using System.Linq.Expressions;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities.Shared;

namespace WMS.Core.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    #endregion
    
    #region Methods
    Task<TEntity> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false);
    Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, bool includeDeleted = false);
    Task<int> GetCountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func,
        bool includeDeleted = false);

    Task<IPagedList<TEntity>> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
                                          int pageIndex = 1, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false);
    
    Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        int pageIndex = 1, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false);

    Task InsertAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion
}