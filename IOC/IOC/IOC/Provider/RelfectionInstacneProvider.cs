using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    class RelfectionInstacneProvider : IInstanceProvider
    {
        public T GetInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
