using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketPlugins.Stores;

namespace WebSocketPlugins.Manager
{
    /// <summary>
    /// 
    /// </summary>
   public class UserManager
    {
        private readonly IUserStores _userStores;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userStores"></param>
        public UserManager(IUserStores userStores)
        {
            _userStores = userStores;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<(string name, string image)> GetUserInfo(long userId)
        {
            var user = await _userStores.GetAsync(z => z.Id == userId);
            return (user.Name, user.Image);
        }
    }
}
