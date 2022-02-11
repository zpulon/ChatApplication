using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCore.Dto.Request
{
   public class ScheduleSubmitRequest
    {
        //
        // 摘要:
        //     任务分组（JobGroup + JobName 唯一确定一个任务）（*必传）
        [Required]
        public string JobGroup { get; set; }
        //
        // 摘要:
        //     任务名称
        [Required]
        public string JobName { get; set; }
        //
        // 摘要:
        //     执行表达式（可百度'Cron表达式生成器'）
        public string CronStr { get; set; }
        //
        // 摘要:
        //     开始执行时间
        public DateTime StarRunTime { get; set; }
        //
        // 摘要:
        //     结束执行时间
        public DateTime? EndRunTime { get; set; }
        //
        // 摘要:
        //     回调地址
        public string Callback { get; set; }
        //
        // 摘要:
        //     自定义参数(回调时带回)
        public Dictionary<string, object> Args { get; set; }
    }
}
