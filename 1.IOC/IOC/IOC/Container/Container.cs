using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace IOC
{
    class Container : IContainer
    {
        Dictionary<Type, object> depot = new Dictionary<Type, object>();

        IInstanceProvider instanceProvider;

        public Container ConfigureInstanceProvider(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
            return this;
        }

        public void Register<TBase, TDerived>() where TDerived : TBase
        {
            InternalRegister<TBase, TDerived>();
        }

        public TBase Resolve<TBase>()
        {
            var obj = Resolve(typeof(TBase));
            if (obj != null) return (TBase)obj;
            return default(TBase);
        }

        public object Resolve(Type type)
        {
            if (depot.ContainsKey(type))
            {
                return depot[type];
            }
            return null;
        }

        void InternalRegister<TBase, TDerived>() where TDerived : TBase
        {
            var instance = instanceProvider.GetInstance<TDerived>();
            depot.Add(typeof(TBase), instance);
        }
    }
}
