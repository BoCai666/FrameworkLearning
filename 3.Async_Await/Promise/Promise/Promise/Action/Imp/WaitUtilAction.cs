using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public sealed class WaitUtilAction : PromiseActionBase, ITicker
    {
        Func<bool> predicate;

        private WaitUtilAction() { }
        public static WaitUtilAction Create(Func<bool> predicate)
        {
            var action = new WaitUtilAction();
            action.predicate = predicate;
            TickManager.Inst.AddTicker(action);
            return action;
        }

        public bool Tick(float dt)
        {
            if (predicate.Invoke())
            {
                SetResult();
                return true;
            }
            return false;
        }
    }
}
