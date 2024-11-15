using HandPaint.Scripts;
using PluginConfig.API.Fields;
using UnityEngine;

namespace HandPaint.Components
{
    public class ColoredWhiplash: AbstractColoredArm
    {
        [PrefabAsset("assets/textures/whiplash_mask.png")]
        private static Texture _mask;        
        
        [PrefabAsset("assets/textures/cube.png")]
        private static Texture _cube;

        private Material _coloredMaterial;
        private Material _originalMaterial;
        private Color _originalRopeColor;

        private SkinnedMeshRenderer _armRenderer;
        private SkinnedMeshRenderer _hookRenderer;
        private LineRenderer _ropeRenderer;
        
        private new void Start()
        {
            _armRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
            _hookRenderer = transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>();
            _ropeRenderer = transform.GetComponent<LineRenderer>();
            base.Start();

            var ropeColor = _ropeRenderer.startColor;
            _originalMaterial = Instantiate(_armRenderer.material);
            _originalRopeColor = new Color(ropeColor.r, ropeColor.g, ropeColor.b);

            _coloredMaterial = _armRenderer.material;
            _coloredMaterial.shader = CustomColorsShader;
            
            _hookRenderer.material = _coloredMaterial;
            MaterialBlock.SetTexture(IdTex, _mask);
            MaterialBlock.SetTexture(Cube, _cube);
            SetPropertyBlock();

            HandPaintConfig.WhiplashRopeColorField.onValueChange += OnRopeColor;
            HandPaintConfig.WhiplashRopeColorField.TriggerValueChangeEvent();
            HandPaintConfig.RepaintWhiplash.TriggerValueChangeEvent();
        }
        
        protected override void RestoreVanillaMaterials()
        {
            if (_armRenderer == null || _hookRenderer == null)
                return;

            _armRenderer.material = _originalMaterial;
            _hookRenderer.material = _originalMaterial;
            _ropeRenderer.startColor = _originalRopeColor;
            _ropeRenderer.endColor = _originalRopeColor;
        }

        protected override void Repaint()
        {
            if (_armRenderer == null || _hookRenderer == null)
                return;

            _armRenderer.material = _coloredMaterial;
            _hookRenderer.material = _coloredMaterial;
            _ropeRenderer.startColor = HandPaintConfig.WhiplashRopeColorField.Value;
            _ropeRenderer.endColor = HandPaintConfig.WhiplashRopeColorField.Value;
        }

        public void OnRopeColor(ColorField.ColorValueChangeEvent e)
        {
            _ropeRenderer.startColor = e.value;
            _ropeRenderer.endColor = e.value;
        }
        
        protected override void SetPropertyBlock()
        {
            _armRenderer.SetPropertyBlock(MaterialBlock);
            _hookRenderer.SetPropertyBlock(MaterialBlock);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandPaintConfig.WhiplashRopeColorField.onValueChange -= OnRopeColor;
        }
        
        protected override BoolField EnableRepaintingField() => HandPaintConfig.RepaintWhiplash;

        protected override ColorAlphaField[] ColorFields() => HandPaintConfig.WhiplashColors;
    }
}