using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// 页码 1开始
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 教室标识前端传入 测试 传999有数据21条
        /// </summary>
        public string ClassRoomId { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// true 第一条为最新数据 false 第一条为旧数据
        /// </summary>
        public bool desc { get; set; } = false;

    }
}
