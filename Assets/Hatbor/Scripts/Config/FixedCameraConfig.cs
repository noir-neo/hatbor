using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("Camera")]
    public sealed class FixedCameraConfig : IConfigurable
    {
        public string PersistentKey => "HidCameraConfig";

        [SerializeField]
        BoolReactiveProperty enabled = new(false);
        [SerializeField]
        Vector3ReactiveProperty cameraPosition = new (Vector3.zero);
        [SerializeField]
        Vector3ReactiveProperty cameraRotation = new (new Vector3(0f, 180f, 0f));
        [SerializeField]
        FloatReactiveProperty fieldOfView = new (30f);

        [ConfigProperty("Enabled")]
        public ReactiveProperty<bool> Enabled => enabled;
        [ConfigProperty("Position")]
        public ReactiveProperty<Vector3> CameraPosition => cameraPosition;
        [ConfigProperty("Rotation")]
        public ReactiveProperty<Vector3> CameraRotation => cameraRotation;
        [ConfigProperty("Field of View")]
        public ReactiveProperty<float> FieldOfView => fieldOfView;
    }
}