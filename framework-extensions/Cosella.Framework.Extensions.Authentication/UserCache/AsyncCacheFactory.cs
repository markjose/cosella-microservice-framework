using Ninject;
using System;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public class AsyncCacheFactory : IAsyncCacheFactory
    {
        private readonly Dictionary<Type, object> _caches;
        private readonly IKernel _kernel;

        public AsyncCacheFactory(IKernel kernel)
        {
            _caches = new Dictionary<Type, object>();
            _kernel = kernel;
        }

        public IAsyncCache<T> Get<T>()
        {
            if(_caches.TryGetValue(typeof(T), out object cache))
            {
                return cache as IAsyncCache<T>;
            }
            cache = _kernel.Get<IAsyncCache<T>>();
            _caches.Add(typeof(T), cache);

            return cache as IAsyncCache<T>;
        }
    }
}
