using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class Button<T> : VisualElement
    {
        static string TemplatePath => @"Assets/Hatbor/UI/Button.uxml";

        readonly UnityEngine.UIElements.Button button;

        public string Label
        {
            get => button.text;
            set => button.text = value;
        }

        public Button()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            var container = visualTree.Instantiate();
            hierarchy.Add(container);
            button = container.Q<UnityEngine.UIElements.Button>();
        }

        public IDisposable Bind(ReactiveProperty<T> property, Func<T> onClicked)
        {

            return Observable.FromEvent(
                h => button.clickable.clicked += h,
                h => button.clickable.clicked -= h
            ).Subscribe(_ => property.Value = onClicked());
        }

        public IDisposable Bind(ReactiveProperty<T> property, Func<UniTask<T>> onClicked)
        {
            return Observable.FromEvent(
                h => button.clickable.clicked += h,
                h => button.clickable.clicked -= h
                ).Subscribe(_ =>
                    UniTask.Void(async () =>
                    {
                        var v = await onClicked();
                        property.Value = v;
                    }));
        }
    }
}