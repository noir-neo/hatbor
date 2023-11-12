using System;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public static class RuntimeBindingExtensions
    {
        public static IDisposable Bind<T>(this INotifyValueChanged<T> field, ReactiveProperty<T> property)
        {
            var disposables = new CompositeDisposable();

            property.Subscribe(v => field.value = v)
                .AddTo(disposables);

            void OnFieldValueChanged(ChangeEvent<T> e) => property.Value = e.newValue;
            field.RegisterValueChangedCallback(OnFieldValueChanged);
            Disposable.Create(() => field.UnregisterValueChangedCallback(OnFieldValueChanged))
                .AddTo(disposables);

            return disposables;
        }

        public delegate bool Parser<TInput, TOutput>(TInput input, out TOutput output);
        public static IDisposable Bind<TField, TProperty>(this INotifyValueChanged<TField> field,
            ReactiveProperty<TProperty> property,
            Parser<TField, TProperty> fieldTypeToPropertyType,
            Parser<TProperty, TField> propertyTypeToFieldType)
        {
            var disposables = new CompositeDisposable();

            property.Subscribe(v =>
                {
                    if (propertyTypeToFieldType(v, out var newValue))
                    {
                        field.value = newValue;
                    }
                })
                .AddTo(disposables);

            void OnFieldValueChanged(ChangeEvent<TField> e)
            {
                if (fieldTypeToPropertyType(e.newValue, out var newValue))
                {
                    property.Value = newValue;
                }
            }

            field.RegisterValueChangedCallback(OnFieldValueChanged);
            Disposable.Create(() => field.UnregisterValueChangedCallback(OnFieldValueChanged))
                .AddTo(disposables);

            return disposables;
        }
    }
}