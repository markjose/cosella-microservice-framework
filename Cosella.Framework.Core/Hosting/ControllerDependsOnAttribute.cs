using System;

namespace Cosella.Framework.Core.Hosting
{
    public class ControllerDependsOnAttribute : Attribute
    {
        public Type[] Types { get; }

        public ControllerDependsOnAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
