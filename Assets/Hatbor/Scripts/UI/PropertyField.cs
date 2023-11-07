using System;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public class PropertyField<TValueType, TField> : VisualElement where TField : BaseField<TValueType>, new()
    {
        readonly TField field;

        public string Label
        {
            get => field.label;
            set => field.label = value;
        }

        public PropertyField()
        {
            field = new TField();
            hierarchy.Add(field);
        }

        public IDisposable Bind(ReactiveProperty<TValueType> property)
        {
            return field.Bind(property);
        }
    }

    public class PropertyField<TValueType, TCompositeField, TField, TFieldValue> : PropertyField<TValueType, TCompositeField>
        where TCompositeField : BaseCompositeField<TValueType, TField, TFieldValue>, new()
        where TField : TextValueField<TFieldValue>, new()
    {
    }
}