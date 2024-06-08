using HandPaint.Components;
using HarmonyLib;
using UnityEngine;

namespace HandPaint.Patches
{
    [HarmonyPatch(typeof(GunSetter))]
    public class GunSetterPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GunSetter), "CheckWeapon")]
        public static bool Punch_Awake_Prefix(GunSetter __instance, string name, GameObject[] prefabs)
        {
           if (!name.StartsWith("rev"))
               return true;

           foreach (var prefab in prefabs)
               prefab.transform.GetChild(0).Find("RightArm").gameObject.AddComponent<ColoredFeedbackerR>();
           
           return true;
        }
    }
}