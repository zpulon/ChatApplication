using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// Redis 服务类
    /// </summary>
    public static class DistributedCacheExtension
    {

        #region CONST
        #endregion

        /// <summary>
        /// 获取缓存并返回指定类型的对象
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T Get<T>(this IDistributedCache cache, string[] prefixs, string key)
            where T : class, new()
        {
            byte[] data = cache.Get(GetPrefix(prefixs) + key);
            if (data == null || data.Length == 0)
                return default(T);

            T obj = null;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream(data))
            {
                obj = (T)formatter.Deserialize(stream);
            }

            return obj;
        }

        /// <summary>
        /// 获取缓存并返回指定类型的对象
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <param name="token">操作标识</param>
        /// <returns></returns>
        public async static Task<T> GetAsync<T>(this IDistributedCache cache, string[] prefixs, string key, CancellationToken token = default(CancellationToken))
            where T : class, new()
        {
            byte[] data = await cache.GetAsync(GetPrefix(prefixs) + key, token);
            if (data == null || data.Length == 0)
                return default(T);

            T obj = null;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream(data))
            {
                obj = (T)formatter.Deserialize(stream);
            }


            return obj;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存实体方式</param>
        public static void Set<T>(this IDistributedCache cache, string[] prefixs, string key, T value, DistributedCacheEntryOptions options)
            where T : class, new()
        {
            byte[] data = Serialize(value);

            cache.Set(GetPrefix(prefixs) + key, data, options);

        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存处理方式</param>
        /// <param name="token">操作标识</param>
        /// <returns></returns>
        public async static Task SetAsync<T>(this IDistributedCache cache, string[] prefixs, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
            where T : class, new()
        {
            byte[] data = Serialize(value);

            await cache.SetAsync(GetPrefix(prefixs) + key, data, options, token);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <param name="encoding">编码规则</param>
        /// <param name="token">操作标识</param>
        public async static Task<string> GetStringAsync(this IDistributedCache cache, string[] prefixs, string key, Encoding encoding, CancellationToken token = default(CancellationToken))
        {
            byte[] data = await cache.GetAsync(GetPrefix(prefixs) + key, token);
            if (data == null || data.Length == 0)
                return string.Empty;
            return encoding.GetString(data);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="encoding">编码规则</param>
        /// <param name="token">操作标识</param>
        /// <returns></returns>
        public async static Task SetStringAsync(this IDistributedCache cache, string[] prefixs, string key, string value, Encoding encoding, CancellationToken token = default(CancellationToken))
        {
            byte[] data = encoding.GetBytes(value);  // 编码字符串

            await cache.SetAsync(GetPrefix(prefixs) + key, data, token); // 缓存
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">缓存建</param>
        /// <returns></returns>
        public async static Task RemoveAsync(this IDistributedCache cache, string[] prefixs, string key)
        {
            var prefix = GetPrefix(prefixs);
            await cache.RemoveAsync(prefix + key);
        }


        private static byte[] Serialize(object value)
        {
            byte[] data = new byte[0];
            if (value != null)
            {
                IFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, value);
                    ms.Flush();
                    ms.Position = 0;
                    data = ms.ToArray();
                }
            }
            return data;
        }

        static private string GetPrefix(string[] strs)
        {
            var prefix = string.Empty;
            if (strs == null) return "normal";

            foreach (var item in strs)
            {
                if (string.IsNullOrEmpty(item)) continue;
                prefix += $"{item}:";
            }
            return prefix;
        }

        /// <summary>
        /// 设置加锁,在如果存在会提示重复
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">指定缓存的键</param>
        /// <param name="value">指定缓存值</param>
        /// <param name="cancellationToken"></param>
        /// <returns>如果存在重复,会返回 ObjectAlreadyExistsException </returns>
        public async static Task<bool> LockSubmit(this IDistributedCache cache, string[] prefixs, string key, string value, CancellationToken cancellationToken = default(CancellationToken)) {
            if (prefixs.Length == 0) {
                throw new ApiCore.Utils.ArgumentNullErrorException("缓存前缀不能为空");
            }
            if (string.IsNullOrEmpty(value)) {
                value = "default";
            }
            var result = await cache.GetStringAsync(prefixs, key, Encoding.UTF8, cancellationToken);
            if (result == value) {
                throw new ApiCore.Utils.ObjectAlreadyExistsException("请勿频繁提交");
            }
            await cache.SetStringAsync(prefixs, key, value, Encoding.UTF8, cancellationToken);
            return true;
        }
        /// <summary>
        /// 解锁操作 从缓存中移除指定的信息
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <param name="prefixs">缓存前缀</param>
        /// <param name="key">指定缓存的键</param>
        /// <returns></returns>
        public async static Task UnlockSubmit(this IDistributedCache cache, string[] prefixs, string key) {
            if (prefixs.Length == 0) {
                throw new ApiCore.Utils.ArgumentNullErrorException("缓存前缀不能为空");
            }
            await cache.RemoveAsync(prefixs, key);
        }
    }
}
