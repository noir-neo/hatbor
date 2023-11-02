using Hatbor.Avatar;
using Hatbor.Rig;
using Hatbor.Rig.VMC;
using Hatbor.VMC;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class MainLifetimeScope : VContainer.Unity.LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VmcServerSettings>(Lifetime.Singleton);
            builder.RegisterEntryPoint<VmcServer>(Lifetime.Singleton).AsSelf();

            builder.Register<IRootTransformRig, VmcRootTransformRig>(Lifetime.Singleton);
            builder.Register<IHumanoidRig, VmcHumanoidRig>(Lifetime.Singleton);
            builder.Register<IExpressionRig, VmcExpressionRig>(Lifetime.Singleton);

            builder.Register<AvatarRig>(Lifetime.Singleton);

            builder.Register<AvatarSettings>(Lifetime.Singleton);
            builder.RegisterEntryPoint<AvatarLoader>(Lifetime.Singleton);
        }
    }
}