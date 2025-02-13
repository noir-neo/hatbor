using Hatbor.VMC;
using UnityEngine;
using UniVRM10;
using VContainer;

namespace Hatbor.Rig.VMC
{
    public sealed class VmcHumanoidRig : IHumanoidRig
    {
        readonly VmcServer vmcServer;

        [Inject]
        public VmcHumanoidRig(VmcServer vmcServer)
        {
            this.vmcServer = vmcServer;
        }

        void IHumanoidRig.Update(Vrm10RuntimeControlRig controlRig)
        {
            vmcServer.ProcessRead();
            Update(controlRig);
        }

        void Update(INormalizedPoseApplicable normalizedPoseApplicable)
        {
            var boneLocalPoses = vmcServer.BoneLocalPoses;
            normalizedPoseApplicable.SetRawHipsPosition(boneLocalPoses[HumanBodyBones.Hips].position);
            foreach (var t in boneLocalPoses)
            {
                var (bone, pose) = t;
                normalizedPoseApplicable.SetNormalizedLocalRotation(bone, pose.rotation);
            }
        }
    }
}