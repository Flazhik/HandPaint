using UnityEngine.UI;

namespace HandPaint.Scripts
{
    public static class SliderExtensions
    {
        public static void SetNormalizedValueWithoutNotify(this Slider slider, float normalized) =>
            slider.SetValueWithoutNotify(slider.minValue + normalized * (slider.maxValue - slider.minValue));
    }
}