using System;

namespace QSFramework.Runtime.IOC
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
        }
    }
}