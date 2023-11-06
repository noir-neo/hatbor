using System;
using UnityEngine;

namespace Hatbor.TextureStreaming
{
    public interface ITextureSender : IDisposable
    {
        public bool AlphaSupport { get; set; }
        public bool IsRunning { get; }

        public void StartServer(string name, int width, int height, TextureFormat format = TextureFormat.RGBA64);
        public void StopServer();
        public void PublishTexture(Texture source);
    }
}