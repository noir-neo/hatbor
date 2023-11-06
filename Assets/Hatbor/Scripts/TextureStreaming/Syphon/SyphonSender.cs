// This code is based on KlakSyphon.
// https://github.com/keijiro/KlakSyphon/blob/master/jp.keijiro.klak.syphon/Runtime/SyphonServer.cs

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hatbor.TextureStreaming.Syphon
{
    public sealed class SyphonSender : ITextureSender
    {
        IntPtr serverInstance;
        Texture serverTexture;
        Material blitMaterial;

        public bool AlphaSupport { get; set; }

        public bool IsRunning => serverInstance != IntPtr.Zero;

        void IDisposable.Dispose()
        {
            StopServer();

            if (blitMaterial != null)
            {
                UnityEngine.Object.Destroy(blitMaterial);
            }
        }

        void ITextureSender.StartServer(string name, int width, int height, TextureFormat format)
        {
            if (serverInstance != IntPtr.Zero || serverTexture != null)
            {
                return;
            }

            ApplyCurrentColorSpace();

            serverInstance = Plugin_CreateServer(name, width, height);
            serverTexture = Texture2D.CreateExternalTexture(
                width, height, format, false, false,
                Plugin_GetServerTexture(serverInstance));

            if (blitMaterial == null)
            {
                blitMaterial = new Material(Shader.Find("Hidden/Klak/Syphon/Blit"))
                {
                    hideFlags = HideFlags.DontSave
                };
            }
        }

        void ITextureSender.PublishTexture(Texture source)
        {
            if (source == null ||
                serverInstance == IntPtr.Zero ||
                serverTexture == null)
            {
                return;
            }

            CopyTexture(source, serverTexture);
            Plugin_PublishServerTexture(serverInstance);
        }

        // NOTE: Graphics.CopyTexture(src, dst) doesn't work.
        void CopyTexture(Texture src, Texture dst)
        {
            var temp = RenderTexture.GetTemporary(
                dst.width, dst.height, 0,
                RenderTextureFormat.Default, RenderTextureReadWrite.Default
            );
            Graphics.Blit(src, temp, blitMaterial, AlphaSupport ? 1 : 0);
            Graphics.CopyTexture(temp, dst);
            RenderTexture.ReleaseTemporary(temp);
        }

        public void StopServer()
        {
            if (serverInstance != IntPtr.Zero)
            {
                Plugin_DestroyServer(serverInstance);
                serverInstance = IntPtr.Zero;
            }

            if (serverTexture != null)
            {
                UnityEngine.Object.Destroy(serverTexture);
                serverTexture = null;
            }
        }

        static void ApplyCurrentColorSpace()
        {
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                Plugin_EnableColorSpaceConversion();
            }
            else
            {
                Plugin_DisableColorSpaceConversion();
            }
        }

        #region Native plugin entry points
        [DllImport("KlakSyphon")]
        static extern void Plugin_EnableColorSpaceConversion();

        [DllImport("KlakSyphon")]
        static extern void Plugin_DisableColorSpaceConversion();

        [DllImport("KlakSyphon")]
        static extern IntPtr Plugin_CreateServer(string name, int width, int height);

        [DllImport("KlakSyphon")]
        static extern void Plugin_DestroyServer(IntPtr instance);

        [DllImport("KlakSyphon")]
        static extern IntPtr Plugin_GetServerTexture(IntPtr instance);

        [DllImport("KlakSyphon")]
        static extern void Plugin_PublishServerTexture(IntPtr instance);
        #endregion
    }
}