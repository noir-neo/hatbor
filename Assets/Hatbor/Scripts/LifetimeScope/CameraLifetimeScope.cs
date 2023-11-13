using Hatbor.Camera;
using Hatbor.TextureStreaming;
using Klak.Spout;
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
        [SerializeField] SpoutResources spoutResources;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(mainCamera);
            builder.RegisterInstance(rawImage);

            builder.RegisterEntryPoint<RenderTextureProvider>(Lifetime.Singleton).AsSelf();

#if UNITY_STANDALONE_OSX
            builder.Register<ITextureSender, TextureStreaming.Syphon.SyphonSender>(Lifetime.Singleton);
#elif UNITY_STANDALONE_WIN
            builder.RegisterInstance(spoutResources);
            builder.Register<ITextureSender, TextureStreaming.Spout.SpoutSender>(Lifetime.Singleton);
#endif
            builder.RegisterEntryPoint<TextureStreamingSender>(Lifetime.Singleton);

            builder.RegisterEntryPoint<FixedCameraController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<Camera.Camera>(Lifetime.Singleton);
            builder.RegisterEntryPoint<CameraCanvas>(Lifetime.Singleton);
        }
    }
}