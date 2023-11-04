using System;
using System.Collections.Generic;
using Hatbor.Config;
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

        readonly CompositeDisposable disposables = new();

        [Inject]
        public ConfigRoot(UIDocument uiDocument,
            IEnumerable<IConfigurable> configs)
        {
            this.uiDocument = uiDocument;
            this.configs = configs;
        }

        void IStartable.Start()
        {
            var container = uiDocument.rootVisualElement;
            foreach (var config in configs)
            {
                var configGroup = new ConfigGroup();
                configGroup.Bind(config).AddTo(disposables);
                container.Q<VisualElement>("unity-content-container").Add(configGroup);
            }
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}