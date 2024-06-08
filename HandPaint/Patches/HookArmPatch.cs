using HandPaint.Components;
using HarmonyLib;

namespace HandPaint.Patches
{
    [HarmonyPatch(typeof(HookArm))]
    public class HookArmPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HookArm), "Start")]
        public static void HookArm_Start_Postfix(HookArm __instance)
        {
            __instance.gameObject.AddComponent<ColoredWhiplash>();
        }
    }
}