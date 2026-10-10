using System;
using System.Collections.Generic;
using Hatbor.Config;
using Hatbor.PerformanceProfiler;
using UniRx;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.UI
{
    public sealed class ConfigRoot : IStartable, ITickable, IDisposable
    {
        readonly UIDocument uiDocument;
        readonly IEnumerable<IConfigurable> configs;
        readonly IEnumerable<IProfilerRecorder> profilerRecorders;
        readonly IFileBrowser fileBrowser;

        readonly CompositeDisposable disposables = new();

        VisualElement panel;
        bool isEditingText;

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
            panel = root.Q<VisualElement>("config-panel");
            root.RegisterCallback<FocusInEvent>(OnFocusIn);
            root.RegisterCallback<FocusOutEvent>(OnFocusOut);
            Disposable.Create(() =>
                {
                    root.UnregisterCallback<FocusInEvent>(OnFocusIn);
                    root.UnregisterCallback<FocusOutEvent>(OnFocusOut);
                })
                .AddTo(disposables);
            root.Q<Button>("panel-toggle").OnClickAsObservable()
                .Subscribe(_ => TogglePanel())
                .AddTo(disposables);
            var statusBar = root.Q<VisualElement>("status-bar");
            foreach (var recorder in profilerRecorders)
            {
                var performanceGroup = new PerformanceGroup();
                performanceGroup.Bind(recorder).AddTo(disposables);
                statusBar.Add(performanceGroup);
            }
            var groups = root.Q<ScrollView>("config-groups");
            foreach (var config in configs)
            {
                var configGroup = new ConfigGroup(fileBrowser);
                configGroup.Bind(config).AddTo(disposables);
                groups.Add(configGroup);
            }
        }

        void ITickable.Tick()
        {
            if (Keyboard.current is not { hKey: { wasPressedThisFrame: true } }) return;
            if (isEditingText) return;
            TogglePanel();
        }

        void OnFocusIn(FocusInEvent e)
        {
            isEditingText = e.target is TextElement { parent: { } parent } &&
                            parent.ClassListContains(TextInputBaseField<string>.inputUssClassName);
        }

        void OnFocusOut(FocusOutEvent e)
        {
            isEditingText = false;
        }

        void TogglePanel()
        {
            panel.ToggleInClassList("config-panel--hidden");
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}