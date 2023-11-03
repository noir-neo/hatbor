using Hatbor.Rig;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Camera
{
    public sealed class Camera : ITickable
    {
        readonly UnityEngine.Camera camera;
        readonly ICameraRig cameraRig;

        [Inject]
        public Camera(UnityEngine.Camera camera, ICameraRig cameraRig)
        {
            this.camera = camera;
            this.cameraRig = cameraRig;
        }


        void ITickable.Tick()
        {
            cameraRig.Update(camera);
        }
    }
}