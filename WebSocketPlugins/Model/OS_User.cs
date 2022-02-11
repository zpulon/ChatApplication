using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Model
{
    public class OS_User
    {
        /// 编号
        public long Id { get; set; }
        /// 账号
        public string Account { get; set; }
        /// 昵称
        public string Nickname { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNumber { get; set; }
        /// 姓名
        public string Name { get; set; }
        /// 密码
        public string Password { get; set; }
        /// 性别 男/女
        public string Sex { get; set; }
        /// 邮箱
        public string Email { get; set; }
        /// 评级 1-5
        public int Rating { get; set; }
        /// 账号状态 冻结/正常
        public string State { get; set; }
        /// 用户类型 学生/教师
        public string Type { get; set; }
        /// 注册时间
        public DateTime RegisterTime { get; set; }
        /// 登录IP
        public string LoginIp { get; set; }
        /// 登录时间
        public DateTime LoginTime { get; set; }
        /// 头像
        public string Image { get; set; }
        /// 介绍
        public string Introduce { get; set; }
        /// 删除状态
        public bool DeleteState { get; set; }
        /// 修改人
        public long UpdateUser { get; set; }
        /// 修改时间
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 学校ID
        /// </summary>
        public long SchoolId { get; set; }
        /// <summary>
        /// 初中、高中、初完中、高完中
        /// </summary>
        public string SchoolType { get; set; }
        /// <summary>
        /// 年级 ps:高2020级历史方向
        /// </summary>
        public string GraduationYear { get; set; }

        /// <summary>
        /// 所属专业Ids
        /// </summary>
        public string SubjectIds { get; set; }
        /// <summary>
        /// 所属专业名称
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 云校学号
        /// </summary>
        public string OnlineSchoolNumber { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 助教老师ID
        /// </summary>
        public long TeacherId { get; set; }
    }
}
