using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Basic
{
    /// <summary>
    /// 常量
    /// </summary>
    public class CommonConstant
    {
        /// <summary>
        ///用户数据 Key前缀标识
        /// </summary>
        public static readonly string CHAT_USER_PREFIX = "CHAT_USER_";
        /// <summary>
        ///用户数据 WEBSOCKET前缀标识
        /// </summary>
        public static readonly string WEBSOCKET_PREFIX = "WEBSOCKET:CHAT_";
        /// <summary>
        ///用户数据 WEBSOCKET前缀标识
        /// </summary>
        public static readonly string CLASSROOM_NUMBER = " CLASSROOM:NUMBER_";
        /// <summary>
        ///发消息 Key前缀标识
        /// </summary>
        public static readonly string CHAT_COMMON_PREFIX = "CHAT:ClassRoom_";
        /// <summary>
        ///推送至指定用户消息 推送方Session Key前缀标识
        /// </summary>
        public static readonly string CHAT_FROM_PREFIX = "CHAT_FROM_";
        /// <summary>
        ///推送至指定用户消息 接收方Session Key前缀标识
        /// </summary>
        public static readonly string CHAT_TO_PREFIX = "CHAT_TO_";
        /// <summary>
        ///RedisTemplate 根据Key模糊匹配查询前缀
        /// </summary>
        public static readonly string REDIS_MATCH_PREFIX = "*";
    }
}
