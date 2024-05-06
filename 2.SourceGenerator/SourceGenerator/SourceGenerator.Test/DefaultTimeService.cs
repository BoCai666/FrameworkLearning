using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGenerator.Test
{
    public class DefaultTimeService
    {
        public float FrameCount { get; private set; }
        public float Time { get; private set; }
        public float UnscaledTime { get; private set; }
        public float DeltaTime => 0;
        public float UnscaledDeltaTime => 0;
        public float Timescale { get; private set; } = 1;
    }
}
