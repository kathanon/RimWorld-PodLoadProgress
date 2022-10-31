using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace PodLoadProgress
{
    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.CompInspectStringExtra))]
    public static class CompTransporter_InspectString_Patch
    {
        public static string Postfix(string original, CompTransporter __instance)
        {
            if (__instance.GroupIsLoading())
            {
                var addition = __instance.LoadingString();
                return  (original.Length > 0) ? $"{addition}\n{original}" : addition;
            }
            else
            {
                return original;
            }
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingAdded))]
    public static class CompTransporter_ThingAdded_Patch
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingAddedAndMergedWith))]
    public static class CompTransporter_ThingAddedAndMerged_Patch
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.Notify_ThingRemoved))]
    public static class CompTransporter_ThingRemoved_Patch
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }

    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.CancelLoad), new Type[] { typeof(Map) })]
    public static class CompTransporter_CancelLoad_Patch
    {
        public static void Postfix(CompTransporter __instance)
        {
            Main.Instance.Progress.Update(__instance);
        }
    }
}
