using System;
using Hatbor.Config;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Light
{
    public sealed class LightPresenter : IStartable, IDisposable
    {
        readonly LightConfig config;
        readonly UnityEngine.Light light;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public LightPresenter(LightConfig config,
            UnityEngine.Light light)
        {
            this.config = config;
            this.light = light;
        }

        void IStartable.Start()
        {
            config.Direction
                .Subscribe(x => light.transform.rotation = Quaternion.Euler(x))
                .AddTo(disposables);
            config.Color
                .Subscribe(x => light.color = x)
                .AddTo(disposables);
            config.ColorTemperature
                .Subscribe(x => light.colorTemperature = x)
                .AddTo(disposables);
            config.Intensity
                .Subscribe(x => light.intensity = x)
                .AddTo(disposables);
            config.BounceIntensity
                .Subscribe(x => light.bounceIntensity = x)
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}