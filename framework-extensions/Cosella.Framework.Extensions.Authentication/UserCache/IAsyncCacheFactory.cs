using System;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.UserCache
{
    public interface IAsyncCacheFactory
    {
        IAsyncCache<T> Create<T>(
            Func<IEnumerable<string>, IEnumerable<T>> fetchItemsFunc,
            Func<T, string> getKeyFunc);
    }
}
