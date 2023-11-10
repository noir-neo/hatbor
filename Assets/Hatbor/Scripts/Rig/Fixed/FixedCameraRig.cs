using Hatbor.Config;
using UnityEngine;
using VContainer;

namespace Hatbor.Rig.Fixed
{
    public sealed class FixedCameraRig : ICameraRig
    {
        readonly FixedCameraConfig config;

        public bool Enabled => config.Enabled.Value;
        public int Order => 1; // after VmcCameraRig

        [Inject]
        public FixedCameraRig(FixedCameraConfig config)
        {
            this.config = config;
        }

        void ICameraRig.Update(Camera camera)
        {
            var transform = camera.transform;
            transform.position = config.CameraPosition.Value;
            transform.rotation = Quaternion.Euler(config.CameraRotation.Value);
            camera.fieldOfView = config.FieldOfView.Value;
        }
    }
}