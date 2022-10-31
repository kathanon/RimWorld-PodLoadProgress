using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PodLoadProgress
{
    static class CompTransporter_Extension
    {
        private const int MIN_ETA = 250; // 0.1 h
        private const int TICKS_PER_SEC = 60;

        public static bool GroupIsLoading(this CompTransporter pod) =>
            pod.LoadingInProgressOrReadyToLaunch && pod.AnyInGroupHasAnythingLeftToLoad;

        public static string LoadingString(this CompTransporter pod)
        {
            LoadProgressBuffer buf = Main.Instance.Progress.Get(pod);
            if (buf == null) { return ""; }
            if (buf.HasETA)
            {
                var ticks = buf.ETAInTicks;
                var eta = Math.Max(ticks, MIN_ETA).ToStringTicksToPeriod(false);
                var etaSec = "";
                if (Main.Instance.showSeconds)
                {
                    var etaInSec = Mathf.RoundToInt(ticks / TICKS_PER_SEC);
                    etaSec = $" ({etaInSec} {"LetterSecond".Translate()})";
                }
                return $"{Strings.Progress} {buf.Last}, {Strings.Eta} {eta}{etaSec}";
            }
            else
            {
                return $"{Strings.Progress} {buf.Last}";
            }
        }

        public static bool IsInGroupWithAny(this CompTransporter pod, List<CompTransporter> list)
        {
            var group = pod.TransportersInGroup(pod.parent.Map);
            foreach (var other in list ?? default)
            {
                if (group.Contains(other)) return true;
            }
            return false;
        }
    }
}