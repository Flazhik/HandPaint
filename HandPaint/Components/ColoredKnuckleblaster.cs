using HandPaint.Scripts;
using PluginConfig.API.Fields;
using UnityEngine;

namespace HandPaint.Components
{
    public class ColoredKnuckleblaster: AbstractColoredArm
    {
        [PrefabAsset("assets/textures/knuckleblaster_mask.png")]
        private static Texture _mask;
        
        [PrefabAsset("assets/textures/shell_mask.png")]
        private static Texture _shellMask;
        
        [PrefabAsset("assets/textures/cube.png")]
        private static Texture _cube;

        public GameObject shell;
        
        private Material _coloredArmMaterial;
        private Material _coloredShellMaterial;
        private Material _originalArmMaterial;
        private Material _originalShellMaterial;

        private SkinnedMeshRenderer _armRenderer;
        private MeshRenderer _shellRenderer;

        private new void Start()
        {
            _armRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            _shellRenderer = shell.transform.GetChild(0).GetComponent<MeshRenderer>();
            base.Start();

            _originalArmMaterial = Instantiate(_armRenderer.material);
            _originalShellMaterial = Instantiate(_shellRenderer.material);
            
            _coloredArmMaterial = _armRenderer.material;
            _coloredArmMaterial.shader = CustomColorsShader;
            _coloredShellMaterial = _shellRenderer.material;
            _coloredShellMaterial.shader = CustomColorsShader;

            _armRenderer.material = _coloredArmMaterial;
            _shellRenderer.material = _coloredShellMaterial;
            
            MaterialBlock.SetTexture(IdTex, _mask);
            MaterialBlock.SetTexture(Cube, _cube);
            _shellRenderer.material.SetTexture(IdTex, _shellMask);
            SetPropertyBlock();

            HandPaintConfig.KnuckleblasterShellColorField.onValueChange += OnShellColor;
            HandPaintConfig.KnuckleblasterShellColorField.TriggerValueChangeEvent();
            HandPaintConfig.RepaintKnuckleblaster.TriggerValueChangeEvent();
        }
        
        protected override void RestoreVanillaMaterials()
        {
            if (_armRenderer == null || _shellRenderer == null)
                return;

            _armRenderer.material = _originalArmMaterial;
            _shellRenderer.material = _originalShellMaterial;
        }

        protected override void Repaint()
        {
            if (_armRenderer == null || _shellRenderer == null)
                return;

            _armRenderer.material = _coloredArmMaterial;
            _shellRenderer.material = _coloredShellMaterial;
        }
        
        public void OnShellColor(ColorField.ColorValueChangeEvent e) => _shellRenderer.material.SetColor(ColorProperties[0], e.value);
        
        protected override void SetPropertyBlock() => _armRenderer.SetPropertyBlock(MaterialBlock);

        protected override BoolField EnableRepaintingField() => HandPaintConfig.RepaintKnuckleblaster;
        
        protected override ColorAlphaField[] ColorFields() => HandPaintConfig.KnuckleblasterColors;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandPaintConfig.KnuckleblasterShellColorField.onValueChange -= OnShellColor;
        }
    }
}