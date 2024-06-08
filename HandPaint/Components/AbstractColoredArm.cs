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

        protected void Start()
        {
            MaterialBlock = new MaterialPropertyBlock();
            CustomColorsShader = Addressables
                .LoadAssetAsync<Shader>("Assets/Shaders/Special/ULTRAKILL-vertexlit-customcolors-emissive.shader")
                .WaitForCompletion();

            for (var i = 0; i < 3; i++)
            {
                var num = i;
                _colorHandlers[i] = v =>
                {
                    MaterialBlock.SetColor(ColorProperties[num], v.value);
                    SetPropertyBlock();
                };
                ColorFields()[i].onValueChange += _colorHandlers[i];
            }

            foreach (var field in ColorFields())
                field.TriggerValueChangeEvent();

            ConfigFields.EnableRepaint.onValueChange += v =>
            {
                if (v.value)
                    Repaint();
                else
                    RestoreVanillaMaterials();
            };
        }

        protected abstract void RestoreVanillaMaterials();

        protected abstract void Repaint();

        protected abstract ColorAlphaField[] ColorFields();

        protected abstract void SetPropertyBlock();

        protected void OnDestroy()
        {
            for (var i = 0; i < 3; i++)
                ColorFields()[i].onValueChange -= _colorHandlers[i];
        }
    }
}