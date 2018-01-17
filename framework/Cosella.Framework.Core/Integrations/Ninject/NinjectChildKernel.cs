using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Activation;
using Ninject.Planning.Bindings;

namespace Cosella.Framework.Core.Integrations.Ninject
{
    internal class NinjectChildKernel : StandardKernel
    {
        private IKernel _parent;

        public NinjectChildKernel(IKernel parentKernel)
        {
            _parent = parentKernel;
        }

        public override bool CanResolve(IRequest request)
        {
            return base.CanResolve(request) || _parent.CanResolve(request);
        }

        public override bool CanResolve(IRequest request, bool ignoreImplicitBindings)
        {
            return base.CanResolve(request, ignoreImplicitBindings) || _parent.CanResolve(request, ignoreImplicitBindings);
        }

        public override IEnumerable<IBinding> GetBindings(Type service)
        {
            var bindings = _parent.GetBindings(service)
                .Concat(base.GetBindings(service))
                .GroupBy(binding => binding?.Service?.Name)
                .Select(group => group.Last());

            return bindings;
        }

        public override IEnumerable<object> Resolve(IRequest request)
        {
            var objects = _parent.Resolve(request)
                .Concat(base.Resolve(request))
                .Distinct()
                .Reverse()
                .Take(1);

            return objects;
        }
    }
}