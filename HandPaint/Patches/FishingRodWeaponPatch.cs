using HandPaint.Components;
using HarmonyLib;

namespace HandPaint.Patches
{
    [HarmonyPatch(typeof(FishingRodWeapon))]
    public class FishingRodWeaponPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FishingRodWeapon), "Awake")]
        public static bool FishingRodWeapon_Awake_Prefix(FishingRodWeapon __instance)
        {
            var arm = __instance.GetComponentInChildren<FishingRodAnimEvents>().transform.Find("RightArm");
            arm.gameObject.AddComponent<ColoredFeedbackerR>();
            return true;
        }
    }
}