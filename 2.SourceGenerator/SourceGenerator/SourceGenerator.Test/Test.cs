using QSFramework.Runtime.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGenerator.Test
{
    public class Test
    {
        [Inject]
        public ITimeService timeService;
    }
}
