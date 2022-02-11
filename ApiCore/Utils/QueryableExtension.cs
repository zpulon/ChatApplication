using ApiCore.Basic;
using ApiCore.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApiCore
{
    /// <summary>
    /// IQueryable Extension<br/>
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propertyName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool desc = false)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Property(param, propertyName);
            dynamic keySelector = Expression.Lambda(body, param);
            return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
        }
        /// <summary>
        /// 再排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propertyName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propertyName, bool desc = false)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Property(param, propertyName);
            dynamic keySelector = Expression.Lambda(body, param);
            return desc ? Queryable.ThenByDescending(queryable, keySelector) : Queryable.ThenBy(queryable, keySelector);
        }
        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="values">参考值</param>
        /// <param name="propName">待过滤的属性字段名</param>
        /// <param name="isExclude"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IList<object> values, string propName, bool isExclude = false)
        {
            var query = queryable;

            var param = Expression.Parameter(typeof(T));
            var method = values.GetType().GetMethod(nameof(values.Contains));
            var include = Expression.Call(Expression.Constant(values), method, Expression.Property(param, propName));
            query = isExclude ? query.Where(Expression.Lambda<Func<T, bool>>(Expression.Not(include), param)) : query.Where(Expression.Lambda<Func<T, bool>>(include, param));

            return query;
        }
        /// <summary>
        /// 过滤排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<T> FilterAndSort<T>(this IQueryable<T> source, PageRequest request)
        {
            var query = source;
            // 过滤
            if (request.Filters != null && request.Filters.Count > 0)
            {
                foreach (var filter in request.Filters)
                {
                    var propName = filter.Field;
                    var isExclude = filter.Exclude;
                    var values = filter.Values;
                    query = query.Filter(values, propName, isExclude);
                }
            }
            // 排序
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                var orderQuery = query.OrderBy(request.Sorts[0].Field, request.Sorts[0].Desc);
                for (var i = 1; i < request.Sorts.Count; i++)
                {
                    orderQuery = orderQuery.ThenBy(request.Sorts[i].Field, request.Sorts[i].Desc);
                }
                query = orderQuery;
            }
            return query;
        }

        /// <summary>
        /// 分页操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static PagingResponseMessage<T> Page<T>(this IQueryable<T> source, PageRequest request)
        {
            var query = source;
            var index = request.Index;
            var size = request.Size;
            var total = query.Count();
            query = query.Skip(index * size).Take(size);
            return new PagingResponseMessage<T>
            {
                Extension = query.ToList(),
                PageIndex = index,
                PageSize = size,
                TotalCount = total
            };
        }
    }
}
