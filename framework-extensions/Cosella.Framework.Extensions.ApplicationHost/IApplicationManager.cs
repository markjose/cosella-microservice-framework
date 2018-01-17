using System;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public interface IApplicationManager
    {
        bool Empty { get; }
        HostedApplication DefaultApp { get; }
        HostedApplication Add(HostedApplicationMeta appRequest, bool isDefault = false);
        HostedApplication Get(string appId);
        HostedApplication FromAlias(string alias);
        HostedApplication Remove(string appId);
        Dictionary<Guid, HostedApplicationMeta> List();
        HostedApplicationMeta GetSummary(string appId);
        HostedApplicationResource GetResource(string appId, string resourceId);
    }
}
