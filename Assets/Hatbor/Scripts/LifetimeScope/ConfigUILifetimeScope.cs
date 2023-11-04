using Hatbor.UI;
using UnityEngine;
using UnityEngine.Serialization;
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
            builder.RegisterInstance(configUIDocument);
            builder.RegisterEntryPoint<ConfigRoot>();
        }
    }
}
