using Ninject;
using System;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public class AsyncCacheFactory : IAsyncCacheFactory
    {
        private readonly Dictionary<Type, object> _caches;

        public AsyncCacheFactory()
        {
            _caches = new Dictionary<Type, object>();
        }

        public IAsyncCache<T> Create<T>(
            Func<IEnumerable<string>, IEnumerable<T>> fetchItemsFunc,
            Func<T, string> getKeyFunc)
        {
            if(_caches.TryGetValue(typeof(T), out object cacheObj))
            {
                return cacheObj as IAsyncCache<T>;
            }
            var cache = new AsyncCache<T>(fetchItemsFunc, getKeyFunc);
            _caches.Add(typeof(T), cache);

            return cache as IAsyncCache<T>;
        }
    }
}
