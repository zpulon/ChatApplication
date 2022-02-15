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
   public class SaveMessageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClassRoomId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChatMessage { get; set; }
    }
}
