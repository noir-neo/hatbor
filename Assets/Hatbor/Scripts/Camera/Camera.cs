using System;
using Hatbor.Rig;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Camera
{
    public sealed class Camera : IStartable, ITickable, IDisposable
    {
        readonly UnityEngine.Camera mainCamera;
        readonly ICameraRig cameraRig;
        readonly RenderTextureProvider renderTextureProvider;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public Camera(UnityEngine.Camera mainCamera,
            ICameraRig cameraRig,
            RenderTextureProvider renderTextureProvider)
        {
            this.mainCamera = mainCamera;
            this.cameraRig = cameraRig;
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
            cameraRig.Update(mainCamera);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}