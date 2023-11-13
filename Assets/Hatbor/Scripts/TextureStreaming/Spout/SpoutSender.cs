using System;
using Klak.Spout;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Hatbor.TextureStreaming.Spout
{
    public sealed class SpoutSender : ITextureSender
    {
        Klak.Spout.SpoutSender sender;
        readonly SpoutResources resources;

        void IDisposable.Dispose()
        {
            if (sender != null)
            {
                Object.Destroy(sender);
                sender = null;
            }
        }

        bool alphaSupport;
        public bool AlphaSupport
        {
            get => alphaSupport;
            set
            {
                alphaSupport = value;
                if (sender != null)
                {
                    sender.keepAlpha = value;
                }
            }
        }

        public bool IsRunning => sender != null && sender.enabled;

        [Inject]
        public SpoutSender(SpoutResources resources)
        {
            this.resources = resources;
        }

        void ITextureSender.StartServer(string name, int width, int height, TextureFormat format)
        {
            if (sender == null)
            {
                sender = new GameObject("SpoutSender").AddComponent<Klak.Spout.SpoutSender>();
                sender.SetResources(resources);
                sender.spoutName = name;
                sender.keepAlpha = AlphaSupport;
                sender.captureMethod = CaptureMethod.Texture;
            }
            sender.enabled = true;
        }

        void ITextureSender.StopServer()
        {
            if (sender != null)
            {
                sender.enabled = false;
            }
        }

        void ITextureSender.PublishTexture(Texture source)
        {
            if (sender != null)
            {
                sender.sourceTexture = source;
            }
        }
    }
}