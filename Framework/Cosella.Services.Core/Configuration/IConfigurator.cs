namespace Cosella.Services.Core.Configuration
{
    public interface IConfigurator
    {
        string GetString(string section, string property);
    }
}