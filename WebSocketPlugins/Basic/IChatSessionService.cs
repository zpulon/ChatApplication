using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketPlugins.Response;

namespace WebSocketPlugins.Basic
{
    public interface IChatSessionService
    {
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classRoomId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task<T> GetMessageAsync<T>(string classRoomId, double score);
        /// <summary>
        /// 保存发送的消息
        /// </summary>
        /// <param name="classRoomId">教室id</param>
        /// <param name="redisMessage">消息体</param>
        /// <returns></returns>
        Task<double> SaveMessageAsync(string classRoomId, RedisMessage redisMessage);
        /// <summary>
        /// 消息分页
        /// </summary>
        /// <param name="classRoomId">教室id</param>
        /// <param name="pageIndex">起始数</param>
        /// <param name="pageSize">-1表示到结束，0为1条 所以需要减1</param>
        /// <param name="desc">是否按降序排列</param>
        /// <returns></returns>
        Task<List<RedisMessage>> GetMessageList(string classRoomId, int pageIndex, int pageSize, bool desc = false);
        /// <summary>
        /// 获取消息条数
        /// </summary>
        /// <param name="classRoomId">教室id</param>
        /// <returns></returns>
        Task<long> GetMessageCount(string classRoomId);
        /// <summary>
        /// 通过教室id userid获取信息
        /// </summary>
        /// <param name="key">classRoomId</param>
        /// <param name="dataKey">userId</param>
        /// <returns></returns>
        Task<T> GetSocketByIdAsync<T>(string key, string dataKey);
        /// <summary>
        /// 通过key获取hash中的字典值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">教室id</param>
        /// <returns></returns>
        ConcurrentDictionary<string, T> HashGetAll<T>(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">websocket</param>
        /// <param name="value">教室标识_user标识_名字</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task SaveClassRoomSndUserInfo<T>(T key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">websocket</typeparam>
        /// <param name="key"></param>
        /// <returns>第一个string classroomid，第二个userid_第三个名字</returns>
        Task<Tuple<string, string, string>> GetClassRoomSndUserInfo<T>(T key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">websocket</typeparam>
        /// <param name="key">classRoomId</param>
        /// <param name="dataKey">userId</param>
        /// <param name="t">websocket</param>
        /// <returns></returns>
        Task<bool> WebSocketHashSet<T>(string key, string dataKey, T t);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">classRoomId</param>
        /// <param name="dataKey">userId</param>
        /// <returns></returns>
        Task<bool> HashDeleteAsync(string key, string dataKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">websocket</param>
        /// <returns></returns>
        Task<bool> StringDeleteAsync<T>(T key);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="classRoomId">websocket</param>
        /// <returns></returns>
        Task Incr(string classRoomId);
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="classRoomId">websocket</param>
        /// <returns></returns>
        Task Decr(string classRoomId);
        /// <summary>
        /// 在线人数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> HashStringLength(string key);
    }
}
