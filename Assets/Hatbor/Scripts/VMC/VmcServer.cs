using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using uOSC;
using VContainer;
using VContainer.Unity;
using Enum = System.Enum;

namespace Hatbor.VMC
{
    public sealed class VmcServer : IStartable, IDisposable
    {
        readonly VmcServerSettings settings;

        readonly OscServer server = new();
        readonly CompositeDisposable disposables = new();

        public float LastAvailableReceivedTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public Pose RootPose { get; private set; }

        readonly Dictionary<HumanBodyBones, Pose> boneLocalPoses = new();
        public IReadOnlyDictionary<HumanBodyBones, Pose> BoneLocalPoses => boneLocalPoses;

        readonly Dictionary<string, float> blendShapeValuesTemp = new();
        public IReadOnlyDictionary<string, float> BlendShapeValues { get; private set; } = new Dictionary<string, float>();

        public Pose CameraPose { get; private set; }
        public float CameraFov { get; private set; }

        [Inject]
        public VmcServer(VmcServerSettings settings)
        {
            this.settings = settings;
        }

        void IStartable.Start()
        {
            settings.Enabled
                .CombineLatest(settings.Port, (enabled, port) => (enabled, port))
                .Subscribe(t => {
                    if (t.enabled)
                    {
                        StopServer();
                        StartServer(t.port);
                    }
                    else
                    {
                        StopServer();
                    }
                })
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        void StartServer(int port)
        {
            server.StartServer(port);
        }

        void StopServer()
        {
            server.StopServer();
        }

        public void ProcessRead()
        {
            while (server.MessageCount > 0)
            {
                var message = server.Dequeue();
                OnRead(message);
            }
        }

        void OnRead(Message message)
        {
            switch (message.address)
            {
                case "/VMC/Ext/OK":
                {
                    IsAvailable = (int)message.values[0] == 1;
                    if (IsAvailable)
                    {
                        LastAvailableReceivedTime = Time.time;
                    }
                    break;
                }
                case "/VMC/Ext/Root/Pos":
                {
                    var pos = new Vector3((float)message.values[1], (float)message.values[2], (float)message.values[3]);
                    var rot = new Quaternion((float)message.values[4], (float)message.values[5], (float)message.values[6], (float)message.values[7]);
                    RootPose = new Pose(pos, rot);
                    break;
                }
                case "/VMC/Ext/Bone/Pos":
                {
                    if (Enum.TryParse((string)message.values[0], out HumanBodyBones bone))
                    {
                        if (bone != HumanBodyBones.LastBone)
                        {
                            var pos = new Vector3((float)message.values[1], (float)message.values[2], (float)message.values[3]);
                            var rot = new Quaternion((float)message.values[4], (float)message.values[5], (float)message.values[6], (float)message.values[7]);
                            boneLocalPoses[bone] = new Pose(pos, rot);
                        }
                    }
                    break;
                }
                case "/VMC/Ext/Blend/Val":
                {
                    var blendName = (string)message.values[0];
                    var blendValue = (float)message.values[1];
                    blendShapeValuesTemp[blendName] = blendValue;
                    break;
                }
                case "/VMC/Ext/Blend/Apply":
                {
                    BlendShapeValues = blendShapeValuesTemp.ToDictionary(
                        x => x.Key,
                        x => x.Value);
                    blendShapeValuesTemp.Clear();
                    break;
                }
                case "/VMC/Ext/Cam":
                {
                    var pos = new Vector3((float)message.values[1], (float)message.values[2], (float)message.values[3]);
                    var rot = new Quaternion((float)message.values[4], (float)message.values[5], (float)message.values[6], (float)message.values[7]);
                    CameraPose = new Pose(pos, rot);
                    CameraFov = (float)message.values[8];
                    break;
                }
            }
        }
    }
}