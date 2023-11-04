using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("Rendering")]
    public sealed class RenderConfig : IConfigurable
    {
        public string PersistentKey => "RenderConfig";

        [SerializeField] Vector2IntReactiveProperty size = new(new Vector2Int(1920, 1080));
        [SerializeField] BoolReactiveProperty enabledSharingTexture = new(true);
        [SerializeField] BoolReactiveProperty transparentBackground = new(true);

        [ConfigProperty("Size")]
        public ReactiveProperty<Vector2Int> Size => size;
#if UNITY_STANDALONE_OSX
        [ConfigProperty("Syphon")]
#elif UNITY_STANDALONE_WIN
        [ConfigProperty("Spout2")]
#endif
        public ReactiveProperty<bool> EnabledSharingTexture => enabledSharingTexture;
        [ConfigProperty("Transparent Background")]
        public ReactiveProperty<bool> TransparentBackground => transparentBackground;
    }
}