using System;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public static class RuntimeBindingExtensions
    {
        public static IDisposable Bind<T>(this BaseField<T> field, ReactiveProperty<T> property)
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
    }
}