using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lala.API;

namespace Vimae
{
    public partial class Player
    {
            public void AddSongToQueue(Song song)
            {
                Lala.API.Instance.CurrentUser.Queue.Add(song);
                QueueModified(this, new EventArgs());
            }

            public void RemoveSongFromQueue(int Position)
            {
                Lala.API.Instance.CurrentUser.Queue.RemoveAt(0);
                QueueModified(this, new EventArgs());
            }
    }
}