using System;
using System.Text;
using Hatbor.Config;
using Hatbor.VMC;
using UniRx;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.UI
{
    public class VmcDebugRoot : IStartable, ITickable, IDisposable
    {
        readonly UIDocument uiDocument;
        readonly VmcServerConfig config;
        readonly VmcServer server;

        readonly CompositeDisposable disposables = new();

        Label label;

        [Inject]
        public VmcDebugRoot(UIDocument uiDocument,
            VmcServerConfig config,
            VmcServer server)
        {
            this.uiDocument = uiDocument;
            this.config = config;
            this.server = server;
        }

        void IStartable.Start()
        {
            config.DebugEnabled
                .Subscribe(enabled =>
                {
                    uiDocument.enabled = enabled;
                    if (!enabled) return;
                    var root = uiDocument.rootVisualElement;
                    label = root.Q<Label>("vmc-debug-label");
                })
                .AddTo(disposables);
        }

        void ITickable.Tick()
        {
            if (label == null) return;
            var text = CreateText();
            label.text = text;
        }

        string CreateText()
        {
            var builder = new StringBuilder();
            builder.AppendLine("VMC Server Debug");
            builder.AppendLine($"Available: {server.IsAvailable}");
            builder.AppendLine($"Last Available Received Time: {server.LastAvailableReceivedTime:F2}");
            builder.AppendLine($"Root Pose: {server.RootPose}");
            builder.AppendLine($"Camera Pose: {server.CameraPose}");
            builder.AppendLine($"Camera FOV: {server.CameraFov:F2}");
            builder.AppendLine("Bone Local Poses:");
            foreach (var kvp in server.BoneLocalPoses)
            {
                builder.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
            builder.AppendLine("Blend Shape Values:");
            foreach (var kvp in server.BlendShapeValues)
            {
                builder.Append($"{kvp.Key}: {kvp.Value:F2}, ");
            }
            return builder.ToString();
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}