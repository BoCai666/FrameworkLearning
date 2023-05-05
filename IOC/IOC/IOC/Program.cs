using System;
using IOC.UnitTest;

namespace IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnitTest testContainer = new TestContainer();
            testContainer.Test();


            Console.ReadKey();
        }
    }
}
