using UniRx;

namespace Hatbor.PerformanceProfiler
{
    public sealed class VmcServerProfilerRecorder : IProfilerRecorder
    {
        readonly ReactiveProperty<string> text = new();
        IReadOnlyReactiveProperty<string> IProfilerRecorder.Text => text;

        int count;
        float time;

        void IProfilerRecorder.Tick(float t)
        {
            time += t;
            if (!(time >= 1f)) return;
            var frameRate = count / time;
            text.Value = $"VMC Received OK: {frameRate:F2}/s";
            time = 0f;
            count = 0;
        }

        public void IncrementCount()
        {
            count++;
        }
    }
}