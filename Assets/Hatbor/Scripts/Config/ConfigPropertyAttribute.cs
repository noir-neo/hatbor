using System;
using UnityEngine.Scripting;

namespace Hatbor.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : PreserveAttribute
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