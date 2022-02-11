using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Basic
{
    /// <summary>
    /// 
    /// </summary>
    public enum ChatEnum
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("进入直播界面拉取信息")]
        AllRef = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("某个同学发送信息刷新")]
        SingleRef = 2,
        /// <summary>
        /// 
        /// </summary>
        [Description("给自己发送ping")]
        ToSelf = 4,
    }
}
