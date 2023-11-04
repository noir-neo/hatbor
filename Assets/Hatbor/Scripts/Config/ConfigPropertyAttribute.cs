using System;

namespace Hatbor.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConfigPropertyAttribute : Attribute
    {
        public string Label { get; set; }

        public ConfigPropertyAttribute(string label)
        {
            Label = label;
        }
    }
}