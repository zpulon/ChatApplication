using System.ComponentModel;

namespace ApiCore.Basic
{
    /// <summary>
    /// 返回参数说明
    /// </summary>
    public class ResponseCodeDefines
    {
        /// <summary>
        /// 成功 0
        /// </summary>
        public static readonly string SuccessCode = ((int)ResponseCodeEnum.SuccessCode).ToString();
        /// <summary>
        /// 对象请求不合法 100
        /// </summary>
        public static readonly string ModelStateInvalid = ((int)ResponseCodeEnum.ModelStateInvalid).ToString();
        /// <summary>
        /// 参数不能为空 101
        /// </summary>
        public static readonly string ArgumentNullError = ((int)ResponseCodeEnum.ArgumentNullError).ToString();
        /// <summary>
        /// 对象已存在 102
        /// </summary>
        public static readonly string ObjectAlreadyExists = ((int)ResponseCodeEnum.ObjectAlreadyExists).ToString();
        /// <summary>
        /// 局部已失效 103
        /// </summary>
        public static readonly string PartialFailure = ((int)ResponseCodeEnum.PartialFailure).ToString();
        /// <summary>
        /// 未找到对应信息 404
        /// </summary>
        public static readonly string NotFound = ((int)ResponseCodeEnum.NotFound).ToString();
        /// <summary>
        /// 授权失效 403
        /// </summary>
        public static readonly string NotAllow = ((int)ResponseCodeEnum.NotAllow).ToString();
        /// <summary>
        /// 服务器内部错误 500
        /// </summary>
        public static readonly string ServiceError = ((int)ResponseCodeEnum.ServiceError).ToString();
    }

    /// <summary>
    /// 响应结果返回值枚举
    /// </summary>
    public enum ResponseCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        SuccessCode = 0,
        /// <summary>
        /// 对象请求不合法
        /// </summary>
        [Description("对象请求不合法")]
        ModelStateInvalid = 100,
        /// <summary>
        /// 参数不能为空
        /// </summary>
        [Description("参数不能为空")]
        ArgumentNullError = 101,
        /// <summary>
        /// 对象已存在
        /// </summary>
        [Description("对象已存在")]
        ObjectAlreadyExists = 102,
        /// <summary>
        /// 局部已失效
        /// </summary>
        [Description("局部已失效")]
        PartialFailure = 103,

        /// <summary>
        /// 未找到对应信息
        /// </summary>
        [Description("未找到对应信息")]
        NotFound = 404,
        /// <summary>
        /// 授权失效
        /// </summary>
        [Description("授权失效")]
        NotAllow = 403,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        [Description("服务器内部错误")]
        ServiceError = 500
    }
}
