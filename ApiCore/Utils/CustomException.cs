using ApiCore.Basic;
using System;

namespace ApiCore.Utils
{
    /// <summary>
    /// 自定义异常类
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 错误信息描述
        /// </summary>
        public string ErrorMsg { get; set; }


        /// <summary>
        /// 指定code异常
        /// </summary>
        /// <param name="code"></param>
        public CustomException(int code) : this(code, string.Empty) { }

        /// <summary>
        /// 指定code异常
        /// </summary>
        /// <param name="code"></param>
        public CustomException(ResponseCodeEnum code) : this(code, code.GetDescription()) { }

        /// <summary>
        /// 自定义异常内容
        /// </summary> 
        /// <param name="message"></param>
        public CustomException(string message) : this(ResponseCodeEnum.ServiceError, message) { }

        /// <summary>
        /// 自定义异常内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public CustomException(ResponseCodeEnum code, string message) : this((int)code, message)
        {
        }

        /// <summary>
        /// 自定义异常内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public CustomException(int code, string message) : base(message)
        {
            Code = code;
            ErrorMsg = message;
        }
        /// <summary>
        /// 自定义异常内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CustomException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
            ErrorMsg = message;
        }
    }

    /// <summary>
    /// 对象验证失败异常
    /// </summary>
    public class ModelStateInvalidException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public ModelStateInvalidException() : this(ResponseCodeEnum.ModelStateInvalid.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public ModelStateInvalidException(string message) : base(ResponseCodeEnum.ModelStateInvalid, message) { }
    }

    /// <summary>
    /// 参数为空异常
    /// </summary>
    public class ArgumentNullErrorException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public ArgumentNullErrorException() : this(ResponseCodeEnum.ArgumentNullError.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public ArgumentNullErrorException(string message) : base(ResponseCodeEnum.ArgumentNullError, message) { }
    }

    /// <summary>
    /// 对象已存在
    /// </summary>
    public class ObjectAlreadyExistsException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public ObjectAlreadyExistsException() : this(ResponseCodeEnum.ObjectAlreadyExists.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public ObjectAlreadyExistsException(string message) : base(ResponseCodeEnum.ObjectAlreadyExists, message) { }
    }


    /// <summary>
    /// 局部已失效
    /// </summary>
    public class PartialFailureException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public PartialFailureException() : this(ResponseCodeEnum.PartialFailure.GetDescription())
        {
        }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public PartialFailureException(string message) : base(ResponseCodeEnum.PartialFailure, message) { }
    }

    /// <summary>
    /// 未找到对应信息
    /// </summary>
    public class NotFoundException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public NotFoundException() : this(ResponseCodeEnum.NotFound.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string message) : base(ResponseCodeEnum.NotFound, message) { }
    }

    /// <summary>
    /// 授权失效
    /// </summary>
    public class NotAllowException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public NotAllowException() : this(ResponseCodeEnum.NotAllow.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public NotAllowException(string message) : base(ResponseCodeEnum.NotAllow, message) { }
    }

    /// <summary>
    /// 服务器内部错误
    /// </summary>
    public class ServiceErrorException : CustomException
    {

        /// <summary>
        /// 
        /// </summary>
        public ServiceErrorException() : this(ResponseCodeEnum.ServiceError.GetDescription()) { }

        /// <summary>
        /// 指定异常信息描述
        /// </summary>
        /// <param name="message"></param>
        public ServiceErrorException(string message) : base(ResponseCodeEnum.ServiceError, message) { }
    }
}
