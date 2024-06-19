using BepInEx;
using HandPaint.Components;
using HarmonyLib;
using UnityEngine;

namespace HandPaint

{
    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class HandPaint : BaseUnityPlugin
    {
        private Harmony _harmony;
        private SkittlesPox _easterEgg;

        private void Awake()
        {
            AssetsManager.Instance.LoadAssets();
            AssetsManager.Instance.RegisterPrefabs();
            
            _easterEgg = SkittlesPox.Instance;
            _harmony = new Harmony(PluginInfo.GUID);
            HandPaintConfig.Init();
            _harmony.PatchAll();
        }
    }
}