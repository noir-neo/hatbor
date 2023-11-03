using System;
using UniRx;
using UnityEngine;

namespace Hatbor.Settings
{
    [Serializable, SettingsGroup("VmcServer")]
    public sealed class VmcServerSettings : ISettings
    {
        const string PersistentKey = "VmcServerSettings";

        [SerializeField] public BoolReactiveProperty enabled = new(true);
        [SerializeField] public IntReactiveProperty port = new(39539);

        [SettingsProperty("Enabled")]
        public ReactiveProperty<bool> Enabled => enabled;
        [SettingsProperty("Port")]
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