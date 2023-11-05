using System;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public class PropertyField<TValueType, TField> : VisualElement where TField : BaseField<TValueType>
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

        public IDisposable Bind(ReactiveProperty<TValueType> property)
        {
            return field.Bind(property);
        }
    }

    public class PropertyField<TValueType, TCompositeField, TField, TFieldValue> : PropertyField<TValueType, TCompositeField>
        where TCompositeField : BaseCompositeField<TValueType, TField, TFieldValue>
        where TField : TextValueField<TFieldValue>, new()
    {
    }
}