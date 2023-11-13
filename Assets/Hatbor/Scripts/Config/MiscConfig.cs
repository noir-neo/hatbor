using System;
using System.IO;
using UnityEngine;

namespace Hatbor.Config
{
    [ConfigGroup("Misc.")]
    public sealed class MiscConfig : IConfigurable
    {
        // This config group should not persist
        public string PersistentKey { get; }

        [ConfigProperty("Legal notices")]
        public Action OpenLegalNotices => OpenLegalNoticesInternal;

        static void OpenLegalNoticesInternal()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "THIRD-PARTY-NOTICES.txt");
            System.Diagnostics.Process.Start(path);
        }
    }
}