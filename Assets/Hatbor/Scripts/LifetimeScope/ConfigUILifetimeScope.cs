using Hatbor.Scripts.FileBrowser;
using Hatbor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class ConfigUILifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UIDocument configUIDocument;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IFileBrowser, StandaloneFileBrowserWrapper>(Lifetime.Singleton);
            builder.RegisterInstance(configUIDocument);
            builder.RegisterEntryPoint<ConfigRoot>();
        }
    }
}
