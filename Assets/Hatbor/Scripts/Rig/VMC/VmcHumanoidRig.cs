using Hatbor.VMC;
using UniHumanoid;
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

        void IHumanoidRig.Update(Vrm10Instance instance)
        {
            vmcServer.ProcessRead();
            Update(instance.Humanoid);
        }

        void Update(Humanoid humanoid)
        {
            var boneLocalPoses = vmcServer.BoneLocalPoses;
            foreach (var (bone, pose) in boneLocalPoses)
            {
                var t = humanoid.GetBoneTransform(bone);
                if (t == null) continue;
                if (bone == HumanBodyBones.Hips)
                {
                    t.SetLocalPositionAndRotation(pose.position, pose.rotation);
                }
                else
                {
                    t.localRotation = pose.rotation;
                }
            }
        }
    }
}