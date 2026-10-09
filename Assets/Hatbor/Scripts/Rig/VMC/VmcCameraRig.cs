using Hatbor.Config;
using Hatbor.VMC;
using UnityEngine;
using VContainer;

namespace Hatbor.Rig.VMC
{
    public sealed class VmcCameraRig : ICameraRig
    {
        readonly VmcServer vmcServer;
        readonly VmcCameraConfig config;

        // TODO: Make configurable
        public bool Enabled => true;
        public int Order => 0;

        [Inject]
        public VmcCameraRig(VmcServer vmcServer, VmcCameraConfig config)
        {
            this.vmcServer = vmcServer;
            this.config = config;
        }

        void ICameraRig.Update(Camera camera)
        {
            vmcServer.ProcessRead();

            var cameraPose = vmcServer.CameraPose;
            var cameraFov = vmcServer.CameraFov;
            var cameraTransform = camera.transform;
            cameraTransform.position = cameraPose.position;
            cameraTransform.rotation = cameraPose.rotation * Quaternion.Euler(config.RotationOffset.Value);
            camera.fieldOfView = cameraFov;
        }
    }
}