using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;

namespace WMS.Core.Extensions;

public static class IQueryableExtensions
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
    {
        if (source == null)
            return new PagedList<T>(new List<T>(), pageIndex, pageSize);

        pageIndex = Math.Max(pageIndex, 1);
        pageSize = Math.Max(pageSize, 1);

        var count = await source.CountAsync();

        var data = new List<T>();

        if (!getOnlyTotalCount)
            data.AddRange(await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync());

        return new PagedList<T>(data, pageIndex, pageSize, count);
    }
    
    public static IQueryable<T> GetOrderedEntities<T>(this IQueryable<T> query, string defaultOrderBy, string orderBy, SortingOrder sortingOrder) where T : class
    {
        try
        {
            if (query is null)
                return null;

            if (defaultOrderBy is null)
                return query;

            orderBy ??= defaultOrderBy;

            if (orderBy.ToLower() == "createdat")
                orderBy = "CreatedAt";

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, orderBy);
            var keySelector = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            return sortingOrder == SortingOrder.ASC
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);
        }
        catch (Exception )
        {
            return query;
        }
    }
}