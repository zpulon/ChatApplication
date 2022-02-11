using ApiCore.Stores;
using Microsoft.EntityFrameworkCore;
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
    public class UserStores : Repository<OS_User, WebSocketDbContext>, IUserStores
    {
        /// <summary>
        /// 
        /// </summary>
        protected WebSocketDbContext context { get; }
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<OS_User> oS_Users { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public UserStores(WebSocketDbContext applicationDbContext) : base(applicationDbContext)
        {
            context = applicationDbContext;
            oS_Users = applicationDbContext.OS_Users;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<OS_User> QueryUser()
        {
            return oS_Users.AsNoTracking();
        }
    }
}
