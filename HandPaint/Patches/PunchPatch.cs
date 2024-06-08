using HandPaint.Components;
using HarmonyLib;
using UnityEngine;

namespace HandPaint.Patches
{
    [HarmonyPatch(typeof(Punch))]
    public class PunchPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Punch), "Awake")]
        public static void Punch_Awake_Postfix(Punch __instance, SkinnedMeshRenderer ___smr)
        {
            if (__instance.name.StartsWith("Arm Red"))
            {
                var coloredKb = __instance.gameObject.AddComponent<ColoredKnuckleblaster>();
                coloredKb.shell = __instance.shell;
            }
            else if (__instance.name.StartsWith("Arm Blue"))
                __instance.gameObject.AddComponent<ColoredFeedbacker>();
            else 
                Debug.LogWarning("GameObject has a Punch component but it's neither blue nor red hand");
        }
    }
}