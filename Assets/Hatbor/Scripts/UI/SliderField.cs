using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public sealed class SliderField : PropertyField<float, Slider>
    {
        public SliderField(float min, float max)
        {
            field.lowValue = min;
            field.highValue = max;
            field.showInputField = true;
        }
    }
}
