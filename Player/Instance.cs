using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimae
{
    public partial class Player
    {
            /// <summary>
            /// Bass identifies the channel by a unique ID, this is it.
            /// </summary>
        public int Channel = new Int32();

            /// <summary>
            /// The length of time between each tick. In milliseconds
            /// </summary>
        public int ProgressTick = new Int32();

            public class CurrentSong
            {
                public double TotalLength { get; set; }
                public double CurrentPosition { get; set; }
                public double TotalLengthInSeconds { get; set; }
                public double CurrentPositionInSeconds { get; set; }
                public Lala.API.Song Song { get; set; }
                public CurrentSong()
                {
                    this.TotalLength = new Double();
                    this.CurrentPosition = new Double();
                    this.TotalLength = new Double();
                    this.TotalLengthInSeconds = new Double();
                    this.Song = new Lala.API.Song();
                }
            }
        }
    }