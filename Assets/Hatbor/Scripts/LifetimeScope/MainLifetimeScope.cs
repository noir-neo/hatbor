using Hatbor.Avatar;
using Hatbor.Rig;
using Hatbor.Rig.VMC;
using Hatbor.Config;
using Hatbor.Rig.Fixed;
using Hatbor.PerformanceProfiler;
using Hatbor.VMC;
using VContainer;
using VContainer.Unity;

namespace Hatbor.LifetimeScope
{
    public sealed class MainLifetimeScope : VContainer.Unity.LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Config
            builder.RegisterEntryPoint<ConfigStore>();
            builder.Register<IConfigurable, VmcServerConfig>(Lifetime.Singleton).AsSelf();
            builder.Register<IConfigurable, FixedCameraConfig>(Lifetime.Singleton).AsSelf();
            builder.Register<IConfigurable, AvatarConfig>(Lifetime.Singleton).AsSelf();
            builder.Register<IConfigurable, RenderConfig>(Lifetime.Singleton).AsSelf();
            builder.Register<IConfigurable, LightConfig>(Lifetime.Singleton).AsSelf();
            builder.Register<IConfigurable, MiscConfig>(Lifetime.Singleton).AsSelf();

            // PerformanceProfiler
            builder.RegisterEntryPoint<PerformanceProfilerTicker>();
            builder.Register<IProfilerRecorder, FrameRateProfilerRecorder>(Lifetime.Singleton).AsSelf();
            builder.Register<IProfilerRecorder, VmcServerProfilerRecorder>(Lifetime.Singleton).AsSelf();

            // Rig/VMC
            builder.RegisterEntryPoint<VmcServer>(Lifetime.Singleton).AsSelf();
            builder.Register<IRootTransformRig, VmcRootTransformRig>(Lifetime.Singleton);
            builder.Register<IHumanoidRig, VmcHumanoidRig>(Lifetime.Singleton);
            builder.Register<IExpressionRig, VmcExpressionRig>(Lifetime.Singleton);
            builder.Register<ICameraRig, VmcCameraRig>(Lifetime.Singleton);

            // Rig/Fixed
            builder.Register<ICameraRig, FixedCameraRig>(Lifetime.Singleton);

            // Avatar
            builder.Register<AvatarRig>(Lifetime.Singleton);
            builder.RegisterEntryPoint<AvatarLoader>(Lifetime.Singleton);
        }
    }
}