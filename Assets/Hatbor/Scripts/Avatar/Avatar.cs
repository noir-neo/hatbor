using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hatbor.Rig;
using UniVRM10;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Hatbor.Avatar
{
    public sealed class Avatar : IAsyncStartable, ITickable, IDisposable
    {
        readonly string path;
        readonly AvatarRig rig;

        Vrm10Instance instance;

        [Inject]
        public Avatar(string path, AvatarRig rig)
        {
            this.path = path;
            this.rig = rig;
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            instance = await LoadAsync(path, cancellation);
            // Runtime の生成より前に instance.transform を動かすと ControlRig がズレる (UniVRM v0.115.0)
            // ので、とりあえず初期化しておく
            _ = instance.Runtime;
            rig.Update(instance);
        }

        void ITickable.Tick()
        {
            if (instance == null) return;
            rig.Update(instance);
        }

        void IDisposable.Dispose()
        {
            if (instance == null) return;
            Object.Destroy(instance.gameObject);
            instance = null;
        }

        static async UniTask<Vrm10Instance> LoadAsync(string path, CancellationToken ctx)
        {
            var instance = await Vrm10.LoadPathAsync(path,
                materialGenerator: new UrpVrm10MaterialDescriptorGenerator(),
                ct: ctx);
            return instance;
        }
    }
}