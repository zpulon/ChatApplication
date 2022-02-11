using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCore.Dto.Request
{
    /// 停止一个任务
    /// </summary>
    public class StopScheduleRequest
    {
        /// <summary>
        /// 任务分组
        /// </summary>
        [Required(ErrorMessage = "任务分组不能为空")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "任务名称不能为空")]
        public string JobName { get; set; }
    }
}
