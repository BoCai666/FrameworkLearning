using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public interface IPromiseAction
    {
        EPromiseStatus GetStatus();
        void GetResult();
        void OnCompleted(Action continuation);
        void UnsafeOnCompleted(Action continuation);
    }

    public interface IPromiseAction<T>
    {
        EPromiseStatus GetStatus();
        T GetResult();
        void OnCompleted(Action continuation);
        void UnsafeOnCompleted(Action continuation);
    }
}
