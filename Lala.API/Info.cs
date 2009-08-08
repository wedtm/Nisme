using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    public static class Info
    {
        public static Version Version = new Version(2, 0, 0, Convert.ToInt32(DateTime.Now.Ticks));
    }
}
