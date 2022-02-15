using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketPlugins.SocketsManager
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionManager
    {


        #region 

        /// <summary>
        /// 通过classRoomId_userId 组合为key 
        /// </summary>
        private static readonly ConcurrentDictionary<Tuple<string, string>, WebSocket> _connections = new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static WebSocket GetSocketById(string classRoomId, string userId)
        {

            Tuple<string, string> tuple = new(classRoomId, userId);
            var result = _connections.FirstOrDefault(x => x.Key == tuple).Value;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classRoomId">教室标识</param>
        /// <returns></returns>
        public static ConcurrentDictionary<Tuple<string, string>, WebSocket> GetAllConnectionByClassRoomId(string classRoomId)
        {
            var dictionary = _connections.Where(x => x.Key.Item1 == classRoomId).ToDictionary(z => z.Key, z => z.Value);
            ConcurrentDictionary<Tuple<string, string>, WebSocket> concurrentDictionary =
            new(dictionary);
            return concurrentDictionary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classRoomId">教室标识</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<Tuple<string, string>, WebSocket> GetSelfWebsocket(string classRoomId, string userId)
        {
            var dictionary = _connections.Where(x => x.Key.Item1 == classRoomId && x.Key.Item2 == userId).ToDictionary(z => z.Key, z => z.Value);
            ConcurrentDictionary<Tuple<string, string>, WebSocket> concurrentDictionary =
            new(dictionary);
            return concurrentDictionary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>元组 第一个 教室标识 第二个用户标识</returns>
        public static Tuple<string, string> GetId(WebSocket socket)
        {
            var key = _connections.FirstOrDefault(x => x.Value == socket).Key;
            if (key != null)
            {
                Tuple<string, string> tuple = new(key.Item1, key.Item2);
                return tuple;
            }
            else
            {
                return new Tuple<string, string>(null, null);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task RemoveSocketAsync(string classRoomId, string userId)
        {
            Tuple<string, string> tuple = new(classRoomId, userId);
            _connections.TryRemove(tuple, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>

        public static async Task AddsSocketAsync(WebSocket socket, string classRoomId, string userId)
        {
            await Task.Run(() => {
                Tuple<string, string> tuple = new(classRoomId, userId);
                if (_connections.ContainsKey(tuple))
                {
                    _connections[tuple] = socket;
                }
                else
                {
                    _connections.TryAdd(tuple, socket);
                }
            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public static async Task<long> GetClassRoomByIdAsync(string classRoomId)
        {
            long number = 0;
            await Task.Run(() => {
                number = _connections.Where(z => z.Key.Item1 == classRoomId).Count();
            });
            return number;
        }
        #endregion

    }
}
