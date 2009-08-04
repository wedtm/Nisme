using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    [Serializable]
    public class Histo
    {
        #region Public Members
        public UInt32 Count { get; set; }
        public Boolean IsMatched { get; set; }
        public String Name { get; set; }
        public UInt32 ListenCount { get; set; }
        #endregion
    }
}
