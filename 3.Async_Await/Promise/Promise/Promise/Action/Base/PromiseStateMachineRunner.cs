using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public sealed class PromiseStateMachineRunner : PromiseActionBase
    {
        public readonly Promise promise;
        private readonly IAsyncStateMachine stateMachine;

        public PromiseStateMachineRunner(IAsyncStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            status = EPromiseStatus.Pending;
            promise = new Promise(this);
        }

        public void MoveNext()
        {
            stateMachine.MoveNext();
        }

        public void SetException(Exception exception)
        {
        }
    }

    public class PromiseStateMachineRunner<T> : PromiseActionBase<T>
    {
        public readonly Promise<T> promise;
        private readonly IAsyncStateMachine stateMachine;

        public PromiseStateMachineRunner(IAsyncStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            status = EPromiseStatus.Pending;
            promise = new Promise<T>(this);
        }

        public void MoveNext()
        {
            stateMachine.MoveNext();
        }

        public void SetException(Exception exception)
        {
        }
    }
}
