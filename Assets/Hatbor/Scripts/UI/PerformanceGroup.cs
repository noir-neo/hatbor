using System;
using Hatbor.PerformanceProfiler;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class PerformanceGroup : VisualElement
    {
        readonly Label label;

        public PerformanceGroup()
        {
            AddToClassList("status-chip");
            label = new Label();
            hierarchy.Add(label);
        }

        public IDisposable Bind(IProfilerRecorder recorder)
        {
            return recorder.Text.Subscribe(t => label.text = t);
        }
    }
}
