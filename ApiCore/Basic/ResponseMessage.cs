using System;
using System.Collections.Generic;

namespace ApiCore.Basic
{
    /// <summary>
    /// 响应体
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// 结果码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 构造器
        /// </summary>
        public ResponseMessage()
        {
            Code = ResponseCodeDefines.SuccessCode;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() => Code == ResponseCodeDefines.SuccessCode;
    }

    /// <summary>
    /// 携带数据的响应体
    /// </summary>
    /// <typeparam name="TEx"></typeparam>
    public class ResponseMessage<TEx> : ResponseMessage
    {
        /// <summary>
        /// 携带数据
        /// </summary>
        public TEx Extension { get; set; }
    }

    /// <summary>
    /// 分页响应体
    /// </summary>
    /// <typeparam name="Tentity"></typeparam>
    public class PagingResponseMessage<Tentity> : ResponseMessage<List<Tentity>>
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 对象总数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageCount { get => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize); }
    }
}
