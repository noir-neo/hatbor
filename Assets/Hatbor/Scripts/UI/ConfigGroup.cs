using System;
using System.Reflection;
using Hatbor.Config;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class ConfigGroup : VisualElement
    {
        const string TemplatePath = "Assets/Hatbor/UI/ConfigGroup.uxml";

        readonly TemplateContainer container;

        public ConfigGroup()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            container = visualTree.Instantiate();
            hierarchy.Add(container);
        }

        public IDisposable Bind(IConfigurable configurable)
        {
            var configurableType = configurable.GetType();

            var label = container.Q<Label>();
            label.text = GetConfigGroupAttribute(configurableType).Label;

            var disposables = new CompositeDisposable();

            foreach (var p in configurableType.GetMembers())
            {
                var provider = (ICustomAttributeProvider)p;
                if (provider.GetCustomAttributes(typeof(ConfigPropertyAttribute),false) is not ConfigPropertyAttribute[] attributes || attributes.Length == 0)
                    continue;

                var property = configurableType.GetProperty(p.Name)?.GetValue(configurable);
                var attr = attributes[0];
                var (element, disposable) = CreateFieldAndBind(property, attr.Label);
                disposable.AddTo(disposables);
                container.Add(element);
            }

            return disposables;
        }

        static ConfigGroupAttribute GetConfigGroupAttribute(ICustomAttributeProvider provider)
        {
            return provider.GetCustomAttributes(typeof(ConfigGroupAttribute),false) is ConfigGroupAttribute[] { Length: > 0 } attributes
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