using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NakedBank.Application;

namespace NakedBank.WebApi.Extensions
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new ApplicationProfile());
                    cfg.AddProfile(new InfrastructureProfile());
                },
                LoggerFactory.Create(builder => builder.AddConsole())
            );

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
