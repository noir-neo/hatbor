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
        [RangeConfigProperty("Temperature", 1500f, 20000f)]
        public ReactiveProperty<float> ColorTemperature => colorTemperature;
        [RangeConfigProperty("Intensity", 0f, 10f)]
        public ReactiveProperty<float> Intensity => intensity;
        [RangeConfigProperty("Indirect Multiplier", 0f, 10f)]
        public ReactiveProperty<float> BounceIntensity => bounceIntensity;


    }
}