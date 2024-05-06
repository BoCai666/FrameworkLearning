using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGenerator.Test
{
    public interface ITimeService
    {
        float FrameCount { get; }
        float Time { get; }
        float UnscaledTime { get; }
        float DeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float Timescale { get; }
    }
}
