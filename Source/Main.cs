using HugsLib.Utils;
using RimWorld;

namespace PodLoadProgress
{
    public class Main : HugsLib.ModBase
    {
        public Main()
        {
            Instance = this;
        }

        internal new ModLogger Logger => base.Logger;

        internal static Main Instance { get; private set; }

        public override string ModIdentifier => "kathanon.PodLoadProgress";

        internal LoadProgressManager Progress { get; } = new LoadProgressManager();

        public override void DefsLoaded()
        {
            // TODO: load settings
        }
    }

    static class ExtensionMethods
    {
        public static bool GroupIsLoading(this CompTransporter pod) =>
            pod.LoadingInProgressOrReadyToLaunch && pod.AnyInGroupHasAnythingLeftToLoad;
    }
}