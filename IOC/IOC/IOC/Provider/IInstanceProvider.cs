using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    interface IInstanceProvider
    {
        T GetInstance<T>();
    }
}
