using System;
using System.Collections.Generic;
using Hatbor.Config;
using Hatbor.PerformanceProfiler;
using UniRx;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.UI
{
    public sealed class ConfigRoot : IStartable, IDisposable
    {
        readonly UIDocument uiDocument;
        readonly IEnumerable<IConfigurable> configs;
        readonly IEnumerable<IProfilerRecorder> profilerRecorders;
        readonly IFileBrowser fileBrowser;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public ConfigRoot(UIDocument uiDocument,
            IEnumerable<IConfigurable> configs,
            IEnumerable<IProfilerRecorder> profilerRecorders,
            IFileBrowser fileBrowser)
        {
            this.uiDocument = uiDocument;
            this.configs = configs;
            this.profilerRecorders = profilerRecorders;
            this.fileBrowser = fileBrowser;
        }

        void IStartable.Start()
        {
            var root = uiDocument.rootVisualElement;
            var container = root.Q<VisualElement>("unity-content-container");
            foreach (var recorder in profilerRecorders)
            {
                var performanceGroup = new PerformanceGroup();
                performanceGroup.Bind(recorder).AddTo(disposables);
                container.Add(performanceGroup);
            }
            foreach (var config in configs)
            {
                var configGroup = new ConfigGroup(fileBrowser);
                configGroup.Bind(config).AddTo(disposables);
                container.Add(configGroup);
            }
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}