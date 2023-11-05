using Hatbor.Avatar;
using Hatbor.Camera;
using Hatbor.Rig;
using Hatbor.Rig.VMC;
using Hatbor.Config;
using Hatbor.VMC;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class MainLifetimeScope : VContainer.Unity.LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ConfigStore>();
            builder.Register<IConfigurable, VmcServerConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<VmcServer>(Lifetime.Singleton).AsSelf();

            builder.Register<IRootTransformRig, VmcRootTransformRig>(Lifetime.Singleton);
            builder.Register<IHumanoidRig, VmcHumanoidRig>(Lifetime.Singleton);
            builder.Register<IExpressionRig, VmcExpressionRig>(Lifetime.Singleton);
            builder.Register<ICameraRig, VmcCameraRig>(Lifetime.Singleton);

            builder.Register<AvatarRig>(Lifetime.Singleton);

            builder.Register<IConfigurable, AvatarConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<AvatarLoader>(Lifetime.Singleton);

            builder.Register<IConfigurable, RenderConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<RenderTextureProvider>(Lifetime.Singleton).AsSelf();
        }
    }
}