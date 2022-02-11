using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiCore.Filters
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public new UserInfo User { get; internal set; }
    }

    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户姓名 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 学校名称 
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 年纪方向 
        /// </summary>
        public string GraduationYear { get; set; }
        /// <summary>
        /// 云学号 
        /// </summary>
        public string OnlineSchoolNumber { get; set; }
        /// <summary>
        /// 班级 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 用户登录凭证
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        //[SwaggerIgnore]
        public string Token { get; set; }
    }
}
