
using Microsoft.Extensions.DependencyInjection;
using System;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Manager;
using WebSocketPlugins.Model;
using WebSocketPlugins.Request;
using WebSocketPlugins.Response;
using Xunit;

namespace WebSocketTest.TestModel
{

    [Collection("UnitTestCollection")]
    public  class SocketTest
    {
        private readonly UserManager _userManager;
        private readonly IChatSessionService  _chatSessionService;
        private readonly TestBase<WebSocketDbContext> _testBase;
        public SocketTest(TestBase<WebSocketDbContext> testBase)
        {
            _testBase = testBase;
            _userManager = _testBase.ServiceProvider.GetRequiredService<UserManager>();
            _chatSessionService = _testBase.ServiceProvider.GetRequiredService<IChatSessionService>();
        }
        [Fact(DisplayName = "用户测试")]
        public async void TestGetUserList()
        {
            var result = await _userManager.GetUserInfo(222);
            Assert.True(string.Compare(result.Name,"邹平", StringComparison.OrdinalIgnoreCase)==0, "是否查询成功");
        }
        [Fact(DisplayName = "消息列表测试")]
        public async void TestGetMessageList()
        {
            var result = await _chatSessionService.GetMessageList("111", 1, 10, true);
            var resultTotleCount = (int)await _chatSessionService.GetMessageCount("111");
            Assert.True(resultTotleCount > 0, "是否查询成功");
            Assert.True(result.Count>0, "是否查询成功");
        }
        [Fact(DisplayName = "保存消息测试")]
        public async void TestPostSaveMessage()
        {
            SaveMessageRequest saveMessage = new()
            {
                ChatMessage = "afaf",
                ClassRoomId = "111",
                UserId = 222
            };
            var user  = await _userManager.GetUserInfo(saveMessage.UserId);
            Assert.True(user !=null , "查询成功");
            double socre = await _chatSessionService.SaveMessageAsync(saveMessage.ClassRoomId, new RedisMessage { Id = Guid.NewGuid().ToString(), UserId = saveMessage.UserId, Image = user.Image, Message = saveMessage.ChatMessage, WebSocketId = $"{saveMessage.ClassRoomId}_{saveMessage.UserId}", Name = user.Name });
            Assert.True(socre > 0, "保存成功");
        }
    }
}
