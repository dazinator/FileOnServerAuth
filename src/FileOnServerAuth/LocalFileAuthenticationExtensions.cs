using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;

namespace Dazinator.FileOnServerAuth
{
    public static class LocalFileAuthenticationExtensions
    {
        public static IServiceCollection AddLocalFileAuthentication(this IServiceCollection services,
            Action<AuthCodeOptions> configureOptions)
        {
            services.Configure<AuthCodeOptions>(configureOptions);
            services.ConfigureOptions<AuthCodeOptionsPostConfigureOptions>();
            services.AddSingleton<SystemAuthCodeProvider>((sp) => {
                var options = sp.GetRequiredService<IOptions<AuthCodeOptions>>();
                var fp = new PhysicalFileProvider(options.Value.PhysicalRootPath);
                return Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance<SystemAuthCodeProvider>(sp, fp);
            });
            return services;
        }

        public static IServiceCollection AddLocalFileAuthentication(
            this IServiceCollection services,           
           IConfiguration config)
        {
            services.Configure<AuthCodeOptions>(config);
            services.ConfigureOptions<AuthCodeOptionsPostConfigureOptions>();
            services.AddSingleton<SystemAuthCodeProvider>((sp)=> {
                var options = sp.GetRequiredService<IOptions<AuthCodeOptions>>();
                var fp = new PhysicalFileProvider(options.Value.PhysicalRootPath);
                return Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance<SystemAuthCodeProvider>(sp, fp);
            });
            return services;
        }
    }
}
