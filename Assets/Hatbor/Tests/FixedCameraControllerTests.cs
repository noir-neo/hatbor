using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Hatbor.Camera;
using Hatbor.Config;
using Hatbor.Rig;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniVRM10;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Hatbor.Tests
{
    public sealed class FixedCameraControllerTests
    {
        FixedCameraConfig config;
        AvatarHead head;
        FixedCameraController controller;
        Vrm10Instance instance;

        [SetUp]
        public void SetUp()
        {
            config = new FixedCameraConfig();
            head = new AvatarHead();
            controller = new FixedCameraController(config, new RenderConfig(), head);
            ((IStartable)controller).Start();
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)controller).Dispose();
            if (instance != null) Object.Destroy(instance.gameObject);
        }

        [Test]
        public void ResetKeepsCameraWhenNoAvatarIsBound()
        {
            var position = config.CameraPosition.Value;
            var rotation = config.CameraRotation.Value;

            config.ResetToAvatarHead();

            Assert.That(config.CameraPosition.Value, Is.EqualTo(position));
            Assert.That(config.CameraRotation.Value, Is.EqualTo(rotation));
        }

        [UnityTest]
        public IEnumerator ResetPlacesCameraOneMeterInFrontOfHeadLookingAtIt() => UniTask.ToCoroutine(async () =>
        {
            instance = await TestAvatar.LoadAsync();
            head.Bind(instance);

            config.ResetToAvatarHead();

            var headPosition = instance.Humanoid.Head.position;
            var cameraPosition = config.CameraPosition.Value;
            var cameraForward = Quaternion.Euler(config.CameraRotation.Value) * Vector3.forward;
            Assert.That(Vector3.Distance(cameraPosition, headPosition + Vector3.forward), Is.LessThan(1e-3f));
            Assert.That(Vector3.Angle(cameraForward, headPosition - cameraPosition), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator ResetFollowsAvatarFacingDirection() => UniTask.ToCoroutine(async () =>
        {
            instance = await TestAvatar.LoadAsync();
            head.Bind(instance);
            instance.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            config.ResetToAvatarHead();

            var headPosition = instance.Humanoid.Head.position;
            var cameraPosition = config.CameraPosition.Value;
            var cameraForward = Quaternion.Euler(config.CameraRotation.Value) * Vector3.forward;
            Assert.That(Vector3.Distance(cameraPosition, headPosition + Vector3.right), Is.LessThan(1e-3f));
            Assert.That(Vector3.Angle(cameraForward, Vector3.left), Is.LessThan(0.1f));
        });

        [UnityTest]
        public IEnumerator ResetDoesNothingAfterDispose() => UniTask.ToCoroutine(async () =>
        {
            instance = await TestAvatar.LoadAsync();
            head.Bind(instance);
            var position = config.CameraPosition.Value;

            ((IDisposable)controller).Dispose();
            config.ResetToAvatarHead();

            Assert.That(config.CameraPosition.Value, Is.EqualTo(position));
        });
    }
}
