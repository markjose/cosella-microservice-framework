using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public class AsyncCache<TResult> where TResult : class, IAsyncCache<TResult>
    {
        private readonly ConcurrentDictionary<string, AsyncCacheItem> _cache;
        private readonly Func<IEnumerable<string>, IEnumerable<TResult>> _fetchItemsFunc;
        private readonly Func<TResult, string> _getKeyFunc;

        public AsyncCache(
            Func<IEnumerable<string>, IEnumerable<TResult>> fetchItemsFunc,
            Func<TResult, string> getKeyFunc)
        {
            _cache = new ConcurrentDictionary<string, AsyncCacheItem>();
            _fetchItemsFunc = fetchItemsFunc;
            _getKeyFunc = getKeyFunc;
        }
    }
}
