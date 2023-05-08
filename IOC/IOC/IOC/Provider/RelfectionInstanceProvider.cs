using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IOC
{
    class RelfectionInstanceProvider : IInstanceProvider
    {
        private IContainer container;

        public RelfectionInstanceProvider(IContainer container)
        {
            this.container = container;
        }

        public TDerived GetInstance<TDerived>()
        {
            return (TDerived)InternalGetInstance(typeof(TDerived));
        }

        object InternalGetInstance(Type type)
        {
            var ctor = GetConstructorInfo(type);
            var parameters = ctor.GetParameters();
            object[] parametersCache = null;
            if (parameters.Length > 0)
            {
                parametersCache = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var item = parameters[i];
                    var obj = container.Resolve(item.ParameterType);
                    if (obj == null) obj = InternalGetInstance(item.ParameterType);
                    parametersCache[i] = obj;
                }
            }
            return ctor.Invoke(parametersCache);
        }

        ConstructorInfo GetConstructorInfo(Type type)
        {
            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            ConstructorInfo info = null;
            var maxParamsCountCtorIndex = 0;
            var maxParamsCount = 0;
            for (int i = 0; i < ctors.Length; i++)
            {
                var item = ctors[i];
                var paramsCount = item.GetParameters().Length;
                if (item.IsDefined(typeof(InjectAttribute)))
                {
                    info = item;
                    break;
                }
                else if (paramsCount > maxParamsCount)
                {
                    maxParamsCount = paramsCount;
                    maxParamsCountCtorIndex = i;
                }
            }
            if (info == null) info = ctors[maxParamsCountCtorIndex];
            return info;
        }
    }
}
