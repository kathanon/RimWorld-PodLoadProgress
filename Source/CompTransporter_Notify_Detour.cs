using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace PodLoadProgress
{
    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingAdded))]
    public static class CompTransporter_ThingAdded_Detour
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingAddedAndMergedWith))]
    public static class CompTransporter_ThingAddedAndMerged_Detour
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingRemoved))]
    public static class CompTransporter_ThingRemoved_Detour
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.CancelLoad), new Type[] { typeof(Map) })]
    public static class CompTransporter_CancelLoad_Detour
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }
}
