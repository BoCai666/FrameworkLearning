# Promise
一个参考UniTask而实现的极简的异步方案，也是对async/await背后原理学习的一个记录。
# async/await原理
async/await与IEnumerator/yieldreturn类似，其标识的方法内部都会被编译器生成一个状态机，两者的状态机运行的方法命名都一致都是MoveNext，只是前者无返回值，后者有个bool返回值。  

状态机内部通过int状态值来分割await/yieldreturn的代码段，执行状态机的MoveNext方法来逐步执行到所有被分割的代码段。

async/await生成的状态机MoveNext方法通过回调调回调的形式往下执行，而IEnumerator/yieldreturn生成的状态机MoveNext方法需在循环中一直检测返回值后往下执行。

# async/await生成代码
```csharp
// 源码
using System;
using System.Threading.Tasks;
public class C {
    public void M() {
        B();
    }
    
    public async Task<int> A()
    {
        Console.Write("A1");
        await Task.Delay(5);
        Console.Write("A2");
        return 1;
    }
    
    public async void B()
    {
        Console.Write("B1");
        var a = await A();
        Console.Write($"B2: {a}");
    }
}

// 编译器生成的代码
public class C
{
    [CompilerGenerated]
    private sealed class <A>d__1 : IAsyncStateMachine
    {
        public int <>1__state;

        public AsyncTaskMethodBuilder<int> <>t__builder;

        public C <>4__this;

        private TaskAwaiter <>u__1;

        private void MoveNext()
        {
            int num = <>1__state;
            int result;
            try
            {
                TaskAwaiter awaiter;
                if (num != 0)
                {
                    Console.Write("A1");
                    awaiter = Task.Delay(5).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        num = (<>1__state = 0);
                        <>u__1 = awaiter;
                        <A>d__1 stateMachine = this;
                        <>t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                        return;
                    }
                }
                else
                {
                    awaiter = <>u__1;
                    <>u__1 = default(TaskAwaiter);
                    num = (<>1__state = -1);
                }
                awaiter.GetResult();
                Console.Write("A2");
                result = 1;
            }
            catch (Exception exception)
            {
                <>1__state = -2;
                <>t__builder.SetException(exception);
                return;
            }
            <>1__state = -2;
            <>t__builder.SetResult(result);
        }

        void IAsyncStateMachine.MoveNext()
        {
            //ILSpy generated this explicit interface implementation from .override directive in MoveNext
            this.MoveNext();
        }

        [DebuggerHidden]
        private void SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
        {
        }

        void IAsyncStateMachine.SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
        {
            //ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
            this.SetStateMachine(stateMachine);
        }
    }


    [CompilerGenerated]
    private sealed class <B>d__2 : IAsyncStateMachine
    {
        public int <>1__state;

        public AsyncVoidMethodBuilder <>t__builder;

        public C <>4__this;

        private int <a>5__1;

        private int <>s__2;

        private TaskAwaiter<int> <>u__1;

        private void MoveNext()
        {
            int num = <>1__state;
            try
            {
                TaskAwaiter<int> awaiter;
                if (num != 0)
                {
                    Console.Write("B1");
                    awaiter = <>4__this.A().GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        num = (<>1__state = 0);
                        <>u__1 = awaiter;
                        <B>d__2 stateMachine = this;
                        <>t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                        return;
                    }
                }
                else
                {
                    awaiter = <>u__1;
                    <>u__1 = default(TaskAwaiter<int>);
                    num = (<>1__state = -1);
                }
                <>s__2 = awaiter.GetResult();
                <a>5__1 = <>s__2;
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                defaultInterpolatedStringHandler.AppendLiteral("B2: ");
                defaultInterpolatedStringHandler.AppendFormatted(<a>5__1);
                Console.Write(defaultInterpolatedStringHandler.ToStringAndClear());
            }
            catch (Exception exception)
            {
                <>1__state = -2;
                <>t__builder.SetException(exception);
                return;
            }
            <>1__state = -2;
            <>t__builder.SetResult();
        }

        void IAsyncStateMachine.MoveNext()
        {
            //ILSpy generated this explicit interface implementation from .override directive in MoveNext
            this.MoveNext();
        }

        [DebuggerHidden]
        private void SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
        {
        }

        void IAsyncStateMachine.SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
        {
            //ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
            this.SetStateMachine(stateMachine);
        }
    }

    public void M()
    {
        B();
    }

    [NullableContext(1)]
    [AsyncStateMachine(typeof(<A>d__1))]
    [DebuggerStepThrough]
    public Task<int> A()
    {
        <A>d__1 stateMachine = new <A>d__1();
        stateMachine.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
        stateMachine.<>4__this = this;
        stateMachine.<>1__state = -1;
        stateMachine.<>t__builder.Start(ref stateMachine);
        return stateMachine.<>t__builder.Task;
    }

    [AsyncStateMachine(typeof(<B>d__2))]
    [DebuggerStepThrough]
    public void B()
    {
        <B>d__2 stateMachine = new <B>d__2();
        stateMachine.<>t__builder = AsyncVoidMethodBuilder.Create();
        stateMachine.<>4__this = this;
        stateMachine.<>1__state = -1;
        stateMachine.<>t__builder.Start(ref stateMachine);
    }
}

```
# 推荐
- [UniTask](https://github.com/Cysharp/UniTask)：一个为Unity提供的高性能，0GC的async/await异步方案
- [ETTask](https://github.com/egametang/ET)：ET框架中的async/await异步方案
- [SharpLab](https://sharplab.io/)：一个快速显示.net代码（如c#）的编译中间过程和结果的网站

