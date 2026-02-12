using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWPFWork.EntityFrameworkCore.Interceptors;

namespace SimpleWPFWork.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            // Interceptor
            services.AddSingleton<SoftDeleteInterceptor>();

            // DbContext
            services.AddDbContext<SimpleWPFWorkDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<SoftDeleteInterceptor>();
                options.UseSqlServer(connectionString)
                       .AddInterceptors(interceptor);
            });

            return services;
        }
    }
}