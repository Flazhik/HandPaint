using HandPaint.Components;
using HarmonyLib;

namespace HandPaint.Patches
{
    [HarmonyPatch(typeof(RevolverAnimationReceiver))]
    public class RevolverAnimationReceiverPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RevolverAnimationReceiver), "Start")]
        public static bool RevolverAnimationReceiver_Start_Prefix(RevolverAnimationReceiver __instance)
        {
            var go = __instance.transform.Find("RightArm").gameObject;
           go.AddComponent<ColoredFeedbackerR>();

           return true;
        }
    }
}