using System;

namespace Hatbor.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingsPropertyAttribute : Attribute
    {
        public string Label { get; set; }

        public SettingsPropertyAttribute(string label)
        {
            Label = label;
        }
    }
}