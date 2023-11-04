using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("Avatar")]
    public sealed class AvatarConfig : IConfigurable
    {
        static readonly string DefaultPath = System.IO.Path.Combine(Application.streamingAssetsPath, "avatar.vrm");

        [SerializeField] StringReactiveProperty path = new(DefaultPath);

        [FilePathConfigProperty("Choose Avatar", "vrm")]
        public ReactiveProperty<string> Path => path;
    }
}