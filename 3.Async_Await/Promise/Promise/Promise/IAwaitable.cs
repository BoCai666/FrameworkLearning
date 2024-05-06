using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    [AsyncMethodBuilder(typeof(AsyncAwaitableMethodBuilder))]
    public interface IAwaitable
    {
        IAwaiter GetAwaiter();
    }

    [AsyncMethodBuilder(typeof(AsyncAwaitableMethodBuilder<>))]
    public interface IAwaitable<T>
    {
        IAwaiter<T> GetAwaiter();
    }
}
