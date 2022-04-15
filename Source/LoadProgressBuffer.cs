using RimWorld;
using UnityEngine;
using Verse;

namespace PodLoadProgress
{
    internal class LoadProgressBuffer
    {
        private const int BUFFER_SIZE = 20;
        private const int MIN_BUFFER_ETA = 4;

        private readonly LoadProgress[] progressBuf = new LoadProgress[BUFFER_SIZE];
        private readonly int[] ticksBuf = new int[BUFFER_SIZE];
        private int last = -1;
        private bool full = false;

        public void Update(CompTransporter pod)
        {
            int ticks = Find.TickManager.TicksGame;
            LoadProgress progress = LoadProgress.FindGroupProgress(pod);
            if (last >= 0 && progress.Loaded == progressBuf[last].Loaded)
            {
                return;
            }

            if (last < 0 || ticks > ticksBuf[last]) {
                ++last;
                if (last >= BUFFER_SIZE)
                {
                    last = 0;
                    full = true;
                }
            }

            progressBuf[last] = progress;
            ticksBuf[last] = ticks;
        }

        public bool HasETA => Num >= MIN_BUFFER_ETA;

        public bool HasLast => last >= 0;

        public int Num => full ? BUFFER_SIZE : last + 1;

        public LoadProgress Last { get { return progressBuf[last]; } }

        public int ETAInTicks
        {
            get
            {
                if (!HasETA) { return 0; }

                int first = (!full || last == BUFFER_SIZE - 1) ? 0 : last + 1;
                int workDone = progressBuf[last].Loaded - progressBuf[first].Loaded;
                float time = ticksBuf[last] - ticksBuf[first];
                float rate = time / workDone;
                int workLeft = progressBuf[last].Total - progressBuf[last].Loaded;
                return Mathf.RoundToInt(workLeft * rate);
            }
        }
    }
}
