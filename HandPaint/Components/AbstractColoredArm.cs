using HandPaint.Scripts;
using PluginConfig.API.Fields;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HandPaint.Components
{
    public abstract class AbstractColoredArm: MonoBehaviour
    {
        protected static readonly int IdTex = Shader.PropertyToID("_IDTex");
        protected static readonly int Cube = Shader.PropertyToID("_Cube");
        protected static readonly int[] ColorProperties = {
            Shader.PropertyToID("_CustomColor1"),
            Shader.PropertyToID("_CustomColor2"),
            Shader.PropertyToID("_CustomColor3")
        };

        protected Shader CustomColorsShader;
        protected MaterialPropertyBlock MaterialBlock;

        private readonly ColorField.ColorValueChangeEventDelegate[] _colorHandlers =
            new ColorField.ColorValueChangeEventDelegate[3];
        
        private bool _skittlesPoxActive;
        private Color _skittlesPoxValue = new Color(1, 0, 0, 1);
        
        private SkittlesPox _skittlesPox;

        protected void Start()
        {
            _skittlesPox = SkittlesPox.Instance;
            MaterialBlock = new MaterialPropertyBlock();
            CustomColorsShader = Addressables
                .LoadAssetAsync<Shader>("Assets/Shaders/Special/ULTRAKILL-vertexlit-customcolors-emissive.shader")
                .WaitForCompletion();

            BindHandlers();
            EnableRepaintingField().onValueChange += v =>
            {
                if (v.value)
                    Repaint();
                else
                    RestoreVanillaMaterials();
            };
        }

        private void FixedUpdate()
        {
            if (!_skittlesPox.activated)
            {
                if (!_skittlesPoxActive)
                    return;

                _skittlesPoxActive = false;
                BindHandlers();
                return;
            }

            if (!_skittlesPoxActive)
            {
                UnbindHandlers();
                _skittlesPoxActive = true;
                return;
            }
            
            for (var i = 0; i < 3; i++)
            {
                var hue = (_skittlesPox.hue +
                          Time.fixedDeltaTime * HandPaintConfig.EasterEggConfig.Frequency.value / 60 +
                          HandPaintConfig.EasterEggConfig.Phases[i].value) % 1f;

                _skittlesPoxValue = Color.HSVToRGB(hue, 1, 1);
                MaterialBlock.SetColor(ColorProperties[i], _skittlesPoxValue);
                SetPropertyBlock();
            }
        }

        protected void BindHandlers()
        {
            var fields = ColorFields();
            for (var i = 0; i < 3; i++)
            {
                var num = i;
                _colorHandlers[i] = v =>
                {
                    MaterialBlock.SetColor(ColorProperties[num], v.value);
                    SetPropertyBlock();
                };
                fields[i].onValueChange += _colorHandlers[i];
                fields[i].TriggerValueChangeEvent();
            }
        }
        
        protected void UnbindHandlers()
        {
            var fields = ColorFields();
            for (var i = 0; i < fields.Length; i++)
                fields[i].onValueChange -= _colorHandlers[i];
        }

        protected abstract void RestoreVanillaMaterials();

        protected abstract void Repaint();

        protected abstract BoolField EnableRepaintingField();
        
        protected abstract ColorAlphaField[] ColorFields();

        protected abstract void SetPropertyBlock();

        protected virtual void OnDestroy() => UnbindHandlers();
    }
}