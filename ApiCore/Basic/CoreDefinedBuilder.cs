using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiCore.Basic
{
    public class CoreDefinedBuilder
    {
        public CoreDefinedBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
        IServiceCollection Services { get; }
    }
}
