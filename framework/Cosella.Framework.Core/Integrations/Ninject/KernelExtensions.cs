using Ninject;

namespace Cosella.Framework.Core.Integrations.Ninject
{
    public static class KernelExtensions
    {
        public static IKernel CreateChild(this IKernel parentKernel)
        {
            return new NinjectChildKernel(parentKernel);
        }
    }
}
