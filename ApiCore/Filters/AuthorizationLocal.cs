using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiCore.Filters
{
    /// <summary>
    /// 基础信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    [DebuggerDisplay("Authorization: PermissionName={PermissionName} Parataxis={Parataxis}")]
    public class AuthorizationLocalAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        private ObjectFactory _factory;
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationLocalAttribute()
        {
            ImplementationType = typeof(AuthorizationImpl);
        }
        /// <summary>
        /// Gets the <see cref="Type"/> of filter to create.
        /// </summary>
        public Type ImplementationType { get; }
        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; }

        public bool IsReusable { get; set; }
        /// <summary>
        /// 权限名称（多个权限用“,”分割后顺序执行）
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 同时满足输入权限，默认为true
        /// </summary>
        public bool Parataxis { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            object[] arguments = null;
            arguments = new object[] { PermissionName ?? "", Parataxis };
            if (_factory == null)
            {
                var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();
                _factory = ActivatorUtilities.CreateFactory(ImplementationType, argumentTypes ?? Type.EmptyTypes);
            }

            var filter = (IFilterMetadata)_factory(serviceProvider, arguments);
            if (filter is IFilterFactory filterFactory)
            {
                // Unwrap filter factories
                filter = filterFactory.CreateInstance(serviceProvider);
            }
            return filter;
        }
        /// <summary>
        /// 
        /// </summary>

        public class AuthorizationImpl : IAsyncActionFilter
        {

            /// <summary>
            /// 权限名称（多个权限用“,”分割后顺序执行）
            /// </summary>
            protected string PermissionName { get; }

            /// <summary>
            /// 同时满足输入权限，默认为true
            /// </summary>
            protected bool Parataxis { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="permissionName"></param>
            /// <param name="parataxis"></param>
            public AuthorizationImpl(
                string permissionName,
                bool parataxis

            )
            {
                PermissionName = permissionName;
                Parataxis = parataxis;

            }

            /// <summary>
            /// 验证功能权限
            /// </summary>
            /// <param name="context"></param>
            /// <param name="next"></param>
            /// <returns></returns>
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var user = GetUser(context);
                if (user == null)
                {
                    return;
                }
                if (context.Controller is BaseController controller)
                {
                    controller.User = user;
                }
                await next();
            }
            private UserInfo GetUser(ActionExecutingContext context)
            {
                if (context == null)
                {
                    return null;
                }
                if (context?.HttpContext?.User == null)
                {
                    context.Result = new ContentResult()
                    {
                        Content = "用户未登录",
                        StatusCode = 403
                    };
                    return null;
                }

                var identity = context.HttpContext.User;
                var token = context.HttpContext.Request.Headers["authorization"].ToString().Replace("Bearer ", "");
                var grant_type = identity.FindFirst("grant_type")?.Value;
                long.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long resultId);
                var user = new UserInfo()
                {
                    Id = resultId,
                    UserName = identity.FindFirst("UserName")?.Value,
                    GraduationYear = identity.FindFirst("GraduationYear")?.Value,
                    SchoolName = identity.FindFirst("SchoolName")?.Value,
                    OnlineSchoolNumber = identity.FindFirst("OnlineSchoolNumber")?.Value,
                    ClassName = identity.FindFirst("ClassName")?.Value,
                    Token = token,
                };
                //if (string.Compare(grant_type, "admin", StringComparison.OrdinalIgnoreCase) == 0)
                //{
                //    RedisHelper.HSet("ADMIN_TOKEN", resultId.ToString(), EncryptHelper.GetInstance().MD5(token));
                //}
                //else
                //{
                //    RedisHelper.Set("userToken_" + resultId, token, TimeSpan.FromDays(7));
                //}
                return user;
            }
        }
    }
}
