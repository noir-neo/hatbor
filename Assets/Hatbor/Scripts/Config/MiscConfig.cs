using System;
using UniRx;

namespace Hatbor.Config
{
    [ConfigGroup("Misc.")]
    public sealed class MiscConfig : IConfigurable
    {
        // This config group should not persist
        public string PersistentKey { get; }

        public ReactiveProperty<bool> LegalNotices { get; } = new(false);

        [ConfigProperty("Legal notices")]
        public Action OpenLegalNotices => OpenLegalNoticesInternal;
        void OpenLegalNoticesInternal()
        {
            LegalNotices.Value = true;
        }
    }
}