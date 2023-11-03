using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Avatar
{
    [Serializable]
    public sealed class AvatarSettings
    {
        static readonly string DefaultPath = System.IO.Path.Combine(Application.streamingAssetsPath, "avatar.vrm");

        [SerializeField] StringReactiveProperty path = new(DefaultPath);
        public ReactiveProperty<string> Path => path;
    }
}