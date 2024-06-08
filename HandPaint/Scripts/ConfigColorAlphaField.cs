using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HandPaint.Scripts
{
    public class ConfigColorAlphaField : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Image image;
        public Image metallicPreview;
        public Slider red;
        public Slider green;
        public Slider blue;
        public Slider alpha;
        public TMP_InputField redInput;
        public TMP_InputField greenInput;
        public TMP_InputField blueInput;
        public TMP_InputField alphaInput;
        public Button resetButton;

        public void SliderSetR(float newR)
        {
            redInput.SetTextWithoutNotify(((int) (red.normalizedValue * (double) byte.MaxValue)).ToString());
            SetColor();
        }

        public void SliderSetG(float newG)
        {
            greenInput.SetTextWithoutNotify(((int) (green.normalizedValue * (double) byte.MaxValue)).ToString());
            SetColor();
        }

        public void SliderSetB(float newB)
        {
            blueInput.SetTextWithoutNotify(((int) (blue.normalizedValue * (double) byte.MaxValue)).ToString());
            SetColor();
        }
        
        public void SliderSetA(float newA)
        {
            alphaInput.SetTextWithoutNotify(((int) (alpha.normalizedValue * (double) byte.MaxValue)).ToString());
            SetColor();
        }

        public void SetColor() => SetColor(red.value, green.value, blue.value, alpha.value);

        public void SetColor(float newR, float newG, float newB, float newA)
        {
            image.color = new Color(newR, newG, newB);
            var num = Vector3.Dot(Vector3.one, new Vector3(newR, newG, newB)) / 3.0 < 0.9 ? 1f : 0.7f;
            metallicPreview.color = new Color(num, num, num, newA);
        }
    }
}