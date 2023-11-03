using System;

namespace Hatbor.Settings
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsGroupAttribute : Attribute
    {
        public string Label { get; }

        public SettingsGroupAttribute(string label)
        {
            Label = label;
        }
    }
}