using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public interface IObjectResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
        object ResolveParameter(Type parameterType, string parameterName, IReadOnlyList<IInjectParameter> parameters);
        void Inject(object instance);

    }

    public enum ELifetime
    {
        Transient,
        Singleton
    }
}