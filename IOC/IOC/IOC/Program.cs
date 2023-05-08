using System;
using IOC.UnitTest;

namespace IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=======测试容器=======");
            IUnitTest testContainer = new TestContainer();
            testContainer.Test();

            Console.WriteLine("=======测试构造函数注入=======");
            IUnitTest testConstructorInjection = new TestConstructorInjection();
            testConstructorInjection.Test();

            Console.ReadKey();
        }
    }
}
