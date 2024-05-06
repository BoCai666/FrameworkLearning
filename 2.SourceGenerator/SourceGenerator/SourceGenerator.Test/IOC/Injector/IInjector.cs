using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public interface IInjector
    {
        void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters);
        object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters);
    }
}


