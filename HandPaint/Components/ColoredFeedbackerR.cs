using HandPaint.Scripts;
using PluginConfig.API.Fields;
using UnityEngine;

namespace HandPaint.Components
{
    public class ColoredFeedbackerR: AbstractColoredArm
    {
        [PrefabAsset("assets/textures/feedbacker_mask.png")]
        private static Texture _mask;        
        
        [PrefabAsset("assets/textures/cube.png")]
        private static Texture _cube;
        
        private Material _coloredMaterial;
        private Material _originalMaterial;

        private SkinnedMeshRenderer _armRenderer;
        private ColorAlphaField[] _colorFields;

        private new void Start()
        {
            _armRenderer = transform.GetComponent<SkinnedMeshRenderer>();
            _colorFields = HandPaintConfig.FeedbackerColors;
            base.Start();

            _originalMaterial = Instantiate(_armRenderer.material);
            _coloredMaterial = _armRenderer.material;

            _coloredMaterial.shader = CustomColorsShader;
            MaterialBlock.SetTexture(IdTex, _mask);
            MaterialBlock.SetTexture(Cube, _cube);
            SetPropertyBlock();

            HandPaintConfig.PaintFeedbackerSeparately.onValueChange += OnSeparatePaint;
            HandPaintConfig.PaintFeedbackerSeparately.TriggerValueChangeEvent();
            HandPaintConfig.RepaintFeedbacker.TriggerValueChangeEvent();
        }
        
        public void OnSeparatePaint(BoolField.BoolValueChangeEvent v) {
            UnbindHandlers();
            _colorFields = v.value
                ? HandPaintConfig.RightFeedbackerColors
                : HandPaintConfig.FeedbackerColors;
            BindHandlers();
        }
        
        protected override void RestoreVanillaMaterials()
        {
            if (_armRenderer == null)
                return;

            _armRenderer.material = _originalMaterial;
        }

        protected override void Repaint()
        {
            if (_armRenderer == null)
                return;

            _armRenderer.material = _coloredMaterial;
        }
        
        protected override void SetPropertyBlock()
        {
            _armRenderer.SetPropertyBlock(MaterialBlock);
        }
        
        protected override BoolField EnableRepaintingField() => HandPaintConfig.RepaintFeedbacker;

        protected override ColorAlphaField[] ColorFields()
        {
            return _colorFields;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandPaintConfig.PaintFeedbackerSeparately.onValueChange -= OnSeparatePaint;
        }
    }
}