using System;
using System.Reflection;
using Hatbor.Settings;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class SettingsGroup : VisualElement
    {
        const string TemplatePath = "Assets/Hatbor/UI/SettingsGroup.uxml";

        readonly TemplateContainer container;

        public SettingsGroup()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            container = visualTree.Instantiate();
            hierarchy.Add(container);
        }

        public IDisposable Bind(ISettings settings)
        {
            var settingsType = settings.GetType();

            var label = container.Q<Label>();
            label.text = GetSettingsGroupAttribute(settingsType).Label;

            var disposables = new CompositeDisposable();

            foreach (var p in settingsType.GetMembers())
            {
                var provider = (ICustomAttributeProvider)p;
                if (provider.GetCustomAttributes(typeof(SettingsPropertyAttribute),false) is not SettingsPropertyAttribute[] attributes || attributes.Length == 0)
                    continue;

                var property = settingsType.GetProperty(p.Name)?.GetValue(settings);
                var attr = attributes[0];
                var (element, disposable) = CreateFieldAndBind(property, attr.Label);
                disposable.AddTo(disposables);
                container.Add(element);
            }

            return disposables;
        }

        static SettingsGroupAttribute GetSettingsGroupAttribute(ICustomAttributeProvider provider)
        {
            return provider.GetCustomAttributes(typeof(SettingsGroupAttribute),false) is SettingsGroupAttribute[] { Length: > 0 } attributes
                ? attributes[0]
                : null;
        }

        static (VisualElement, IDisposable) CreateFieldAndBind(object property, string label)
        {
            return property switch
            {
                ReactiveProperty<bool> p => CreateFieldAndBind<Toggle, UnityEngine.UIElements.Toggle, bool>(p, label),
                ReactiveProperty<int> p => CreateFieldAndBind<IntegerField, UnityEngine.UIElements.IntegerField, int>(p, label),
                _ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
            };
        }

        static (VisualElement, IDisposable) CreateFieldAndBind<TPropertyField, TField, TProperty>(ReactiveProperty<TProperty> property, string label)
            where TPropertyField : PropertyField<TField, TProperty>, new()
            where TField : BaseField<TProperty>
        {
            var propertyField = new TPropertyField
            {
                Label = label
            };
            return (propertyField, propertyField.Bind(property));
        }
    }
}