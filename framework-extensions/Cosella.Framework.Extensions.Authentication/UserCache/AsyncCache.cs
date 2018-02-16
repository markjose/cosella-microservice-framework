using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public class AsyncCache<T> : IAsyncCache<T>
    {
        private readonly ConcurrentDictionary<string, AsyncCacheItem> _cache;
        private readonly Func<IEnumerable<string>, IEnumerable<T>> _fetchItemsFunc;
        private readonly Func<T, string> _getKeyFunc;

        public AsyncCache(
            Func<IEnumerable<string>, IEnumerable<T>> fetchItemsFunc,
            Func<T, string> getKeyFunc)
        {
            _cache = new ConcurrentDictionary<string, AsyncCacheItem>();
            _fetchItemsFunc = fetchItemsFunc;
            _getKeyFunc = getKeyFunc;
        }
    }
}
