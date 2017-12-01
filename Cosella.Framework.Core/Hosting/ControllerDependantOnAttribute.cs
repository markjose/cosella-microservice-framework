using System;

namespace Cosella.Framework.Core.Hosting
{
    public class ControllerDependantOnAttribute : Attribute
    {
        public Type[] Types { get; }

        public ControllerDependantOnAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
