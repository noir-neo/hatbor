using System;
using System.Collections;
using Hatbor.Config;
using Hatbor.PerformanceProfiler;
using Hatbor.Rig;
using Hatbor.Rig.VMC;
using Hatbor.VMC;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Hatbor.Tests
{
    public sealed class VmcCameraRigTests
    {
        const float TimeoutSeconds = 10f;
        const float SentFieldOfView = 45f;
        static readonly Pose SentPose = new(new Vector3(1f, 2f, 3f), Quaternion.Euler(10f, 20f, 30f));

        int port;
        VmcServer server;
        VmcCameraConfig config;
        ICameraRig rig;
        UnityEngine.Camera camera;

        [SetUp]
        public void SetUp()
        {
            port = VmcCameraSender.FindFreePort();
            var serverConfig = new VmcServerConfig();
            serverConfig.Port.Value = port;
            server = new VmcServer(serverConfig, new VmcServerProfilerRecorder());
            ((IStartable)server).Start();

            config = new VmcCameraConfig();
            rig = new VmcCameraRig(server, config);
            camera = new GameObject(nameof(VmcCameraRigTests)).AddComponent<UnityEngine.Camera>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(camera.gameObject);
            ((IDisposable)server).Dispose();
        }

        [UnityTest]
        public IEnumerator AppliesReceivedPoseAsIsWithoutRotationOffset()
        {
            yield return ReceiveSentCamera();

            Assert.That(Vector3.Distance(camera.transform.position, SentPose.position), Is.LessThan(1e-4f));
            Assert.That(Quaternion.Angle(camera.transform.rotation, SentPose.rotation), Is.LessThan(0.01f));
        }

        [UnityTest]
        public IEnumerator AppliesRotationOffsetInCameraLocalSpace()
        {
            config.RotationOffset.Value = new Vector3(0f, 0f, 90f);

            yield return ReceiveSentCamera();

            var cameraTransform = camera.transform;
            Assert.That(Vector3.Distance(cameraTransform.position, SentPose.position), Is.LessThan(1e-4f));
            Assert.That(Vector3.Angle(cameraTransform.forward, SentPose.forward), Is.LessThan(0.01f));
            Assert.That(Vector3.Angle(cameraTransform.up, -SentPose.right), Is.LessThan(0.01f));
        }

        IEnumerator ReceiveSentCamera()
        {
            var deadline = Time.realtimeSinceStartup + TimeoutSeconds;
            while (!Mathf.Approximately(camera.fieldOfView, SentFieldOfView))
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(deadline), "VMC camera message was not received");
                VmcCameraSender.Send(port, SentPose, SentFieldOfView);
                yield return null;
                rig.Update(camera);
            }
        }
    }
}
