using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public class Container : IObjectResolver
    {
        private readonly Registry registry;
        private readonly Dictionary<Registration, object> sharedInstances;

        public Container(Registry registry)
        {
            this.registry = registry;
            sharedInstances = new Dictionary<Registration, object>();
        }

        public object Resolve(Type type)
        {
            if (registry.TryGet(type, out var registration))
            {
                return ResloveInternal(registration);
            }
            throw new Exception($"Type {type} is not registered");
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object ResolveParameter(Type parameterType, string parameterName, IReadOnlyList<IInjectParameter> injectParameters)
        {
            if (injectParameters != null)
            {
                foreach (var injectParameter in injectParameters)
                {
                    if (injectParameter.Match(parameterType, parameterName))
                    {
                        return injectParameter.Value;
                    }
                }
            }
            return Resolve(parameterType);
        }

        public void Inject(object instance)
        {
            var injector = ReflectionInjector.Build(instance.GetType());
            injector.Inject(instance, this, null);
        }

        private object ResloveInternal(Registration registration)
        {
            switch (registration.lifeTime)
            {
                case ELifetime.Singleton:
                    if (!sharedInstances.TryGetValue(registration, out var instance))
                    {
                        instance = registration.GetInstance(this);
                        sharedInstances.Add(registration, instance);
                    }
                    return instance;
                case ELifetime.Transient:
                default:
                    return registration.GetInstance(this);
            }
        }
    }
}