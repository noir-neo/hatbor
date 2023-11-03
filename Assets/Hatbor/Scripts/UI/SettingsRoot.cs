using System;
using System.Collections.Generic;
using Hatbor.Settings;
using UniRx;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.UI
{
    public sealed class SettingsRoot : IStartable, IDisposable
    {
        readonly UIDocument uiDocument;
        readonly IEnumerable<ISettings> settings;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public SettingsRoot(UIDocument uiDocument,
            IEnumerable<ISettings> settings)
        {
            this.uiDocument = uiDocument;
            this.settings = settings;
        }

        void IStartable.Start()
        {
            var container = uiDocument.rootVisualElement;
            foreach (var setting in settings)
            {
                var settingsGroup = new SettingsGroup();
                settingsGroup.Bind(setting).AddTo(disposables);
                container.Q<VisualElement>("unity-content-container").Add(settingsGroup);
            }
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}