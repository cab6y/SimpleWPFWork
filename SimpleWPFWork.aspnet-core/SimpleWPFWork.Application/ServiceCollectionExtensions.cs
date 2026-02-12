using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace SimpleWPFWork.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);

            // Scrutor ile otomatik service kayıt
            services.Scan(scan => scan
                .FromAssemblyOf<ApplicationAssemblyMarker>()
                .AddClasses(classes => classes.Where(type =>
                    type.Name.EndsWith("AppService") &&
                    !type.IsAbstract))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}