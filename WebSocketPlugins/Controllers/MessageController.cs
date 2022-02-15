using ApiCore.Basic;
using ApiCore.Filters;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Manager;
using WebSocketPlugins.Request;
using WebSocketPlugins.Response;
using WebSocketPlugins.SocketsManager;
using static WebSocketPlugins.Manager.UserManager;

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
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager userManager;
        /// <summary>
        /// 
        /// </summary>
        public ConnectionManager _connections { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ichatSessionService"></param>
        /// <param name="connections"></param>
        /// <param name="userManager"></param>
        public MessageController(IChatSessionService ichatSessionService, ConnectionManager connections, UserManager userManager, IMemoryCache memoryCache)
        {
            _ichatSessionService = ichatSessionService;
            _connections = connections;
            this.userManager = userManager;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 进入直播间到redis 获取信息
        /// </summary>
        /// <param name="request">用户信息</param>
        /// <returns></returns>
        [HttpGet("list")]
        [AuthorizationLocal]
        public async Task<PagingResponseMessage<RedisMessage>> GetMessageList([FromQuery] ChatRequest request)
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
        [HttpGet("single")]
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
        /// 进入直播间到redis 获取单条信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [AuthorizationLocal]
        public async Task<ResponseMessage<double>> SaveMessage([FromBody] SaveMessageRequest request)
        {
            ResponseMessage<double> response = new();
            try
            {
                _memoryCache.TryGetValue(request.UserId,out UserCache user);
                if (user == null)
                {
                    user = await userManager.GetUserInfo(request.UserId);
                    _memoryCache.Set(request.UserId, user);
                }
                double socre = await _ichatSessionService.SaveMessageAsync(request.ClassRoomId, new RedisMessage { Id = Guid.NewGuid().ToString(), UserId = request.UserId, Image = user.Image, Message = request.ChatMessage, WebSocketId = $"{request.ClassRoomId}_{request.UserId}", Name = user.Name });
                response.Extension = socre;
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
        public async Task<ResponseMessage<long>> GetClassRoomNumber([FromQuery] string classroomid)
        {
            ResponseMessage<long> response = new();
            try
            {
                await Task.Run(async () => { response.Extension = await _connections.GetClassRoomByIdAsync(classroomid);});
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
    }
}
