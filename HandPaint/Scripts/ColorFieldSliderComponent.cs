using UnityEngine;
using UnityEngine.EventSystems;

namespace HandPaint.Scripts
{
    internal class ColorFieldSliderComponent : MonoBehaviour, IPointerUpHandler
    {
        public ColorAlphaField callback;

        public void OnPointerUp(PointerEventData data)
        {
            callback?.OnValueChange(callback.GetColorFromSliders());
        }
    }
}