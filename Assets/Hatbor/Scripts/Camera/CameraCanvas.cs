using System;
using Hatbor.Config;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using Vector2 = UnityEngine.Vector2;

namespace Hatbor.Camera
{
    public sealed class CameraCanvas : IStartable, IDisposable
    {
        static readonly Rect UVDefault = new(0, 0, 1, 1);
        static readonly Rect UVMirror = new(1, 0, -1, 1);

        readonly RenderTextureProvider renderTextureProvider;
        readonly RenderConfig renderConfig;
        readonly RawImage rawImage;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public CameraCanvas(RenderTextureProvider renderTextureProvider,
            RenderConfig renderConfig,
            RawImage rawImage)
        {
            this.renderTextureProvider = renderTextureProvider;
            this.renderConfig = renderConfig;
            this.rawImage = rawImage;
        }

        void IStartable.Start()
        {
            rawImage.texture = renderTextureProvider.RenderTexture;
            rawImage.SetNativeSize();

            var parent = rawImage.transform.parent as RectTransform;
            parent.ObserveEveryValueChanged(p => p.sizeDelta)
                .CombineLatest(renderTextureProvider.OnSizeChanged,
                    (parentSize, textureSize) => (parentSize, textureSize))
                .Select(t => CalcCoveredSize(t.parentSize, t.textureSize))
                .Subscribe(size => rawImage.rectTransform.sizeDelta = size)
                .AddTo(disposables);

            renderConfig.MirrorPreview
                .Select(b => b ? UVMirror : UVDefault)
                .Subscribe(uv => rawImage.uvRect = uv)
                .AddTo(disposables);
        }

        static Vector2 CalcCoveredSize(Vector2 parentSize, Vector2 textureSize)
        {
            var parentAspect = parentSize.x / parentSize.y;
            var aspect = textureSize.x / textureSize.y;
            return aspect < parentAspect
                ? new Vector2(parentSize.x, parentSize.x / aspect)
                : new Vector2(parentSize.y * aspect, parentSize.y);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}