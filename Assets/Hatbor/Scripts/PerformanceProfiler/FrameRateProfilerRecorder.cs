using UniRx;

namespace Hatbor.PerformanceProfiler
{
    public sealed class FrameRateProfilerRecorder : IProfilerRecorder
    {
        readonly ReactiveProperty<string> text = new();
        IReadOnlyReactiveProperty<string> IProfilerRecorder.Text => text;

        int count;
        float time;

        void IProfilerRecorder.Tick(float t)
        {
            count++;
            time += t;
            if (!(time >= 1f)) return;
            var frameRate = count / time;
            text.Value = $"Frame Rate: {frameRate:F2}/s";
            time = 0f;
            count = 0;
        }
    }
}