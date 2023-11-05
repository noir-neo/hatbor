using UnityEngine;

namespace Hatbor.UI
{
    public sealed class Vector2IntField : PropertyField<Vector2Int, UnityEngine.UIElements.Vector2IntField, UnityEngine.UIElements.IntegerField, int>
    {
        protected override string TemplatePath => @"Assets/Hatbor/UI/Vector2IntField.uxml";
    }
}