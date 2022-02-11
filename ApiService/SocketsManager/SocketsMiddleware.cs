using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ApiService.SocketsManager
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketsMiddleware
    {

        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="handler"></param>
        public SocketsMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            Handler = handler;

        }
        private SocketHandler Handler { get; set; }
        /// <summary>
        ///  wesocket 入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;
            var socket = await context.WebSockets.AcceptWebSocketAsync();

            string userId = context.Request.Query["userId"].ToString();
            string classRoomId = context.Request.Query["classRoomId"].ToString();
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(classRoomId))
                return;
            await Handler.OnConnected(socket, classRoomId, userId);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(socket, classRoomId, userId, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close || socket.State == WebSocketState.CloseReceived)
                {
                    await Handler.OnDisconnected(socket);
                }
            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="messageHandle"></param>
        /// <returns></returns>
        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> messageHandle)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandle(result, buffer);

            }
        }



    }
}
