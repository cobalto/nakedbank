namespace NakedBank.Application.Repositories
{
    public interface IConfigurationRepository
    {
        string GetConfig(string configName);
    }
}
