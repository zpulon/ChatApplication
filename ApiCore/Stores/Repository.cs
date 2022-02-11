using Microsoft.EntityFrameworkCore;
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
    public class Repository<P, TContext> : IRepository<P> where P : class where TContext : DbContext {
        private readonly TContext _context;
        private readonly DbSet<P> _dbSet;
        /// <summary>
        /// 实例化对象
        /// </summary>
        public Repository(TContext context) {
            _context = context;
            _dbSet = context.Set<P>();
        }
        /// <summary>
        /// 获取查询引用
        /// </summary>
        /// <returns></returns>
        public IQueryable<P> Get(bool isAsNotracking = true) {
            var q = from c in _dbSet
                    select c;
            if (isAsNotracking) {
                q = q.AsNoTracking();
            }
            return q;
        }
        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="expression">执行表达式</param>
        /// <param name="isNoTracking">是否跟踪</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<List<P>> GetListAsync(Expression<Func<P, bool>> expression = null, CancellationToken cancellationToken = default(CancellationToken), bool isNoTracking = true) {
            var q = from c in _dbSet
                    select c;
            if (expression != null) {
                q = q.Where(expression);
            }
            if (isNoTracking) {
                q = q.AsNoTracking();
            }
            return await q.ToListAsync(cancellationToken);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="expression">执行表达式</param>
        /// <param name="isNoTracking">是否跟踪</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<P> GetAsync(Expression<Func<P, bool>> expression = null, CancellationToken cancellationToken = default(CancellationToken), bool isNoTracking = true) {
            var q = from c in _dbSet
                    select c;
            if (expression != null) {
                q = q.Where(expression);
            }
            if (isNoTracking) {
                q = q.AsNoTracking();
            }
            return await q.FirstOrDefaultAsync(cancellationToken);
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity">添加实体</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<P> CreateAsync(P entity, CancellationToken cancellationToken = default(CancellationToken)) {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            } catch (Exception) {
                throw;
            }
        }
        /// <summary>
        /// 创建实体集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<List<P>> CreateRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken)) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            _dbSet.AddRange(collection);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return collection;
            } catch (Exception) {
                throw;
            }
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<P> UpdateAsync(P entity, CancellationToken cancellationToken = default(CancellationToken)) {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            } catch (Exception) {
                throw;
            }
        }
        /// <summary>
        /// 更新对象集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<List<P>> UpdateRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken)) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            _dbSet.UpdateRange(collection);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return collection;
            } catch (Exception) {
                throw;
            }
        }
        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="entity">添加实体</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<P> DeleteAsync(P entity, CancellationToken cancellationToken = default(CancellationToken)) {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            } catch (Exception) {
                throw;
            }
        }
        /// <summary>
        /// 物理删除实体集合
        /// </summary>
        /// <param name="collection">实体集合</param>
        /// <param name="cancellationToken">请求执行凭证</param>
        /// <returns></returns>
        public virtual async Task<List<P>> DeleteRangeAsync(List<P> collection, CancellationToken cancellationToken = default(CancellationToken)) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            _dbSet.RemoveRange(collection);
            try {
                await _context.SaveChangesAsync(cancellationToken);
                return collection;
            } catch (Exception) {
                throw;
            }
        }
    }
}
