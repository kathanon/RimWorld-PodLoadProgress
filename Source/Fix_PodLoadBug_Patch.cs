using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace PodLoadProgress
{
    [HarmonyPatch(typeof(Pawn_CarryTracker), nameof(Pawn_CarryTracker.TryStartCarry), new Type[] { typeof(Thing), typeof(int), typeof(bool) })]
    public static class Fix_PodLoadBug_Patch
    {
        public static void Postfix(int __result, Pawn_CarryTracker __instance, Thing item, int count, bool reserve)
        {
            {
                Thing carried = __instance.CarriedThing;
                if (__result > 0 && item != carried && carried != null)
                {
                    Pawn pawn = __instance.pawn;
                    if (pawn.CurJobDef == JobDefOf.HaulToTransporter)
                    {
                        var trans = FindTransferable(pawn, carried.def);
                        if (trans != null)
                        {
                            if (!trans.things.Contains(carried))
                            {
                                trans.things.Add(carried);
                            }
                        }
                    }
                }
            }
        }

        public static TransferableOneWay FindTransferable(Pawn pawn, ThingDef def)
        {
            CompTransporter tr = ((JobDriver_HaulToTransporter) pawn.jobs.curDriver).Transporter;
            foreach (var t in tr.leftToLoad)
            {
                if (t.HasAnyThing && t.ThingDef == def)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
