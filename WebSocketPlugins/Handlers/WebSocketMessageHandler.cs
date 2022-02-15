using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketPlugins.Basic;
using WebSocketPlugins.SocketsManager;

namespace WebSocketPlugins.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSocketMessageHandler : SocketHandler
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="_chatSessionService"></param>
        /// <param name="userService"></param>
        public WebSocketMessageHandler(ConnectionManager connection) : base(connection)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public override async Task OnConnected(WebSocket socket, string classRoomId, string userId)
        {

            await base.OnConnected(socket, classRoomId, userId);
            //int messageType = Convert.ToInt32(ChatEnum.AllRef);
            await SendMessageToAll($"{{\"type\":{1}}}", classRoomId);
        }
        /// <summary>
        /// 介绍到信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override async Task Receive(WebSocket socket, string classRoomId, string userId, WebSocketReceiveResult result, byte[] buffer)
        {
            var redisMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            if (redisMessage == "ping")
            {
                //心跳不保存数据
                int messageType = Convert.ToInt32(ChatEnum.ToSelf);
                string message = $"{{\"type\":{messageType}}}";
                byte[] bufferMessgae = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(bufferMessgae, 0, bufferMessgae.Length), WebSocketMessageType.Text, true, CancellationToken.None);

            }
            else
            {
                int messageType = Convert.ToInt32(ChatEnum.SingleRef);
                double.TryParse(redisMessage, out double socre);
                await SendMessageToAll($"{{\"type\":{messageType},\"score\":{socre}}} ", classRoomId);
            }

        }
    }
}
