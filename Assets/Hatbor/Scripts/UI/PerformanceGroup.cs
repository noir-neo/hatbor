using System;
using Hatbor.PerformanceProfiler;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class PerformanceGroup : VisualElement
    {
        readonly IFileBrowser fileBrowser;

        readonly Label label;
        readonly VisualElement container;

        public PerformanceGroup()
        {

            label = new Label();
            hierarchy.Add(label);
            container = new VisualElement();
            hierarchy.Add(container);
        }

        public IDisposable Bind(IProfilerRecorder recorder)
        {
            var disposables = new CompositeDisposable();
            recorder.Text
                .Subscribe(t => label.text = t)
                .AddTo(disposables);
            return disposables;
        }
    }
}