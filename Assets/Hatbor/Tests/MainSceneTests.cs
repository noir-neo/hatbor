using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hatbor.Config;
using Hatbor.LifetimeScope;
using Hatbor.Rig;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using VContainer;
using Object = UnityEngine.Object;

namespace Hatbor.Tests
{
    [UnityPlatform(RuntimePlatform.OSXEditor, RuntimePlatform.WindowsEditor)]
    public sealed class MainSceneTests
    {
        const string SceneName = "Main";
        const float TimeoutSeconds = 60f;
        const float SentFieldOfView = 45f;
        static readonly Pose SentPose = new(new Vector3(1f, 2f, 3f), Quaternion.Euler(10f, 20f, 30f));

        readonly Dictionary<string, string> savedPlayerPrefs = new();

        IObjectResolver main;
        UnityEngine.Camera mainCamera;
        AvatarHead avatarHead;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

            main = Object.FindFirstObjectByType<MainLifetimeScope>().Container;
            mainCamera = Object.FindFirstObjectByType<CameraLifetimeScope>().Container.Resolve<UnityEngine.Camera>();
            avatarHead = main.Resolve<AvatarHead>();
            SavePlayerPrefs();

            yield return WaitUntil(() => avatarHead.TryGetFacingPose(out _), "Avatar was not loaded");
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return SceneManager.UnloadSceneAsync(SceneName);
            RestorePlayerPrefs();
        }

        [UnityTest]
        public IEnumerator ResetButtonMovesFixedCameraInFrontOfAvatarHead()
        {
            main.Resolve<FixedCameraConfig>().Enabled.Value = true;

            Click(FindConfigElement<Button>(b => b.text == "Reset to Avatar Head"));
            yield return null;
            yield return null;

            Assert.That(avatarHead.TryGetFacingPose(out var headPose), Is.True);
            var cameraTransform = mainCamera.transform;
            Assert.That(Vector3.Distance(cameraTransform.position, headPose.position + headPose.forward), Is.LessThan(1e-3f));
            Assert.That(Vector3.Angle(cameraTransform.forward, -headPose.forward), Is.LessThan(0.1f));
        }

        [UnityTest]
        public IEnumerator VmcCameraIsAppliedWithRotationOffset()
        {
            Assert.That(FindConfigElement<Vector3Field>(f => f.label == "Rotation Offset"), Is.Not.Null);

            var port = VmcCameraSender.FindFreePort();
            main.Resolve<FixedCameraConfig>().Enabled.Value = false;
            main.Resolve<VmcServerConfig>().Enabled.Value = true;
            main.Resolve<VmcServerConfig>().Port.Value = port;
            main.Resolve<VmcCameraConfig>().RotationOffset.Value = new Vector3(0f, 0f, 90f);

            yield return WaitUntil(() =>
            {
                VmcCameraSender.Send(port, SentPose, SentFieldOfView);
                return Mathf.Approximately(mainCamera.fieldOfView, SentFieldOfView);
            }, "VMC camera message was not applied");

            var cameraTransform = mainCamera.transform;
            Assert.That(Vector3.Distance(cameraTransform.position, SentPose.position), Is.LessThan(1e-4f));
            Assert.That(Vector3.Angle(cameraTransform.forward, SentPose.forward), Is.LessThan(0.01f));
            Assert.That(Vector3.Angle(cameraTransform.up, -SentPose.right), Is.LessThan(0.01f));
        }

        static T FindConfigElement<T>(Func<T, bool> predicate) where T : VisualElement
        {
            return Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None)
                .Where(d => d.rootVisualElement != null)
                .SelectMany(d => d.rootVisualElement.Query<T>().ToList())
                .FirstOrDefault(predicate);
        }

        static void Click(Button button)
        {
            Assert.That(button, Is.Not.Null);
            using var submit = NavigationSubmitEvent.GetPooled();
            submit.target = button;
            button.SendEvent(submit);
        }

        static IEnumerator WaitUntil(Func<bool> condition, string timeoutMessage)
        {
            var deadline = Time.realtimeSinceStartup + TimeoutSeconds;
            while (!condition())
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(deadline), timeoutMessage);
                yield return null;
            }
        }

        void SavePlayerPrefs()
        {
            savedPlayerPrefs.Clear();
            foreach (var key in PersistentKeys())
            {
                savedPlayerPrefs[key] = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : null;
            }
        }

        void RestorePlayerPrefs()
        {
            foreach (var (key, value) in savedPlayerPrefs)
            {
                if (value == null)
                {
                    PlayerPrefs.DeleteKey(key);
                }
                else
                {
                    PlayerPrefs.SetString(key, value);
                }
            }
            PlayerPrefs.Save();
        }

        IEnumerable<string> PersistentKeys()
        {
            return main.Resolve<IEnumerable<IConfigurable>>()
                .Select(c => c.PersistentKey)
                .Where(k => !string.IsNullOrEmpty(k));
        }
    }
}
