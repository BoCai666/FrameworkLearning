using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    class Container : IContainer
    {
        Dictionary<Type, object> depot = new Dictionary<Type, object>();

        IInstanceProvider instanceProvider;

        public Container() : this(new RelfectionInstacneProvider())
        {
        }

        public Container(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public void Register<TBase, TDerived>() where TDerived : TBase
        {
            InternalRegister<TBase, TDerived>();
        }

        public TBase Resolve<TBase>()
        {
            var type = typeof(TBase);
            if (depot.ContainsKey(type))
            {
                return (TBase)depot[type];
            }
            return default(TBase);
        }

        void InternalRegister<TBase, TDerived>() where TDerived : TBase
        {
            var instance = instanceProvider.GetInstance<TDerived>();
            depot.Add(typeof(TBase), instance);
        }
    }
}
