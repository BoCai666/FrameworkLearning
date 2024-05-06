using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Promise
{
    [AsyncMethodBuilder(typeof(AsyncPromiseMethodBuilder))]
    [StructLayout(LayoutKind.Auto)]
    public readonly partial struct Promise : IAwaitable
    {
        public readonly IPromiseAction action;

        public EPromiseStatus Status
        {
            get
            {
                if (action == null) return EPromiseStatus.Succeed;
                return action.GetStatus();
            }
        }

        public Promise(IPromiseAction action)
        {
            this.action = action;
        }

        public IAwaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public readonly struct Awaiter : IAwaiter
        {
            private readonly Promise promise;
            public bool IsCompleted => promise.action == null || promise.action.GetStatus() != EPromiseStatus.Pending;

            public Awaiter(Promise promise)
            {
                this.promise = promise;
            }

            public void GetResult()
            {
                if (promise.action == null) return;
                promise.action.GetResult();
            }

            public void OnCompleted(Action continuation)
            {
                if (promise.action == null) continuation.Invoke();
                else promise.action.OnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                if (promise.action == null) continuation.Invoke();
                else promise.action.OnCompleted(continuation);
            }
        }
    }

    [AsyncMethodBuilder(typeof(AsyncPromiseMethodBuilder<>))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct Promise<T> : IAwaitable<T>
    {
        public readonly IPromiseAction<T> action;
        public T Result { get; private set; }

        public EPromiseStatus Status
        {
            get
            {
                if (action == null) return EPromiseStatus.Succeed;
                return action.GetStatus();
            }
        }

        public Promise(IPromiseAction<T> action)
        {
            this.action = action;
            Result = default;
        }

        public IAwaiter<T> GetAwaiter()
        {
            return new Awaiter<T>(this);
        }

        public readonly struct Awaiter<T> : IAwaiter<T>
        {
            private readonly Promise<T> promise;
            public bool IsCompleted => promise.action.GetStatus() != EPromiseStatus.Pending;

            public Awaiter(Promise<T> promise)
            {
                this.promise = promise;
            }

            public T GetResult()
            {
                if (promise.action == null) return promise.Result;
                return promise.action.GetResult();
            }

            public void OnCompleted(Action continuation)
            {
                if (promise.action == null) continuation.Invoke();
                else promise.action.OnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                this.OnCompleted(continuation);
            }
        }
    }
}
