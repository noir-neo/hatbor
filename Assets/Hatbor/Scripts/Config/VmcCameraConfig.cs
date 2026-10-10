using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("VmcCamera")]
    public sealed class VmcCameraConfig : IConfigurable
    {
        public string PersistentKey => "VmcCameraConfig";

        [SerializeField]
        Vector3ReactiveProperty rotationOffset = new (Vector3.zero);

        [ConfigProperty("Rotation Offset")]
        public ReactiveProperty<Vector3> RotationOffset => rotationOffset;
    }
}