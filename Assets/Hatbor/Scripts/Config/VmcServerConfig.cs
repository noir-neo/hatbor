using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("VmcServer")]
    public sealed class VmcServerConfig : IConfigurable
    {
        public string PersistentKey => "VmcServerConfig";

        [SerializeField] public BoolReactiveProperty enabled = new(true);
        [SerializeField] public IntReactiveProperty port = new(39539);

        [ConfigProperty("Enabled")]
        public ReactiveProperty<bool> Enabled => enabled;
        [ConfigProperty("Port")]
        public ReactiveProperty<int> Port => port;
    }
}