using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PodLoadProgress
{
    internal class LoadProgress 
    { 
        public int Loaded;
        public int Total;
        public int PodsLoaded;
        public int PodsTotal;

        public LoadProgress() 
        {
            Loaded = 0;
            Total = 0;
            PodsLoaded = 0;
            PodsTotal = 0;
        }

        public LoadProgress(int Loaded, int Total)
        {
            this.Loaded = Loaded;
            this.Total = Total;
            this.PodsLoaded = (Loaded == Total) ? 1 : 0;
            this.PodsTotal = 1;
        }
        static internal LoadProgress FindProgress(CompTransporter pod)
        {
            int loaded = 0;
            int total = 0;

            var loadedThings = new Dictionary<ThingDef, int>();
            foreach (Thing t in pod.innerContainer)
            {
                if (loadedThings.ContainsKey(t.def))
                {
                    loadedThings[t.def] += t.stackCount;
                }
                else
                {
                    loadedThings[t.def] = t.stackCount;
                }
            }

            foreach (var tr in pod.leftToLoad)
            {
                ThingDef td = tr.ThingDef;
                int stack = DefaultCarryStack(td);
                float amount = 0;
                if (loadedThings.ContainsKey(td))
                {
                    amount = loadedThings[td];
                    int add = Mathf.CeilToInt(amount / stack);
                    loaded += add;
                    loadedThings.Remove(td);
                }
                amount += tr.CountToTransfer;
                int add2 = Mathf.CeilToInt(amount / stack);
                total += Mathf.CeilToInt(amount / stack);
            }

            foreach (var pair in loadedThings)
            {
                int stacks = Mathf.CeilToInt(((float)pair.Value) / DefaultCarryStack(pair.Key));
                loaded += stacks;
                total += stacks;
            }

            return new LoadProgress(loaded, total);
        }

        static internal int DefaultCarryStack(ThingDef td) => Mathf.Min(td.stackLimit, Mathf.RoundToInt(75 / td.VolumePerUnit));

        public override string ToString()
        {
            return $"{(int) (((float) Loaded) / Total * 100f)}%";
        }

        public void Add(LoadProgress add)
        {
            Loaded += add.Loaded;
            Total += add.Total;
            PodsLoaded += add.PodsLoaded;
            PodsTotal += add.PodsTotal;
        }

        static internal LoadProgress FindGroupProgress(CompTransporter pod)
        {
            LoadProgress progress = new LoadProgress();
            foreach (CompTransporter other in pod.TransportersInGroup(pod.Map))
            {
                progress.Add(LoadProgress.FindProgress(other));
            }
            return progress;
        }
    }
}
