using System;

namespace Hatbor.Config
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigGroupAttribute : Attribute
    {
        public string Label { get; }

        public ConfigGroupAttribute(string label)
        {
            Label = label;
        }
    }
}