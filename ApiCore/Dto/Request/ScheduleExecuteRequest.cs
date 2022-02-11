using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCore.Dto.Request
{
   public class ScheduleExecuteRequest
    {
        //
        // 摘要:
        //     任务名称
        public string JobName { get; set; }
        //
        // 摘要:
        //     任务分组
        public string JobGroup { get; set; }
        //
        // 摘要:
        //     参数，添加任务时用户自定参数
        public Dictionary<string, object> Args { get; set; }
        //
        // 摘要:
        //     回调地址
        public string Callback { get; set; }
    }
}
