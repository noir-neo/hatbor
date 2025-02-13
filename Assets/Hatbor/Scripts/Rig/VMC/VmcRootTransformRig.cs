using Hatbor.VMC;
using UnityEngine;
using UniVRM10;
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

        void IRootTransformRig.Update(Vrm10Instance instance)
        {
            vmcServer.ProcessRead();
            var rootPose = vmcServer.RootPose;
            instance.transform.SetLocalPositionAndRotation(rootPose.position, rootPose.rotation);
        }
    }
}