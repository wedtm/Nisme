using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimae
{
    public partial class Player
    {
        public class Instance
        {
            /// <summary>
            /// Bass identifies the channel by a unique ID, this is it.
            /// </summary>
            public int Channel { get; set; }

            /// <summary>
            /// The length of time between each tick. In milliseconds
            /// </summary>
            public int ProgressTick { get; set; }

            public class CurrentSong
            {
                public static double TotalLength { get; set; }
                public static double CurrentPosition { get; set; }
                public static double TotalLengthInSeconds { get; set; }
                public static double CurrentPositionInSeconds { get; set; }
                public static Lala.API.Song Song { get; set; }
            }
        }
    }
}