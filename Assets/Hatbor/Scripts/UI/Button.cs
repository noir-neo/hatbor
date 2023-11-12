using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class Button<T> : VisualElement
    {
        readonly Button button;

        public string Label
        {
            get => button.text;
            set => button.text = value;
        }

        public Button()
        {
            button = new Button();
            hierarchy.Add(button);
        }

        public IDisposable Bind(ReactiveProperty<T> property, Func<T> onClicked)
        {

            return button.OnClickAsObservable()
                .Subscribe(_ => property.Value = onClicked());
        }

        public IDisposable Bind(ReactiveProperty<T> property, Func<UniTask<T>> onClicked)
        {
            return button.OnClickAsObservable()
                .Subscribe(_ =>
                    UniTask.Void(async () =>
                    {
                        var v = await onClicked();
                        property.Value = v;
                    }));
        }
    }
}