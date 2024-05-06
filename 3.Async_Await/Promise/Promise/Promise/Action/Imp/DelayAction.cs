using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public sealed class DelayAction : PromiseActionBase, ITicker
    {
        float timer;
        float delay;

        private DelayAction() { }
        public static DelayAction Create(float delay)
        {
            var action = new DelayAction();
            action.delay = delay;
            action.status = EPromiseStatus.Pending;
            action.timer = 0;
            action.continuation = null;
            TickManager.Inst.AddTicker(action);
            return action;
        }

        public bool Tick(float dt)
        {
            timer += dt;
            if (timer >= delay)
            {
                SetResult();
                return true;
            }
            return false;
        }
    }
}
