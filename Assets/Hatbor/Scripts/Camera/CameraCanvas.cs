using System;
using System.Numerics;
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
        readonly RenderTextureProvider renderTextureProvider;
        readonly RawImage rawImage;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public CameraCanvas(RenderTextureProvider renderTextureProvider,
            RawImage rawImage)
        {
            this.renderTextureProvider = renderTextureProvider;
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