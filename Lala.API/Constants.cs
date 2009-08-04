using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    public static class Constants
    {
        public static String UserID { get; set; }
        internal static String pCookie;
        public static String Cookie
        {
            get
            {
                return pCookie.Replace("\n", "").Replace("\r", "");
            }
            set
            {
                pCookie = value;
            }
        }
        public const double PREV_TRACK_RESET_DELAY = 2; // 2 seconds in, and it restarts the current track
        public static long TRACK_COUNT { get; set; }
        public const string VERSION = "0.5b";
    }
}
