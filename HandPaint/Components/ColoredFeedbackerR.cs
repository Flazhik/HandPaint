using HandPaint.Scripts;
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

        private new void Start()
        {
            _armRenderer = transform.GetComponent<SkinnedMeshRenderer>();
            base.Start();

            _originalMaterial = Instantiate(_armRenderer.material);
            _coloredMaterial = _armRenderer.material;

            _coloredMaterial.shader = CustomColorsShader;
            MaterialBlock.SetTexture(IdTex, _mask);
            MaterialBlock.SetTexture(Cube, _cube);
            SetPropertyBlock();
            ConfigFields.EnableRepaint.TriggerValueChangeEvent();
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

        protected override ColorAlphaField[] ColorFields() => ConfigFields.FeedbackerColors;
    }
}