using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public sealed class Registration
    {
        public readonly Type implementationType;
        public readonly ELifetime lifeTime;
        public readonly IInstanceProvider instanceProvider;
        public readonly IReadOnlyList<Type> interfaceTypes;

        public Registration(Type implementationType, ELifetime lifeTime, IReadOnlyList<Type> interfaceTypes, IInstanceProvider instanceProvider)
        {
            this.implementationType = implementationType;
            this.interfaceTypes = interfaceTypes;
            this.lifeTime = lifeTime;
            this.instanceProvider = instanceProvider;
        }

        public object GetInstance(IObjectResolver resolver)
        {
            return instanceProvider.GetInstance(resolver);
        }
    }
}