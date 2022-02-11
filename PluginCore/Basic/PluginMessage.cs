using System.Collections.Generic;

namespace PluginCore.Basic
{
    public class PluginMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public PluginMessage()
        {
            Code = PluginCodeDefines.SuccessCode;
        }

        public bool IsSuccess()
        {
            if (Code == PluginCodeDefines.SuccessCode)
            {
                return true;
            }
            return false;
        }
    }

    public class PluginMessage<TEx> : PluginMessage
    {
        public TEx Extension { get; set; }
    }

    public class PagingPluginMessage<Tentity> : PluginMessage<List<Tentity>>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }
    }




}
