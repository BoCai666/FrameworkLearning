using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    interface IContainer
    {
        void Register<TBase, TDerived>() where TDerived : TBase; 
        TBase Resolve<TBase>();
    }
}
