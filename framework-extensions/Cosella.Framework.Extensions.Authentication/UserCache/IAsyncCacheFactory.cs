namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public interface IAsyncCacheFactory
    {
        IAsyncCache<T> Get<T>();
    }
}
