using Hatbor.VMC;
using UnityEngine;
using VContainer;

namespace Hatbor.Rig.VMC
{
    public sealed class VmcRootTransformRig : IRootTransformRig
    {
        readonly VmcServer vmcServer;

        [Inject]
        public VmcRootTransformRig(VmcServer vmcServer)
        {
            this.vmcServer = vmcServer;
        }

        void IRootTransformRig.Update(Transform rootTransform)
        {
            vmcServer.ProcessRead();
            var rootPose = vmcServer.RootPose;
            rootTransform.position = rootPose.position;
            rootTransform.rotation = rootPose.rotation;
        }
    }
}