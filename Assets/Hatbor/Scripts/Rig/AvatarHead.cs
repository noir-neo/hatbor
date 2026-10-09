using UnityEngine;
using UniVRM10;

namespace Hatbor.Rig
{
    public sealed class AvatarHead
    {
        Transform root;
        Transform head;
        Quaternion restRotationInRoot;

        public void Bind(Vrm10Instance instance)
        {
            root = instance.transform;
            head = instance.Humanoid.Head;
            restRotationInRoot = Quaternion.Inverse(root.rotation) * head.rotation;
        }

        public bool TryGetFacingPose(out Pose pose)
        {
            if (head == null)
            {
                pose = default;
                return false;
            }

            var facingRotation = head.rotation * Quaternion.Inverse(restRotationInRoot);
            var horizontalForward = Vector3.ProjectOnPlane(facingRotation * Vector3.forward, Vector3.up);
            if (horizontalForward.sqrMagnitude < Mathf.Epsilon)
            {
                horizontalForward = Vector3.ProjectOnPlane(root.forward, Vector3.up);
            }

            pose = new Pose(head.position, Quaternion.LookRotation(horizontalForward, Vector3.up));
            return true;
        }
    }
}