using System;
using Hatbor.Config;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Camera
{
    public sealed class RenderTextureProvider : IStartable, IDisposable
    {
        const int Depth = 24;
        const RenderTextureFormat Format = RenderTextureFormat.DefaultHDR;

        readonly RenderConfig renderConfig;

        readonly ISubject<Vector2Int> sizeChangedSubject;

        public RenderTexture RenderTexture { get; }
        public IObservable<Vector2Int> OnSizeChanged => sizeChangedSubject;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public RenderTextureProvider(RenderConfig renderConfig)
        {
            this.renderConfig = renderConfig;
            var size = ClampSize(renderConfig.Size.Value);
            RenderTexture = new RenderTexture(size.x, size.y, Depth, Format, 0)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
            };
            RenderTexture.Create();
            sizeChangedSubject = new BehaviorSubject<Vector2Int>(size);
        }

        void IStartable.Start()
        {
            renderConfig.Size
                .Subscribe(Resize)
                .AddTo(disposables);
        }

        void Resize(Vector2Int size)
        {
            var clampedSize = ClampSize(size);
            if (RenderTexture.width == clampedSize.x && RenderTexture.height == clampedSize.y)
            {
                return;
            }
            RenderTexture.Release();
            RenderTexture.width = clampedSize.x;
            RenderTexture.height = clampedSize.y;
            RenderTexture.Create();
            sizeChangedSubject.OnNext(clampedSize);
        }

        static Vector2Int ClampSize(Vector2Int v)
        {
            v.x = Math.Max(v.x, 4);
            v.y = Math.Max(v.y, 4);
            return v;
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}