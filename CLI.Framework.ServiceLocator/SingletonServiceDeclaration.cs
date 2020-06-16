using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Framework.ServiceLocator
{
    internal class SingletonServiceDeclaration : ServiceDeclaration
    {
        public object Instance { get; set; }

        public SingletonServiceDeclaration(Type intfc, Type implementation) : base(intfc, implementation)
        {
        }
    }
}
