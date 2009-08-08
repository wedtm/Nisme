using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimae
{
    public partial class Player
    {
        public event EventHandler Played;
        public event EventHandler Stopped;
        public event EventHandler Resumed;
        public event EventHandler QueueModified;

    }
}