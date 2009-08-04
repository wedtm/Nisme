using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    public sealed class XPathStatements
    {
        public sealed class Playlists
        {
            public static readonly string Histos = "/response/item/histos/item/genre/*";
            public static readonly string Songs = "/response/item/tracks/item/list/*";
            public static readonly string Artists = "/response/item/artists/item/list/*";
            public static readonly string Albums = "/response/item/albums/item/list/*";
        }
        
    }
}
