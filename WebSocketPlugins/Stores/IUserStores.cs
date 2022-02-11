using ApiCore.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketPlugins.Model;

namespace WebSocketPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IUserStores : IRepository<OS_User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<OS_User> QueryUser();
        /// <summary>
        /// 
        /// </summary>
        IQueryable<OS_User> oS_Users { get; }
    }
}
