using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    public static class Instance
    {
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
        public static User CurrentUser { get; set; }
    }
}
