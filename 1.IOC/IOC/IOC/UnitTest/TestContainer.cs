using System;
using System.Collections.Generic;
using System.Text;

namespace IOC.UnitTest
{
    class TestContainer : IUnitTest
    {
        public void Test()
        {
            var c = new Container();
            c.ConfigureInstanceProvider(new RelfectionInstanceProvider(c));
            c.Register<IPeople, Teacher>();
            var t = c.Resolve<IPeople>();
            Console.WriteLine(t.GetType());
        }

        interface IPeople { }

        class Teacher : IPeople { }

    }
}
