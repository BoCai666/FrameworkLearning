using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public interface IAwaiter : ICriticalNotifyCompletion
    {
        public bool IsCompleted { get;}
        void GetResult();
    }

    public interface IAwaiter<T> : ICriticalNotifyCompletion
    {
        public bool IsCompleted { get;}
        T GetResult();
    }
}
