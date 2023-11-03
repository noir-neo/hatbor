using Hatbor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class SettingsUILifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UIDocument settingsUIDocument;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(settingsUIDocument);
            builder.RegisterEntryPoint<SettingsRoot>();
        }
    }
}
