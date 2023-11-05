using System;
using Hatbor.Config;
using Hatbor.Rig;
using UniRx;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Camera
{
    public sealed class Camera : IStartable, ITickable, IDisposable
    {
        readonly UnityEngine.Camera mainCamera;
        readonly UniversalAdditionalCameraData cameraData;
        readonly ICameraRig cameraRig;
        readonly RenderTextureProvider renderTextureProvider;
        readonly RenderConfig renderConfig;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public Camera(UnityEngine.Camera mainCamera,
            ICameraRig cameraRig,
            RenderTextureProvider renderTextureProvider,
            RenderConfig renderConfig)
        {
            this.mainCamera = mainCamera;
            this.cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            this.cameraRig = cameraRig;
            this.renderTextureProvider = renderTextureProvider;
            this.renderConfig = renderConfig;
        }

        void IStartable.Start()
        {
            mainCamera.targetTexture = renderTextureProvider.RenderTexture;

            renderTextureProvider.OnSizeChanged
                .Subscribe(_ => mainCamera.ResetAspect())
                .AddTo(disposables);

            // TODO: support transparent with post-processing
            renderConfig.TransparentBackground
                .Subscribe(b => cameraData.renderPostProcessing = !b)
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