using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public class RegistrationBuilder
    {
        internal readonly Type implementationType;
        internal readonly ELifetime lifetime;
        internal List<IInjectParameter> injectParameters;
        internal List<Type> interfaceTypes;

        public RegistrationBuilder(Type implementationType, ELifetime lifetime)
        {
            this.implementationType = implementationType;
            this.lifetime = lifetime;
        }

        public virtual Registration Build()
        {
            var injector = ReflectionInjector.Build(implementationType);
            var instanceProvider = new InstanceProvider(injector, injectParameters);
            return new Registration(implementationType, lifetime, interfaceTypes, instanceProvider);
        }

        public RegistrationBuilder As<T>() => As(typeof(T));

        public RegistrationBuilder As(Type interfaceType)
        {
            AddInterfaceType(interfaceType);
            return this;
        }

        public RegistrationBuilder WithParameter<T>(object value) => WithParameter(typeof(T), value);

        public RegistrationBuilder WithParameter(Type type, object value)
        {
            injectParameters = injectParameters ?? new List<IInjectParameter>();
            injectParameters.Add(new TypedParameter(type, value));
            return this;
        }

        public RegistrationBuilder WithParameter(string name,  object value)
        {
            injectParameters = injectParameters ?? new List<IInjectParameter>();
            injectParameters.Add(new NamedParameter(name, value));
            return this;
        }

        private void AddInterfaceType(Type interfaceType)
        {
            if (!interfaceType.IsAssignableFrom(implementationType)) 
            {
                throw new Exception($"{implementationType} is not assignable from {interfaceType}");
            }
            interfaceTypes = interfaceTypes ?? new List<Type>(2);
            if (!interfaceTypes.Contains(interfaceType))
            {
                interfaceTypes.Add(interfaceType);
            }
        }
    }
}