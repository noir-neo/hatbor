using System.Collections;
using Cysharp.Threading.Tasks;
using Hatbor.Rig;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniVRM10;
using Object = UnityEngine.Object;

namespace Hatbor.Tests
{
    public sealed class AvatarHeadTests
    {
        Vrm10Instance instance;

        [TearDown]
        public void TearDown()
        {
            if (instance != null) Object.Destroy(instance.gameObject);
        }

        [Test]
        public void HasNoFacingPoseBeforeBind()
        {
            Assert.That(new AvatarHead().TryGetFacingPose(out _), Is.False);
        }

        [UnityTest]
        public IEnumerator FacesAvatarForwardAtHeadPositionInRestPose() => UniTask.ToCoroutine(async () =>
        {
            var head = await LoadAndBindAsync();

            Assert.That(head.TryGetFacingPose(out var pose), Is.True);
            Assert.That(Vector3.Distance(pose.position, instance.Humanoid.Head.position), Is.LessThan(1e-4f));
            Assert.That(Vector3.Angle(pose.forward, Vector3.forward), Is.LessThan(0.1f));
            Assert.That(Vector3.Angle(pose.up, Vector3.up), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator FollowsAvatarRootYaw() => UniTask.ToCoroutine(async () =>
        {
            var head = await LoadAndBindAsync();

            instance.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            Assert.That(head.TryGetFacingPose(out var pose), Is.True);
            Assert.That(Vector3.Angle(pose.forward, Vector3.right), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator FollowsHeadYaw() => UniTask.ToCoroutine(async () =>
        {
            var head = await LoadAndBindAsync();

            var headBone = instance.Humanoid.Head;
            headBone.rotation = Quaternion.AngleAxis(45f, Vector3.up) * headBone.rotation;

            Assert.That(head.TryGetFacingPose(out var pose), Is.True);
            Assert.That(Vector3.Angle(pose.forward, Quaternion.Euler(0f, 45f, 0f) * Vector3.forward), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator StaysLevelWhenHeadPitchesAndRolls() => UniTask.ToCoroutine(async () =>
        {
            var head = await LoadAndBindAsync();

            var headBone = instance.Humanoid.Head;
            headBone.rotation = Quaternion.Euler(30f, 0f, 20f) * headBone.rotation;

            Assert.That(head.TryGetFacingPose(out var pose), Is.True);
            Assert.That(Vector3.Angle(pose.forward, Vector3.forward), Is.LessThan(0.1f));
            Assert.That(Vector3.Angle(pose.up, Vector3.up), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator HasNoFacingPoseAfterAvatarIsDestroyed() => UniTask.ToCoroutine(async () =>
        {
            var head = await LoadAndBindAsync();

            Object.DestroyImmediate(instance.gameObject);

            Assert.That(head.TryGetFacingPose(out _), Is.False);
        });

        async UniTask<AvatarHead> LoadAndBindAsync()
        {
            instance = await TestAvatar.LoadAsync();
            var head = new AvatarHead();
            head.Bind(instance);
            return head;
        }
    }
}
