using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiService.SocketsManager
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SocketHandler
    {



        /// <summary>
        /// 
        /// </summary>
        public ConnectionManager Connections { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections"></param>
        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="userId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task OnConnected(WebSocket socket, string classRoomId, string userId)
        {
            await Task.Run(async () => { await ConnectionManager.AddsSocketAsync(socket, classRoomId, userId); });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var result = ConnectionManager.GetId(socket);
            if (result != null && result.Item1 != null)
                await ConnectionManager.RemoveSocketAsync(result.Item1, result.Item2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clssRoomId"></param>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string clssRoomId, string userId, string message)
        {
            var webSocket = ConnectionManager.GetSocketById(clssRoomId, userId);
            await SendMessage(webSocket, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public async Task SendMessageToAll(string message, string classRoomId)
        {
            foreach (var conn in ConnectionManager.GetAllConnectionByClassRoomId(classRoomId))
            {
                await SendMessage(conn.Value, message);
            }
        }
        /// <summary>
        /// ping
        /// </summary>
        /// <param name="message"></param>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SendMessageToSelf(string message, string classRoomId, string userId)
        {
            foreach (var conn in ConnectionManager.GetSelfWebsocket(classRoomId, userId))
            {
                await SendMessage(conn.Value, message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public abstract Task Receive(WebSocket socket, string classRoomId, string userId, WebSocketReceiveResult result, byte[] buffer);


    }
}
