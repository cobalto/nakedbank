using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
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
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
