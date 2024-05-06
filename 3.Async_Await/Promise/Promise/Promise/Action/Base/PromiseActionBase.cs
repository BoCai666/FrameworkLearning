using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public abstract class PromiseActionBase : IPromiseAction
    {
        protected EPromiseStatus status;
        protected Action continuation;

        public virtual void GetResult()
        {
        }

        public virtual EPromiseStatus GetStatus()
        {
            return status;
        }

        public virtual void OnCompleted(Action continuation)
        {
            if (status == EPromiseStatus.Pending) this.continuation = continuation;
            else continuation();
        }

        public virtual void UnsafeOnCompleted(Action continuation)
        {
            this.OnCompleted(continuation);
        }

        public virtual void SetResult()
        {
            status = EPromiseStatus.Succeed;
            continuation?.Invoke();
        }
    }

    public abstract class PromiseActionBase<T> : IPromiseAction<T>
    {
        protected EPromiseStatus status;
        protected Action continuation;
        protected T result;

        public virtual T GetResult()
        {
            return result;
        }

        public virtual EPromiseStatus GetStatus()
        {
            return status;
        }

        public virtual void OnCompleted(Action continuation)
        {
            if (status == EPromiseStatus.Pending) this.continuation = continuation;
            else continuation();
        }

        public virtual void UnsafeOnCompleted(Action continuation)
        {
            this.OnCompleted(continuation);
        }

        public virtual void SetResult(T result)
        {
            this.result = result;
            status = EPromiseStatus.Succeed;
            continuation?.Invoke();
        }
    }
}
