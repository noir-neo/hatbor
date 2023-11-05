using Hatbor.Camera;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class CameraLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UnityEngine.Camera mainCamera;
        [SerializeField] RawImage rawImage;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(mainCamera);
            builder.RegisterInstance(rawImage);
            builder.RegisterEntryPoint<Camera.Camera>(Lifetime.Singleton);
            builder.RegisterEntryPoint<CameraCanvas>(Lifetime.Singleton);
        }
    }
}