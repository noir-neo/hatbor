using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Config
{
    public sealed class ConfigStore : IStartable, IDisposable
    {
        readonly IEnumerable<IConfigurable> configurables;

        [Inject]
        public ConfigStore(IEnumerable<IConfigurable> configurables)
        {
            this.configurables = configurables;
        }

        void IStartable.Start()
        {
            foreach (var configurable in configurables)
            {
                Load(configurable);
            }
        }

        void IDisposable.Dispose()
        {
            foreach (var configurable in configurables)
            {
                Save(configurable);
            }
        }

        static void Load(IConfigurable configurable)
        {
            if (string.IsNullOrEmpty(configurable.PersistentKey)) return;
            var json = PlayerPrefs.GetString(configurable.PersistentKey);
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, configurable);
            }
        }

        static void Save(IConfigurable configurable)
        {
            if (string.IsNullOrEmpty(configurable.PersistentKey)) return;
            var json = JsonUtility.ToJson(configurable);
            PlayerPrefs.SetString(configurable.PersistentKey, json);
        }
    }
}