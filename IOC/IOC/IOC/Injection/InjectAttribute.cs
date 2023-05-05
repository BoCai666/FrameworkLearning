using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    class InjectAttribute : Attribute
    {
    }
}
