using System;
using Klak.Spout;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hatbor.TextureStreaming.Spout
{
    public sealed class SpoutSender : ITextureSender
    {
        Klak.Spout.SpoutSender sender;
        SpoutResources resources;

        void IDisposable.Dispose()
        {
            if (sender != null)
            {
                Object.Destroy(sender);
                sender = null;
            }
            if (resources != null)
            {
                Object.Destroy(resources);
                resources = null;
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

        void ITextureSender.StartServer(string name, int width, int height, TextureFormat format)
        {
            if (resources == null)
            {
                resources = ScriptableObject.CreateInstance<SpoutResources>();
                resources.hideFlags = HideFlags.DontSave;
                resources.blitShader = Shader.Find("Hidden/Klak/Spout/Blit");
            }

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