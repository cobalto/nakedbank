using Microsoft.Extensions.Configuration;
using NakedBank.Application.Repositories;

namespace NakedBank.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public readonly IConfiguration _configuration;

        public ConfigurationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfig(string configName)
        {
            return _configuration[configName];
        }
    }
}
