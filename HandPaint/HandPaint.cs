using BepInEx;
using HarmonyLib;

namespace HandPaint

{
    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class HandPaint : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            AssetsManager.Instance.LoadAssets();
            AssetsManager.Instance.RegisterPrefabs();

            _harmony = new Harmony(PluginInfo.GUID);
            ConfigFields.Init();
            _harmony.PatchAll();
        }
    }
}