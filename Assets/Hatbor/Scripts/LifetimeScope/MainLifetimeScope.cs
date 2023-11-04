using Hatbor.Avatar;
using Hatbor.Rig;
using Hatbor.Rig.VMC;
using Hatbor.Config;
using Hatbor.VMC;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class MainLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] UnityEngine.Camera mainCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IConfigurable, VmcServerConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<VmcServer>(Lifetime.Singleton).AsSelf();

            builder.Register<IRootTransformRig, VmcRootTransformRig>(Lifetime.Singleton);
            builder.Register<IHumanoidRig, VmcHumanoidRig>(Lifetime.Singleton);
            builder.Register<IExpressionRig, VmcExpressionRig>(Lifetime.Singleton);
            builder.Register<ICameraRig, VmcCameraRig>(Lifetime.Singleton);

            builder.Register<AvatarRig>(Lifetime.Singleton);

            builder.Register<IConfigurable, AvatarConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<AvatarLoader>(Lifetime.Singleton);

            builder.RegisterInstance(mainCamera);
            builder.RegisterEntryPoint<Camera.Camera>(Lifetime.Singleton);
        }
    }
}