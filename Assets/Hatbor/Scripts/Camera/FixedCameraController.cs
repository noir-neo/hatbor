using System;
using Hatbor.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;
using Input = Hatbor.HID.Input;

namespace Hatbor.Camera
{
    public sealed class FixedCameraController : IStartable, IDisposable, ITickable
    {
        readonly FixedCameraConfig config;
        readonly RenderConfig renderConfig;

        readonly Input input = new();

        [Inject]
        public FixedCameraController(FixedCameraConfig config,
            RenderConfig renderConfig)
        {
            this.config = config;
            this.renderConfig = renderConfig;
        }

        void IStartable.Start()
        {
            input.Enable();
        }

        void IDisposable.Dispose()
        {
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
    }
}