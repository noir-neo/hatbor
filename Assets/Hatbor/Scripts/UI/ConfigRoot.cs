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
        readonly IFileBrowser fileBrowser;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public ConfigRoot(UIDocument uiDocument,
            IEnumerable<IConfigurable> configs,
            IFileBrowser fileBrowser)
        {
            this.uiDocument = uiDocument;
            this.configs = configs;
            this.fileBrowser = fileBrowser;
        }

        void IStartable.Start()
        {
            var container = uiDocument.rootVisualElement;
            foreach (var config in configs)
            {
                var configGroup = new ConfigGroup(fileBrowser);
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