using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PodLoadProgress
{
    public static class Strings
    {
        public static readonly string ID     = "kathanon.PodLoadProgress";
        public static readonly string PREFIX = ID + ".";

        public static readonly string Eta               = (PREFIX + "ETA"              ).Translate();
        public static readonly string Progress          = (PREFIX + "progress"         ).Translate();
        public static readonly string ShowSeconds_title = (PREFIX + "showSeconds.title").Translate();
        public static readonly string ShowSeconds_desc  = (PREFIX + "showSeconds.desc" ).Translate();
    }
}
