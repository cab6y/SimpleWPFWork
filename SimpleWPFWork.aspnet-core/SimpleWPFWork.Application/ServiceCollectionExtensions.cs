using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SimpleWPFWork.Application.Common.Behaviors;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.Domain.Entities.Todos;
using SimpleWPFWork.EntityFramworkCore.Repositories.Categories;
using SimpleWPFWork.EntityFramworkCore.Repositories.Todos;
using System.Reflection;

namespace SimpleWPFWork.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationAssemblyMarker).Assembly;

            // AutoMapper
            services.AddAutoMapper(assembly);

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                // Validation pipeline behavior
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);
            services.AddScoped<ITodoRepository, EFCoreTodoRepository>();
            services.AddScoped<ICategoryRepository, EFCoreCategoryRepository>();



            // Scrutor ile AppService'leri kaydet
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