using System.Linq;
using Hatbor.Config;
using Hatbor.UI;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hatbor.Tests
{
    public sealed class ConfigTests
    {
        [Test]
        public void VmcCameraConfigRotationOffsetDefaultsToZero()
        {
            Assert.That(new VmcCameraConfig().RotationOffset.Value, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void VmcCameraConfigRotationOffsetSurvivesJsonRoundTrip()
        {
            var saved = new VmcCameraConfig();
            saved.RotationOffset.Value = new Vector3(10f, 20f, 90f);

            var loaded = new VmcCameraConfig();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(saved), loaded);

            Assert.That(loaded.RotationOffset.Value, Is.EqualTo(new Vector3(10f, 20f, 90f)));
        }

        [Test]
        public void FixedCameraConfigSurvivesJsonRoundTrip()
        {
            var saved = new FixedCameraConfig();
            saved.Enabled.Value = true;
            saved.CameraPosition.Value = new Vector3(1f, 2f, 3f);

            var loaded = new FixedCameraConfig();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(saved), loaded);

            Assert.That(loaded.Enabled.Value, Is.True);
            Assert.That(loaded.CameraPosition.Value, Is.EqualTo(new Vector3(1f, 2f, 3f)));
        }

        [Test]
        public void FixedCameraConfigResetToAvatarHeadNotifiesOncePerInvoke()
        {
            var config = new FixedCameraConfig();
            var count = 0;
            using var _ = config.ResetToAvatarHeadRequested.Subscribe(_ => count++);

            config.ResetToAvatarHead();
            config.ResetToAvatarHead();

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void ConfigGroupShowsRotationOffsetFieldForVmcCameraConfig()
        {
            var group = new ConfigGroup(null);
            using var _ = group.Bind(new VmcCameraConfig());

            var labels = group.Query<Vector3Field>().ToList().Select(f => f.label);
            Assert.That(labels, Has.Member("Rotation Offset"));
        }

        [Test]
        public void ConfigGroupShowsResetButtonForFixedCameraConfig()
        {
            var group = new ConfigGroup(null);
            using var _ = group.Bind(new FixedCameraConfig());

            var texts = group.Query<Button>().ToList().Select(b => b.text);
            Assert.That(texts, Has.Member("Reset to Avatar Head"));
        }
    }
}
