using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.PerformanceProfiler
{
    public sealed class PerformanceProfilerTicker : ITickable
    {
        readonly IEnumerable<IProfilerRecorder> profilerRecorders;

        [Inject]
        public PerformanceProfilerTicker(IEnumerable<IProfilerRecorder> profilerRecorders)
        {
            this.profilerRecorders = profilerRecorders;
        }

        void ITickable.Tick()
        {
            foreach (var recorder in profilerRecorders)
            {
                recorder.Tick(Time.unscaledDeltaTime);
            }
        }
    }
}