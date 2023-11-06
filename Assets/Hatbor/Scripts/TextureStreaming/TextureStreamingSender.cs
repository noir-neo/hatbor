using System;
using Hatbor.Camera;
using Hatbor.Config;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.TextureStreaming
{
    public sealed class TextureStreamingSender : IStartable, IDisposable, ITickable
    {
        const string ServerName = "Hatbor";

        readonly RenderConfig renderConfig;
        readonly RenderTextureProvider renderTextureProvider;

        readonly ITextureSender sender;
        readonly CompositeDisposable disposables = new();

        [Inject]
        public TextureStreamingSender(ITextureSender sender,
            RenderConfig renderConfig,
            RenderTextureProvider renderTextureProvider)
        {
            this.sender = sender;
            this.renderConfig = renderConfig;
            this.renderTextureProvider = renderTextureProvider;
        }

        void IStartable.Start()
        {
            renderConfig.EnabledSharingTexture
                .CombineLatest(renderTextureProvider.OnSizeChanged,
                    (enabled, size) => (enabled, size))
                .Subscribe(t =>
                {
                    var (enabled, size) = t;
                    if (enabled)
                    {
                        if (sender.IsRunning)
                        {
                            sender.StopServer();
                        }

                        sender.StartServer(ServerName, size.x, size.y, TextureFormat.RGBA64);
                    }
                    else
                    {
                        sender.StopServer();
                    }
                })
                .AddTo(disposables);

            renderConfig.TransparentBackground
                .Subscribe(b => sender.AlphaSupport = b)
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            sender.Dispose();
            disposables.Dispose();
        }

        void ITickable.Tick()
        {
            if (!sender.IsRunning)
            {
                return;
            }
            sender.PublishTexture(renderTextureProvider.RenderTexture);
        }
    }
}