using ApiCore.Basic;
using ApiCore.Filters;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Request;
using WebSocketPlugins.Response;
using WebSocketPlugins.SocketsManager;

namespace WebSocketPlugins.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/message")]
    public class MessageController : BaseController
    {
        private readonly IChatSessionService _ichatSessionService;
        /// <summary>
        /// 
        /// </summary>
        public ConnectionManager _connections { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ichatSessionService"></param>
        /// <param name="connections"></param>
        public MessageController(IChatSessionService ichatSessionService, ConnectionManager connections)
        {
            _ichatSessionService = ichatSessionService;
            _connections = connections;
        }
        /// <summary>
        /// 进入直播间到redis 获取信息
        /// </summary>
        /// <param name="request">用户信息</param>
        /// <returns></returns>
        [HttpPost("list")]
        [AuthorizationLocal]
        public async Task<PagingResponseMessage<RedisMessage>> GetMessageList([FromBody] ChatRequest request)
        {
            PagingResponseMessage<RedisMessage> response = new();
            try
            {
                response.TotalCount = (int)await _ichatSessionService.GetMessageCount(request.ClassRoomId);
                response.Extension = await _ichatSessionService.GetMessageList(request.ClassRoomId, request.PageIndex, request.PageSize, request.desc);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        /// <summary>
        /// 进入直播间到redis 获取单条信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("single/message")]
        [AuthorizationLocal]
        public async Task<ResponseMessage<RedisMessage>> GetMessage([FromQuery] MessageRequest request)
        {
            ResponseMessage<RedisMessage> response = new();
            try
            {
                response.Extension = await _ichatSessionService.GetMessageAsync<RedisMessage>(request.ClassRoomId, request.Score);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

         /// <summary>
         /// 获取在线人数   
         /// </summary>
         /// <param name="classroomid">教室标识</param>
         /// <returns></returns>
        [HttpGet("number")]
        [AuthorizationLocal]
        public async Task<long> GetClassRoomNumber([FromQuery] string classroomid)
        {
            long number = 0;
            try
            {
                await Task.Run(async () => { number = await _connections.GetClassRoomByIdAsync(classroomid); });
            }
            catch (Exception)
            {

                throw;
            }
            return number;
        }
    }
}
