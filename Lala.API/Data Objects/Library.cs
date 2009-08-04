using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    [Serializable]
    public class Library
    {
        public String Owner { get; set; }
        public List<Playlist> Playlists { get; set; }
        public Playlist Playing { get; set; }
        public Playlist Displayed { get; set; }
        public Library()
        {
            Owner = String.Empty;
            Playlists = new List<Playlist>();
            Playing = null;
            Displayed = null;
        }
    }
}
