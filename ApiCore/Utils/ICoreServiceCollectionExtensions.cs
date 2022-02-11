using ApiCore.Basic;
using ApiCore.JsonFilter;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiCore.Utils
{
    public static class ICoreServiceCollectionExtensions
    {

        public static IServiceProvider ServiceProvider { get; set; }

        public static CoreDefinedBuilder AddUserDefined(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddSingleton<IJsonHelper, JsonHelper>();
            services.AddSingleton<HttpClientActuator>();
            ServiceProvider = services.BuildServiceProvider();
            return new CoreDefinedBuilder(services);
        }
    }

    public class CoreOptions
    {
        public string DataBaseConnection { get; set; }
    }
}
