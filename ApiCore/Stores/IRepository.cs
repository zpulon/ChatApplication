using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiCore.Stores {
    /// <summary>
    /// 仓储公共处理方法
    /// </summary>
    public interface IRepository<P>  where P : class {

        /// <summary>
        /// 获取查询引用
        /// </summary>
        /// <param name="isAsNotracking">是否跟踪</param>
        /// <returns></returns>
        IQueryable<P> Get(bool isAsNotracking = true);
        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="expression">执行表达式</param>
        /// <param name="isNoTracking">是否跟踪</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<List<P>> GetListAsync(Expression<Func<P, bool>> expression = null, CancellationToken cancellationToken = default(CancellationToken), bool isNoTracking = true);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="expression">执行表达式</param>
        /// <param name="isNoTracking">是否跟踪</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<P> GetAsync(Expression<Func<P, bool>> expression = null, CancellationToken cancellationToken = default(CancellationToken), bool isNoTracking = true);
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity">添加实体</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<P> CreateAsync(P entity, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 创建实体集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<List<P>> CreateRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<P> UpdateAsync(P entity, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 更新对象集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<List<P>> UpdateRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="entity">添加实体</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<P> DeleteAsync(P entity, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 物理删除实体集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        Task<List<P>> DeleteRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken));
    }
}
