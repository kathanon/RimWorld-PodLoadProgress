using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace PodLoadProgress
{
    [HarmonyPatch(typeof(CompTransporter), nameof(CompTransporter.CompInspectStringExtra))]
    public static class CompTransporter_InspectString_Detour
    {
        private const int MIN_ETA = 500; // 0.2 h

        private enum State { Idle, Loading, Waiting, Done }

        static string Postfix(string original, CompTransporter __instance)
        {
            if (__instance.GroupIsLoading())
            {
                return Loading(__instance) + original;
            }
            else
            {
                return original;
            }
        }

        private static string Loading(CompTransporter pod)
        {
            LoadProgressBuffer buf = Main.Instance.Progress.Get(pod);
            if (buf == null) { return ""; }
            if (buf.HasETA)
            {
                var ticks = buf.ETAInTicks;
                var eta = Mathf.Max(ticks, MIN_ETA).ToStringTicksToPeriod();
                return $"Progress: {buf.Last}, ETA {eta} ({ticks} t)\n";
            }
            else
            {
                return $"Progress: {buf.Last}\n";
            }
        }
    }
}
