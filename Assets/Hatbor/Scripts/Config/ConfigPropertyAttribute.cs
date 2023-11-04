using System;

namespace Hatbor.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string Label { get; }

        public ConfigPropertyAttribute(string label)
        {
            Label = label;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FilePathConfigPropertyAttribute : ConfigPropertyAttribute
    {
        public string Extension { get; }

        public FilePathConfigPropertyAttribute(string label, string extension) : base(label)
        {
            Extension = extension;
        }
    }
}