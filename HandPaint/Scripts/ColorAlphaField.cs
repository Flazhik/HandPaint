using System;
using System.Globalization;
using PluginConfig.API;
using PluginConfig.API.Fields;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HandPaint.Scripts
{
    public class ColorAlphaField: CustomConfigValueField
    {
        [PrefabAsset("assets/ui/coloralphafield.prefab")]
        private static GameObject fieldPrefab;

        private readonly Color _defaultValue;
        private ConfigColorAlphaField _currentUi;
        private Color _value;
        
        private string _lastRed = "0";
        private string _lastGreen = "0";
        private string _lastBlue = "0";
        private string _lastAlpha = "0";

        public Color Value
        {
            get => _value;
            private set
            {
                if (_currentUi != null)
                    SetSliders(value);

                _value = value;
                fieldValue = StringifyColor(value);
            }
        }
        
        public event ColorField.ColorValueChangeEventDelegate onValueChange;
        public event ColorField.PostColorValueChangeEvent postValueChangeEvent;
        
        public void TriggerValueChangeEvent()
        {
            if (onValueChange == null)
                return;
            var data = new ColorField.ColorValueChangeEvent
            {
                value = _value
            };
            onValueChange(data);
            if (data.canceled || !(data.value != _value))
                return;
            Value = data.value;
        }

        public override void OnDisplayNameChange(string newName)
        {
            if (!(_currentUi != null))
                return;
            _currentUi.nameText.text = displayName;
        }

        private void SetSliders(Color c)
        {
            if (_currentUi == null)
                return;
            _currentUi.red.SetValueWithoutNotify(c.r);
            _currentUi.green.SetValueWithoutNotify(c.g);
            _currentUi.blue.SetValueWithoutNotify(c.b);
            _currentUi.alpha.SetValueWithoutNotify(c.a);
            _currentUi.redInput.SetTextWithoutNotify(((int) (c.r * (double) byte.MaxValue)).ToString());
            _currentUi.greenInput.SetTextWithoutNotify(((int) (c.g * (double) byte.MaxValue)).ToString());
            _currentUi.blueInput.SetTextWithoutNotify(((int) (c.b * (double) byte.MaxValue)).ToString());
            _currentUi.alphaInput.SetTextWithoutNotify(((int) (c.a * (double) byte.MaxValue)).ToString());
            _currentUi.SetColor(c.r, c.g, c.b, c.a);
        }
        
        internal Color GetColorFromSliders() =>
            new Color(_currentUi.red.normalizedValue, _currentUi.green.normalizedValue, _currentUi.blue.normalizedValue, _currentUi.alpha.normalizedValue);

        protected override GameObject CreateUI(Transform content)
        {
            var field = Object.Instantiate(fieldPrefab, content);
            _currentUi = field.GetComponent<ConfigColorAlphaField>();
            _currentUi.nameText.text = displayName;
            _currentUi.red.interactable = interactable;
            _currentUi.red.gameObject.AddComponent<ColorFieldSliderComponent>().callback = this;
            _currentUi.green.interactable = interactable;
            _currentUi.green.gameObject.AddComponent<ColorFieldSliderComponent>().callback = this;
            _currentUi.blue.interactable = interactable;
            _currentUi.blue.gameObject.AddComponent<ColorFieldSliderComponent>().callback = this;
            _currentUi.alpha.interactable = interactable;
            _currentUi.alpha.gameObject.AddComponent<ColorFieldSliderComponent>().callback = this;
            _currentUi.redInput.interactable = interactable;
            _currentUi.redInput.onValueChanged.AddListener(val =>
            {
                if (_currentUi.redInput.wasCanceled)
                    return;
                _lastRed = val;
            });
            _currentUi.redInput.onEndEdit.AddListener(val => OnInputFieldChange(_currentUi.redInput, _currentUi.red, ref _lastRed));
            _currentUi.greenInput.interactable = interactable;
            _currentUi.greenInput.onValueChanged.AddListener(val =>
            {
                if (_currentUi.greenInput.wasCanceled)
                    return;
                _lastGreen = val;
            });
            _currentUi.greenInput.onEndEdit.AddListener(val => OnInputFieldChange(_currentUi.greenInput, _currentUi.green, ref _lastGreen));
            _currentUi.blueInput.interactable = interactable;
            _currentUi.blueInput.onValueChanged.AddListener(val =>
            {
                if (_currentUi.blueInput.wasCanceled)
                    return;
                _lastBlue = val;
            });
            _currentUi.blueInput.onEndEdit.AddListener(val => OnInputFieldChange(_currentUi.blueInput, _currentUi.blue, ref _lastBlue));
            _currentUi.alphaInput.interactable = interactable;
            _currentUi.alphaInput.onValueChanged.AddListener(val =>
            {
                if (_currentUi.alphaInput.wasCanceled)
                    return;
                _lastAlpha = val;
            });
            _currentUi.alphaInput.onEndEdit.AddListener(val => OnInputFieldChange(_currentUi.alphaInput, _currentUi.alpha, ref _lastAlpha));
            SetSliders(_value);
            _currentUi.resetButton.onClick = new Button.ButtonClickedEvent();
            _currentUi.resetButton.onClick.AddListener(OnReset);
            _currentUi.resetButton.gameObject.SetActive(false);
            var componentInParent = content.gameObject.GetComponentInParent<ScrollRect>();
            Utils.SetupResetButton(field, componentInParent, e =>
            {
                if (!interactable)
                    return;
                _currentUi.resetButton.gameObject.SetActive(true);
            }, e => _currentUi.resetButton.gameObject.SetActive(false));
            Utils.AddScrollEvents(_currentUi.redInput.gameObject.AddComponent<EventTrigger>(), componentInParent);
            Utils.AddScrollEvents(_currentUi.greenInput.gameObject.AddComponent<EventTrigger>(), componentInParent);
            Utils.AddScrollEvents(_currentUi.blueInput.gameObject.AddComponent<EventTrigger>(), componentInParent);
            Utils.AddScrollEvents(_currentUi.alphaInput.gameObject.AddComponent<EventTrigger>(), componentInParent);
            field.SetActive(!hidden);
            SetInteractableColor(interactable);

            return field;
        }

        internal void OnValueChange(Color newColor)
        {
            if (newColor == _value)
            {
                Value = _value;
                return;
            }

            var eventData = new ColorField.ColorValueChangeEvent { value = newColor };
            if (onValueChange != null)
            {
                try
                {
                    onValueChange.Invoke(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Value change event for {guid} threw an error: {e}");
                }
            }

            Value = eventData.canceled ? _value : eventData.value;

            if (postValueChangeEvent == null)
                return;
            
            try
            {
                postValueChangeEvent.Invoke(_value);
            }
            catch (Exception e)
            {
                Debug.LogError($"Post value change event for {guid} threw an error: {e}");
            }
        }

        private void OnInputFieldChange(
            TMP_InputField field,
            Slider targetSlider,
            ref string lastValue)
        {
            if (field.wasCanceled)
                return;
            
            var num1 = (int) (targetSlider.normalizedValue * (double) byte.MaxValue);
            if (!int.TryParse(field.text, out var result))
            {
                field.SetTextWithoutNotify(num1.ToString());
            }
            else
            {
                var num2 = Mathf.Clamp(result, 0, byte.MaxValue);
                field.SetTextWithoutNotify(num2.ToString());
                if (num1 == num2)
                    return;
                targetSlider.SetNormalizedValueWithoutNotify(num2 / (float) byte.MaxValue);
                OnValueChange(new Color(_currentUi.red.normalizedValue, _currentUi.green.normalizedValue, _currentUi.blue.normalizedValue, _currentUi.alpha.normalizedValue));
            }
        }

        private void OnReset()
        {
            SetSliders(_defaultValue);
            OnValueChange(_defaultValue);
        }
        
        protected override void LoadDefaultValue()
        {
            Value = _defaultValue;
        }

        protected sealed override void LoadFromString(string data)
        {
            var colorSplit = data.Split(',');
            var validData = colorSplit.Length == 4;
            if (!validData)
                return;
            if (!float.TryParse(colorSplit[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var r))
                validData = false;
            if (!float.TryParse(colorSplit[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var g))
                validData = false;
            if (!float.TryParse(colorSplit[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var b))
                validData = false;
            if (!float.TryParse(colorSplit[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var a))
                validData = false;

            if (validData)
                _value = new Color(r, g, b, a);
            else
                OnValueChange(new Color(_defaultValue.r, _defaultValue.g, _defaultValue.b, _defaultValue.a));
        }
        
        private static string StringifyColor(Color c) =>
            $"{c.r.ToString(CultureInfo.InvariantCulture)},{c.g.ToString(CultureInfo.InvariantCulture)},{c.b.ToString(CultureInfo.InvariantCulture)},{c.a.ToString(CultureInfo.InvariantCulture)}";

        private void SetInteractableColor(bool interactable)
        {
            if (_currentUi == null)
                return;
            _currentUi.nameText.color = interactable ? Color.white : Color.gray;
        }

        public ColorAlphaField(ConfigPanel parentPanel, string displayName, string guid, Color defaultValue)
            : base(parentPanel, guid, displayName)
        {
            _defaultValue = defaultValue;
            if (fieldValue == null)
                Value = defaultValue;
            else
                LoadFromString(fieldValue);
        }
    }
}