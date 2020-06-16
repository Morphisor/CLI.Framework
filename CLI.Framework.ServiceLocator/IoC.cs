using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CLI.Framework.ServiceLocator
{
    public static class IoC
    {
        private static readonly ConcurrentDictionary<Type, ServiceDeclaration> _transientDeclarations;
        private static readonly ConcurrentDictionary<Type, SingletonServiceDeclaration> _singletonDeclarations;

        static IoC()
        {
            _transientDeclarations = new ConcurrentDictionary<Type, ServiceDeclaration>();
            _singletonDeclarations = new ConcurrentDictionary<Type, SingletonServiceDeclaration>();
        }

        public static void AddTransient<Interface, Implementation>()
            where Interface : class
            where Implementation : class, Interface
        {
            var interfaceType = typeof(Interface);
            var implementationType = typeof(Implementation);
            var serviceDeclaration = new ServiceDeclaration(interfaceType, implementationType);
            _transientDeclarations.AddOrUpdate(interfaceType, serviceDeclaration, (key, existingVal) => serviceDeclaration);
        }

        public static void AddSingleton<Interface, Implementation>()
            where Interface : class
            where Implementation : class, Interface
        {
            var interfaceType = typeof(Interface);
            var implementationType = typeof(Implementation);
            var singletonDeclaration = new SingletonServiceDeclaration(interfaceType, implementationType);
            _singletonDeclarations.AddOrUpdate(interfaceType, singletonDeclaration, (key, existingVal) => singletonDeclaration);
        }

        public static Interface GetService<Interface>()
            where Interface : class
        {
            var interfaceType = typeof(Interface);
            var service = GetServiceInternal(interfaceType);
            return service as Interface;
        }

        private static object GetServiceInternal(Type interfaceType)
        {
            var serviceEntry = LocateServiceDeclaration(interfaceType);

            if (serviceEntry is SingletonServiceDeclaration && (serviceEntry as SingletonServiceDeclaration).Instance != null)
                return (serviceEntry as SingletonServiceDeclaration).Instance;

            var serviceCtor = serviceEntry.GetImplementationCtor();
            var parameters = serviceCtor.GetParameters();
            var ctorParams = new object[] { };

            if (parameters.Length > 0)
            {
                var dependencies = new List<object>();
                foreach (var parameter in parameters)
                {
                    var dependency = GetServiceInternal(parameter.ParameterType);
                    dependencies.Add(dependency);
                }
                ctorParams = dependencies.ToArray();
            }

            var serviceInstance = serviceCtor.Invoke(ctorParams);

            if (serviceEntry is SingletonServiceDeclaration)
                (serviceEntry as SingletonServiceDeclaration).Instance = serviceInstance;

            return serviceInstance;
        }

        private static ServiceDeclaration LocateServiceDeclaration(Type interfaceType)
        {
            ServiceDeclaration entry = null;
            if (_transientDeclarations.ContainsKey(interfaceType))
                entry = _transientDeclarations[interfaceType];
            else
                entry = _singletonDeclarations[interfaceType];

            return entry;
        }
    }
}
