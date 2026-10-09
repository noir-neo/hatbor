using Cysharp.Threading.Tasks;
using UnityEngine;
using UniVRM10;

namespace Hatbor.Tests
{
    static class TestAvatar
    {
        static readonly string Path = System.IO.Path.Combine(Application.streamingAssetsPath, "avatar.vrm");

        public static async UniTask<Vrm10Instance> LoadAsync()
        {
            return await Vrm10.LoadPathAsync(Path,
                controlRigGenerationOption: ControlRigGenerationOption.None,
                materialGenerator: new UrpVrm10MaterialDescriptorGenerator());
        }
    }
}
