using System;
using System.Collections.Generic;
using System.Linq;
using Hatbor.Rig;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Camera
{
    public sealed class Camera : IStartable, ITickable, IDisposable
    {
        readonly UnityEngine.Camera mainCamera;
        readonly IEnumerable<ICameraRig> cameraRigs;
        readonly RenderTextureProvider renderTextureProvider;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public Camera(UnityEngine.Camera mainCamera,
            IEnumerable<ICameraRig> cameraRigs,
            RenderTextureProvider renderTextureProvider)
        {
            this.mainCamera = mainCamera;
            this.cameraRigs = cameraRigs;
            this.renderTextureProvider = renderTextureProvider;
        }

        void IStartable.Start()
        {
            mainCamera.targetTexture = renderTextureProvider.RenderTexture;

            renderTextureProvider.OnSizeChanged
                .Subscribe(_ => mainCamera.ResetAspect())
                .AddTo(disposables);
        }

        void ITickable.Tick()
        {
            foreach (var cameraRig in cameraRigs
                         .Where(c => c.Enabled)
                         .OrderBy(c => c.Order))
            {
                cameraRig.Update(mainCamera);
            }
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}