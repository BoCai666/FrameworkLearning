using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    class ReflectionInjector : IInjector
    {
        private readonly InjectTypeInfo injectTypeInfo;

        public static ReflectionInjector Build(Type type)
        {
            var typeInfo = TypeAnalyzer.AnalyzeWithCache(type);
            return new ReflectionInjector(typeInfo);
        }

        private ReflectionInjector(InjectTypeInfo injectTypeInfo)
        {
            this.injectTypeInfo = injectTypeInfo;
        }

        public object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters)
        {
            var parameterInfos = injectTypeInfo.injectConstructor.parameterInfos;
            object[] parameterValues = null;
            if (parameterInfos != null && parameterInfos.Count > 0) 
            {
                parameterValues = new object[parameterInfos.Count];
                for (int i = 0; i < parameterInfos.Count; i++)
                {
                    var parameterInfo = parameterInfos[i];
                    parameterValues[i] = resolver.ResolveParameter(parameterInfo.ParameterType, parameterInfo.Name, injectParameters);
                }
            }
            var instance = injectTypeInfo.injectConstructor.constructorInfo.Invoke(parameterValues);
            Inject(instance, resolver, injectParameters);
            return instance;
        }

        public void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
        {
            InjectFields(instance, resolver);
            InjectProperties(instance, resolver);
            InjectMethods(instance, resolver, parameters);
        }

        private void InjectFields(object instance, IObjectResolver resolver)
        {
            if (injectTypeInfo.injectFields == null)
            {
                return;
            }
            foreach (var fieldInfo in injectTypeInfo.injectFields)
            {
                var value = resolver.Resolve(fieldInfo.FieldType);
                fieldInfo.SetValue(instance, value);
            }
        }

        private void InjectProperties(object instance, IObjectResolver resolver)
        {
            if (injectTypeInfo.injectProperties == null)
            {
                return;
            }
            foreach (var propertyInfo in injectTypeInfo.injectProperties)
            {
                var value = resolver.Resolve(propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, value);
            }
        }

        private void InjectMethods(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters)
        {
            if (injectTypeInfo.injectMethods == null)
            {
                return;
            }
            foreach (var method in injectTypeInfo.injectMethods)
            {
                var parameterInfos = method.parameterInfos;
                object[] parameterValues = null;
                if (parameterInfos != null && parameterInfos.Count > 0)
                {
                    parameterValues = new object[parameterInfos.Count];
                    for (int i = 0; i < parameterInfos.Count; i++)
                    {
                        var parameterInfo = parameterInfos[i];
                        parameterValues[i] = resolver.ResolveParameter(parameterInfo.ParameterType, parameterInfo.Name, injectParameters);
                    }
                }
                method.methodInfo.Invoke(instance, parameterValues);
            }
        }
    }
}