﻿using Ninject;
using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Cosella.Framework.Core.Hosting
{
    public class CustomControllerResolver : DefaultHttpControllerTypeResolver
    {
        private static IKernel _kernel;

        public CustomControllerResolver(IKernel kernel) : base(IsHttpEndpoint)
        {
            _kernel = kernel;
        }

        internal static bool IsHttpEndpoint(Type controllerType)
        {
            if (controllerType == null) throw new ArgumentNullException("controllerType");

            var isController = controllerType != null
                && controllerType.IsClass
                && controllerType.IsVisible
                && controllerType.IsAbstract == false
                && typeof(IHttpController).IsAssignableFrom(controllerType);

            if (!isController) return false;

            var dependancies = controllerType
                .GetCustomAttributes(typeof(ControllerDependantOnAttribute), true)
                .Cast<ControllerDependantOnAttribute>()
                .SelectMany(a => a.Types);

            var isEnabled = !dependancies.Any() || dependancies.All(t => _kernel.GetBindings(t).Any());

            return isEnabled;
        }
    }
}
