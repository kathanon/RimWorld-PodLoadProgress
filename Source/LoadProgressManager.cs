using RimWorld;
using System.Collections.Generic;

namespace PodLoadProgress
{
    internal class LoadProgressManager
    {
        public readonly Dictionary<CompTransporter, LoadProgressBuffer> Buffers
            = new Dictionary<CompTransporter, LoadProgressBuffer>();

        public void Update(CompTransporter pod) {
            bool exists = Buffers.ContainsKey(pod);
            bool loading = pod.GroupIsLoading();

            if (loading)
            {
                if (exists)
                {
                    UpdateGroup(pod);
                }
                else
                {
                    Add(pod);
                }
            }
            else
            {
                if (exists)
                {
                    Remove(pod);
                }
                // If it does not exist and we are not loading, all good
            }
        }

        public LoadProgressBuffer Get(CompTransporter pod)
        {
            bool exists = Buffers.ContainsKey(pod);
            bool loading = pod.GroupIsLoading();

            if (loading) {
                if (!exists || !Buffers[pod].HasLast)
                {
                    Update(pod);
                }
                return Buffers[pod];
            } 
            else
            {
                if (exists)
                {
                    var buf = Buffers[pod];
                    Remove(pod);
                    return buf;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Remove(CompTransporter pod)
        {
            if (Buffers.ContainsKey(pod)) 
            {
                foreach (CompTransporter other in pod.TransportersInGroup(pod.Map))
                {
                    Buffers.Remove(other);
                }
            }
        }

        private void Add(CompTransporter pod)
        {
            var buffer = new LoadProgressBuffer();
            buffer.Update(pod);
            foreach (CompTransporter other in pod.TransportersInGroup(pod.Map))
            {
                Buffers[other] = buffer;
            }
        }

        public void UpdateGroup(CompTransporter pod)
        {
            var buffer = Buffers[pod];
            buffer.Update(pod);
            foreach (CompTransporter other in pod.TransportersInGroup(pod.Map))
            {
                if (!Buffers.ContainsKey(other))
                {
                    Buffers[other] = buffer;
                }
            }
        }
    }
}
