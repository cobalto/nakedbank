using Coravel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NakedBank.Infrastructure;
using NakedBank.WebApi.Invocables;

namespace NakedBank.WebApi.Extensions
{
    public static class InjectionsServiceCollectionExtensions
    {
        public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Application.Interfaces.IUserService, Application.Services.UserService>();
            services.AddScoped<Application.Interfaces.IAccountService, Application.Services.AccountService>();

            services.AddScoped<Application.Repositories.IConfigurationRepository, Infrastructure.Repositories.ConfigurationRepository>();
            services.AddScoped<Application.Repositories.IUserRepository, Infrastructure.Repositories.UserRepository>();
            services.AddScoped<Application.Repositories.IAccountRepository, Infrastructure.Repositories.AccountRepository>();
            services.AddScoped<Application.Repositories.ITransactionRepository, Infrastructure.Repositories.TransactionRepository>();

            services.AddDbContext<NakedContext>(options => options.UseMySQL(configuration["MySqlConfig:ConnectionString"]));

            services.AddTransient<BalanceUpdateInvocable>();

            services.AddScheduler();

            return services;
        }

    }
}
