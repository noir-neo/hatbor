using System;
using Hatbor.Config;
using Hatbor.Rig;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;
using Input = Hatbor.HID.Input;

namespace Hatbor.Camera
{
    public sealed class FixedCameraController : IStartable, IDisposable, ITickable
    {
        const float ResetDistanceFromHead = 1f;

        readonly FixedCameraConfig config;
        readonly RenderConfig renderConfig;
        readonly AvatarHead avatarHead;

        readonly Input input = new();
        readonly CompositeDisposable disposables = new();

        [Inject]
        public FixedCameraController(FixedCameraConfig config,
            RenderConfig renderConfig,
            AvatarHead avatarHead)
        {
            this.config = config;
            this.renderConfig = renderConfig;
            this.avatarHead = avatarHead;
        }

        void IStartable.Start()
        {
            input.Enable();

            config.ResetToAvatarHeadRequested
                .Subscribe(_ => ResetToAvatarHead())
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
            input.Dispose();
        }

        void ITickable.Tick()
        {
            if (!config.Enabled.Value ||
                EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }

            var rot = Quaternion.Euler(config.CameraRotation.Value);
            var mirror = renderConfig.MirrorPreview.Value ? -1f : 1f;

            var moveKeyboard = input.Camera.MoveKeyboard.ReadValue<Vector3>() * Time.fixedDeltaTime;
            var movePointer = input.Camera.MovePointer.ReadValue<Vector2>();
            var moveWheel = input.Camera.MoveWheel.ReadValue<float>();
            var move = new Vector3((moveKeyboard.x + movePointer.x) * mirror, moveKeyboard.y + movePointer.y, moveKeyboard.z + moveWheel);
            config.CameraPosition.Value += rot * move;

            var look = input.Camera.LookPointer.ReadValue<Vector2>();
            config.CameraRotation.Value += Quaternion.Euler(0, 0, rot.eulerAngles.z) * new Vector3(-look.y, look.x * mirror, 0f);
        }

        void ResetToAvatarHead()
        {
            if (!avatarHead.TryGetFacingPose(out var headPose)) return;

            var headForward = headPose.rotation * Vector3.forward;
            config.CameraPosition.Value = headPose.position + headForward * ResetDistanceFromHead;
            config.CameraRotation.Value = Quaternion.LookRotation(-headForward, Vector3.up).eulerAngles;
        }
    }
}