using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Request
{
    public struct MessageRequest
    {
        /// <summary>
        /// zzet 分数
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 教室标识前端传入 
        /// </summary>
        public string ClassRoomId { get; set; }
    }
}
