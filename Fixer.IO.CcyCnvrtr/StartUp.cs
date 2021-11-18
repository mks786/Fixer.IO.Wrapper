using System;
using Fixer.IO.CcyCnvrtr.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Fixer.IO.CcyCnvrtr
{
    public static class StartUp
{
        public static void UseFixer(this IServiceCollection services, FixerConfig config)
        {
            var isValidConfig = config?.IsValid() ?? false;
            if (!isValidConfig)
            {
                throw new Exception($"{nameof(FixerConfig)} is invalid");
            }

            RegisterDependencies(services, config);
        }

        private static void RegisterDependencies(IServiceCollection services, FixerConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<IFixerManager, FixerManager>();
        }
    }
}
