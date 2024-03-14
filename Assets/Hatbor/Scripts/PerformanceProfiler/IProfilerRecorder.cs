using UniRx;

namespace Hatbor.PerformanceProfiler
{
    public interface IProfilerRecorder
    {
        IReadOnlyReactiveProperty<string> Text { get; }

        public void Tick(float t);
    }
}