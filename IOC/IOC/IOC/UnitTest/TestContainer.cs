using System;
using System.Collections.Generic;
using System.Text;

namespace IOC.UnitTest
{
    class TestContainer : IUnitTest
    {
        interface IPeople { }

        class Teacher : IPeople { }

        public void Test()
        {
            var c = new Container();
            c.Register<IPeople, Teacher>();
            var t = c.Resolve<IPeople>();
            Console.WriteLine(t.GetType());
        }
    }
}
