using System;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public class PropertyField<TField, TProperty> : VisualElement where TField : BaseField<TProperty>
    {
        protected virtual string TemplatePath { get; }

        readonly TField field;

        public string Label
        {
            get => field.label;
            set => field.label = value;
        }

        public PropertyField()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            var container = visualTree.Instantiate();
            hierarchy.Add(container);
            field = container.Q<TField>();
        }

        public IDisposable Bind(ReactiveProperty<TProperty> property)
        {
            return field.Bind(property);
        }
    }
}