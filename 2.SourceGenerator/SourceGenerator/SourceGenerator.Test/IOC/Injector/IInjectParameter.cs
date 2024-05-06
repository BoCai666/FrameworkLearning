using System;

namespace QSFramework.Runtime.IOC
{
    public interface IInjectParameter
    {
        bool Match(Type type, string parameterName);
        object Value { get; }
    }

    sealed class TypedParameter : IInjectParameter
    {
        public readonly Type type;
        public object Value { get; }

        public TypedParameter(Type type, object value)
        {
            this.type = type;
            Value = value;
        }

        public bool Match(Type type, string parameterName)
        {
            return this.type == type;
        }
    }

    sealed class NamedParameter : IInjectParameter
    {
        public readonly string name;
        public object Value { get; }

        public NamedParameter(string name, object value)
        {
            this.name = name;
            Value = value;
        }

        public bool Match(Type type, string parameterName)
        {
            return parameterName == name;
        }
    }
}