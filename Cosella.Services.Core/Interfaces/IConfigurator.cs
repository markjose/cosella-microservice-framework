namespace Cosella.Services.Core.Interfaces
{
    public interface IConfigurator
    {
        string GetString(string section, string property);
    }
}