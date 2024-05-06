using System;
using System.Collections.Generic;
using System.Reflection;

namespace QSFramework.Runtime.IOC
{
    static class TypeAnalyzer
    {
        static readonly Dictionary<Type, InjectTypeInfo> typeInfoCache = new Dictionary<Type, InjectTypeInfo>();

        public static InjectTypeInfo AnalyzeWithCache(Type type)
        {
            if (!typeInfoCache.TryGetValue(type, out var typeInfo))
            {
                typeInfo = AnalyzeType(type);
                typeInfoCache.Add(type, typeInfo);
            }
            return typeInfo;
        }

        public static InjectTypeInfo AnalyzeType(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            InjectConstructorInfo injectConstructor = null;
            var constructorCount = 0;
            var maxConstructorParameters = -1;
            foreach (var constructor in typeInfo.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (constructor.IsDefined(typeof(InjectAttribute), false))
                {
                    if (++constructorCount > 1)
                    {
                        throw new Exception($"Type found multiple [Inject] marked constructors, type: {type.Name}");
                    }
                    injectConstructor = new InjectConstructorInfo(constructor);
                }
                else if (constructorCount <= 0)
                {
                    var parameterInfos = constructor.GetParameters();
                    if (parameterInfos.Length > maxConstructorParameters)
                    {
                        maxConstructorParameters = parameterInfos.Length;
                        injectConstructor = new InjectConstructorInfo(constructor, parameterInfos);
                    }
                }
            }

            List<InjectMethodInfo> injectMethods = null;
            List<FieldInfo> injectFields = null;
            List<PropertyInfo> injectProperties = null;
            var injectAttributeType = typeof(InjectAttribute);
            foreach (var methodInfo in typeInfo.GetRuntimeMethods())
            {
                if (methodInfo.IsDefined(injectAttributeType, true))
                {
                    if (injectMethods == null)
                    {
                        injectMethods = new List<InjectMethodInfo>(2);
                    }
                    injectMethods.Add(new InjectMethodInfo(methodInfo));
                }
            }
            foreach (var fieldInfo in type.GetRuntimeFields())
            {
                if (fieldInfo.IsDefined(injectAttributeType, true))
                {
                    if (injectFields == null)
                    {
                        injectFields = new List<FieldInfo>(2);
                    }
                    injectFields.Add(fieldInfo);
                }
            }
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                if (propertyInfo.IsDefined(injectAttributeType, true))
                {
                    if (injectProperties == null)
                    {
                        injectProperties = new List<PropertyInfo>(2);
                    }
                    injectProperties.Add(propertyInfo);
                }
            }
            return new InjectTypeInfo(type, injectConstructor, injectMethods, injectFields, injectProperties);
        }
    }

    sealed class InjectTypeInfo
    {
        public readonly Type type;
        public readonly InjectConstructorInfo injectConstructor;
        public readonly IReadOnlyList<InjectMethodInfo> injectMethods;
        public readonly IReadOnlyList<FieldInfo> injectFields;
        public readonly IReadOnlyList<PropertyInfo> injectProperties;

        public InjectTypeInfo(Type type,
            InjectConstructorInfo injectConstructor,
            IReadOnlyList<InjectMethodInfo> injectMethods,
            IReadOnlyList<FieldInfo> injectFields,
            IReadOnlyList<PropertyInfo> injectProperties)
        {
            this.type = type;
            this.injectConstructor = injectConstructor;
            this.injectMethods = injectMethods;
            this.injectFields = injectFields;
            this.injectProperties = injectProperties;
        }
    }

    sealed class InjectConstructorInfo
    {
        public readonly ConstructorInfo constructorInfo;
        public readonly IReadOnlyList<ParameterInfo> parameterInfos;

        public InjectConstructorInfo(ConstructorInfo constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            parameterInfos = constructorInfo.GetParameters();
        }

        public InjectConstructorInfo(ConstructorInfo constructorInfo, IReadOnlyList<ParameterInfo> parameterInfos)
        {
            this.constructorInfo = constructorInfo;
            this.parameterInfos = parameterInfos;
        }
    }

    sealed class InjectMethodInfo
    {
        public readonly MethodInfo methodInfo;
        public readonly IReadOnlyList<ParameterInfo> parameterInfos;

        public InjectMethodInfo(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
            parameterInfos = methodInfo.GetParameters();
        }
    }

}