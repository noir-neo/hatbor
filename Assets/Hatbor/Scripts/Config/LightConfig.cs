using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("Lighting")]
    public sealed class LightConfig : IConfigurable
    {
        public string PersistentKey => "LightConfig";

        [SerializeField] Vector3ReactiveProperty direction = new(new Vector3(50, 180, 0));
        [SerializeField] ColorReactiveProperty color = new(UnityEngine.Color.white);
        [SerializeField] FloatReactiveProperty colorTemperature = new(5000f);
        [SerializeField] FloatReactiveProperty intensity = new(2f);
        [SerializeField] FloatReactiveProperty bounceIntensity = new(1f);

        [ConfigProperty("Direction")]
        public ReactiveProperty<Vector3> Direction => direction;
        [ConfigProperty("Color")]
        public ReactiveProperty<Color> Color => color;
        [ConfigProperty("Temperature")]
        public ReactiveProperty<float> ColorTemperature => colorTemperature;
        [ConfigProperty("Intensity")]
        public ReactiveProperty<float> Intensity => intensity;
        [ConfigProperty("Indirect Multiplier")]
        public ReactiveProperty<float> BounceIntensity => bounceIntensity;


    }
}