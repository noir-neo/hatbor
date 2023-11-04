using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    [Serializable, ConfigGroup("VmcServer")]
    public sealed class VmcServerConfig : IConfigurable
    {
        const string PersistentKey = "VmcServerConfig";

        [SerializeField] public BoolReactiveProperty enabled = new(true);
        [SerializeField] public IntReactiveProperty port = new(39539);

        [ConfigProperty("Enabled")]
        public ReactiveProperty<bool> Enabled => enabled;
        [ConfigProperty("Port")]
        public ReactiveProperty<int> Port => port;

        public void Save()
        {
            var json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(PersistentKey, json);
        }

        public void Load()
        {
            var json = PlayerPrefs.GetString(PersistentKey);
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, this);
            }
        }
    }
}