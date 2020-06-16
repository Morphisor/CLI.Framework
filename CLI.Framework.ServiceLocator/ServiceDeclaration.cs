using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CLI.Framework.ServiceLocator
{
    internal class ServiceDeclaration
    {
        public Type Interface { get; set; }
        public Type Implementation { get; set; }

        public ServiceDeclaration(Type intfc, Type implementation)
        {
            Interface = intfc;
            Implementation = implementation;
        }

        public ConstructorInfo GetImplementationCtor()
        {
            var ctors = Implementation.GetConstructors();
            var ctor = ctors.First(c => c.IsPublic);
            return ctor;
        }
    }
}
