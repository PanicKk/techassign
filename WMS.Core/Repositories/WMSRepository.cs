using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using WMS.Core.Data;
using WMS.Core.Extensions;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities.Shared;

namespace WMS.Core.Repositories;

public class WMSRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly WMSDbContext _dbContext;
    private readonly DbSet<TEntity> _table;

    #endregion

    #region Properties
    
    public virtual IQueryable<TEntity> Table => _table;

    #endregion

    #region Ctor

    public WMSRepository(WMSDbContext assetContext)
    {
        _dbContext = assetContext;
        _table = _dbContext.Set<TEntity>();
    }

    #endregion

    #region Utilities
    protected virtual IQueryable<TEntity> RemoveDeletedFilter(IQueryable<TEntity> query, in bool includeDeleted)
    {
        if (includeDeleted)
            return query.IgnoreQueryFilters();

        return query;
    }

    #endregion

    #region Methods
    
    public virtual async Task<TEntity> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        bool includeDeleted = false)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
        query = func != null ? func(query) : query;

        return await query.FirstOrDefaultAsync();
    }
    
    public virtual async Task<int> GetCountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
        query = func != null ? func(query) : query;

        return await query.CountAsync(cancellationToken);
    }
    
    public virtual async Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func,
        bool includeDeleted = false)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
        query = func != null ? func(query) : query;

        return await query.ToListAsync();
    }
    
    public virtual async Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func, bool includeDeleted = false)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
        query = func != null ? await func(query) : query;

        return await query.ToListAsync();
    }
    
    public virtual async Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        int pageIndex = 1, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
        query = func != null ? func(query) : query;
    
        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }
    
    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        int pageIndex = 1, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false)
    {
        var query = RemoveDeletedFilter(Table, includeDeleted);
    
        query = func != null ? await func(query) : query;
    
        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }
    
    public virtual async Task InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(entity)} could not be added: {ex.Message}");
        }
    }
    
    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _table.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(entity)} could not be updated: {e.Message}");
        }
    }
    
    public virtual async Task UpdateAsync(IList<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        if (!entities.Any())
            return;

        _table.UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }
    
    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.IsDeleted = true;
        await UpdateAsync(entity);
    }
    
    public virtual async Task DeleteAsync(IList<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        if (entities.Any())
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

            await UpdateAsync(entities);
        }
        else
        {
            _table.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var entities = await Table.Where(predicate).ToListAsync();
        await DeleteAsync(entities);

        return entities.Count;
    }

    #endregion
}