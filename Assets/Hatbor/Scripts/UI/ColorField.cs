using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public class ColorField : PropertyField<string, TextField>
    {
        public IDisposable Bind(ReactiveProperty<Color> property)
        {
            return field.Bind(property, TryParseHtmlString, ToHtmlStringRGBA);
        }

        static bool TryParseHtmlString(string input, out Color output)
        {
            if (ColorUtility.TryParseHtmlString(input, out output))
            {
                return true;
            }
            output = Color.white;
            return false;
        }

        static bool ToHtmlStringRGBA(Color input, out string output)
        {
            output = "#" + ColorUtility.ToHtmlStringRGBA(input);
            return true;
        }
    }
}