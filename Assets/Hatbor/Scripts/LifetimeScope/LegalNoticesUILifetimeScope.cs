using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class LegalNoticesUILifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UIDocument uiDocument;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(uiDocument);
            builder.RegisterEntryPoint<UI.LegalNoticesRoot>(Lifetime.Singleton);
        }
    }
}