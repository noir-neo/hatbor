using Hatbor.Light;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class LightLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UnityEngine.Light light;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(light);

            builder.RegisterEntryPoint<LightPresenter>(Lifetime.Singleton);
        }
    }
}