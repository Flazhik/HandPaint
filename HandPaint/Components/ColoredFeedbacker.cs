using HandPaint.Scripts;
using PluginConfig.API.Fields;
using UnityEngine;

namespace HandPaint.Components
{
    public class ColoredFeedbacker: AbstractColoredArm
    {
        [PrefabAsset("assets/textures/feedbacker_l_mask.png")]
        private static Texture _mask;        
        
        [PrefabAsset("assets/textures/cube.png")]
        private static Texture _cube;
        
        private Material _coloredMaterial;
        private Material _originalMaterial;

        private SkinnedMeshRenderer _armRenderer;

        private new void Start()
        {
            _armRenderer = transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            base.Start();

            _originalMaterial = Instantiate(_armRenderer.material);
            _coloredMaterial = _armRenderer.material;

            _coloredMaterial.shader = CustomColorsShader;
            MaterialBlock.SetTexture(IdTex, _mask);
            MaterialBlock.SetTexture(Cube, _cube);
            SetPropertyBlock();
            HandPaintConfig.RepaintFeedbacker.TriggerValueChangeEvent();
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
        
        protected override void SetPropertyBlock() => _armRenderer.SetPropertyBlock(MaterialBlock);

        protected override BoolField EnableRepaintingField() => HandPaintConfig.RepaintFeedbacker;
        
        protected override ColorAlphaField[] ColorFields() => HandPaintConfig.FeedbackerColors;
    }
}