using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PodLoadProgress
{
    [HarmonyPatch(typeof(MainTabWindow_Inspect), nameof(MainTabWindow_Inspect.DoInspectPaneButtons))]
    public static class TabInspect_DoButtons_Patch
    {
        public static void Postfix(Rect rect)
        {
            var selection = Find.Selector.SelectedObjects;
            if (selection.Count < 2) return;

            var trans = new List<CompTransporter>();
            foreach (var o in selection)
            {
                var transTemp = (o as ThingWithComps)?.GetComp<CompTransporter>();
                if (transTemp == null) return;
                if (transTemp.GroupIsLoading() && !transTemp.IsInGroupWithAny(trans) && trans.Count < 5)
                {
                    trans.Add(transTemp);
                }
            }

            if (trans.Count > 0)
            {
                var text = trans.Join(t => t.LoadingString(), "\n");
                float y = rect.y += 26f;
                Widgets.Label(rect.x, ref y, rect.width, text);
            }
        }
    }
}
