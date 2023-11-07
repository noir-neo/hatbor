using System;
using System.Reflection;
using Hatbor.Config;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class ConfigGroup : VisualElement
    {
        readonly IFileBrowser fileBrowser;

        readonly Label label;
        readonly VisualElement container;

        public ConfigGroup(IFileBrowser fileBrowser)
        {
            this.fileBrowser = fileBrowser;

            label = new Label();
            hierarchy.Add(label);
            container = new VisualElement();
            hierarchy.Add(container);
        }

        public IDisposable Bind(IConfigurable configurable)
        {
            var configurableType = configurable.GetType();

            label.text = GetConfigGroupAttribute(configurableType).Label;

            var disposables = new CompositeDisposable();

            foreach (var p in configurableType.GetMembers())
            {
                var provider = (ICustomAttributeProvider)p;
                if (provider.GetCustomAttributes(typeof(ConfigPropertyAttribute),false) is not ConfigPropertyAttribute[] attributes || attributes.Length == 0)
                    continue;

                var property = configurableType.GetProperty(p.Name)?.GetValue(configurable);
                var attr = attributes[0];
                var (element, disposable) = CreateFieldAndBind(property, attr);
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

        (VisualElement, IDisposable) CreateFieldAndBind(object property, ConfigPropertyAttribute attr)
        {
            return (property, attr) switch
            {
                (ReactiveProperty<bool> p, _) =>
                    CreateFieldAndBind<bool, Toggle>(p, attr.Label),
                (ReactiveProperty<int> p, _) =>
                    CreateFieldAndBind<int, IntegerField>(p, attr.Label),
                (ReactiveProperty<Vector2Int> p, _) =>
                    CreateFieldAndBind<Vector2Int, Vector2IntField>(p, attr.Label),
                (ReactiveProperty<string> p, FilePathConfigPropertyAttribute a) =>
                    CreateFilePathFieldAndBind(p, a),
                _ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
            };
        }

        static (VisualElement, IDisposable) CreateFieldAndBind<TValueType, TField>(
            ReactiveProperty<TValueType> property, string label)
            where TField : BaseField<TValueType>, new()
        {
            var propertyField = new PropertyField<TValueType, TField>
            {
                Label = label
            };
            return (propertyField, propertyField.Bind(property));
        }

        (VisualElement, IDisposable) CreateFilePathFieldAndBind(ReactiveProperty<string> property, FilePathConfigPropertyAttribute attr)
        {
            var button = new Button<string>
            {
                Label = attr.Label
            };
            return (button, button.Bind(property, () => fileBrowser.ChooseFileAsync(attr.Extension)));
        }
    }
}