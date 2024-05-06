using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public sealed class InstanceProvider : IInstanceProvider
    {
        private readonly IInjector injector;
        private readonly IReadOnlyList<IInjectParameter> injectParameters;

        public InstanceProvider(IInjector injector, IReadOnlyList<IInjectParameter> injectParameters)
        {
            this.injector = injector;
            this.injectParameters = injectParameters;
        }

        public object GetInstance(IObjectResolver resolver)
        {
            return injector.CreateInstance(resolver, injectParameters);
        }
    }
}