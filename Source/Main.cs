using HugsLib.Settings;
using HugsLib.Utils;

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

        public override string ModIdentifier => Strings.ID;

        internal LoadProgressManager Progress { get; } = new LoadProgressManager();

        public SettingHandle<bool> showSeconds;

        public override void DefsLoaded()
        {
            showSeconds = Settings.GetHandle<bool>("showSeconds", Strings.ShowSeconds_title, Strings.ShowSeconds_desc, false);
        }
    }
}