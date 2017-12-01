using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly List<HostedApplication> _apps;
        public HostedApplication DefaultApp { get; private set; } = null;

        public bool Empty => throw new System.NotImplementedException();
        public ApplicationManager(IKernel kernel)
        {
            _apps = new List<HostedApplication>();

            var config = kernel.Get<ApplicationHostConfiguration>();
            config.Applications.ForEach(app =>
            {
                Add(new HostedApplicationMeta()
                {
                    Name = app.Name,
                    Aliases = app.Aliases.ToList(),
                    ApplicationRoot = app.ApplicationRoot,
                    ApplicationType = app.ApplicationType
                }, app.IsDefault);
            });
        }

        public HostedApplication Add(HostedApplicationMeta request, bool isDefault)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ApplicationHostException("You must specify a name for the application.");

            if (_apps.Any(a => a.Meta.Name == request.Name))
                throw new ApplicationHostException($"An application with the name '{request.Name}' exists.");

            if (!request.Aliases.Any(a => !string.IsNullOrWhiteSpace(a)))
                throw new ApplicationHostException($"You must specify at least one alias for this application.");

            var newApp = new HostedApplication(request);
            _apps.Add(newApp);

            if (isDefault) DefaultApp = newApp;

            return newApp;
        }

        public HostedApplication Get(string appId)
        {
            try
            {
                var uid = Guid.Parse(appId);
                var app = _apps.FirstOrDefault(a => a.AppId == uid);
                if (app == null) throw new ApplicationHostException(); // Not found
                return app;
            }
            catch(Exception)
            {
                throw new ApplicationHostException(); // Not found
            }
        }

        public HostedApplication FromAlias(string alias)
        {
            try
            {
                var app = _apps.FirstOrDefault(a => a.Meta.Aliases.Contains(alias));
                if (app == null) throw new ApplicationHostException(); // Not found
                return app;
            }
            catch (Exception)
            {
                throw new ApplicationHostException(); // Not found
            }
        }

        public HostedApplicationMeta GetSummary(string appId)
        {
            return Get(appId)?.Meta;
        }

        public HostedApplicationResource GetResource(string appId, string resourceId)
        {
            return default(HostedApplicationResource);
        }

        public Dictionary<Guid, HostedApplicationMeta> List()
        {
            return _apps.ToDictionary(a => a.AppId, a => a.Meta);
        }

        public HostedApplication Remove(string appId)
        {
            try
            {
                var uid = Guid.Parse(appId);
                var app = _apps.FirstOrDefault(a => a.AppId == uid);
                if (app == null) throw new ApplicationHostException(); // Not found
                _apps.Remove(app);
                return app;
            }
            catch (Exception)
            {
                throw new ApplicationHostException(); // Not found
            }
        }
    }
}
