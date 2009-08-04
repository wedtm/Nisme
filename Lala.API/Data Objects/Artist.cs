using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    [Serializable]
    public class Artist
    {
        #region Public Members
        public Boolean IsLicensedForDownload { get; set; }
        public UInt32 ListensTrend { get; set; }
        public Int64 RadioLalaID { get; set; }
        public Int64 LalaID { get; set; }
        public Boolean HasArtistRadio { get; set; }
        public String ImgSource { get; set; }
        public String ImgAuthor { get; set; }
        public UInt32 LicensedAlbumCount { get; set; }
        public UInt32 Listens7 { get; set; }
        public UInt32 Listens { get; set; }
        public String Extension { get; set; }
        public UInt32 ListensRank { get; set; }
        public String TopSong { get; set; } // Not sure if this is right...
        public UInt16 LicensedTrackCount { get; set; }
        public String ArtistName { get; set; } //Modified from XML due to .NET Limitation. Original = "Artist"
        public String ImgBasePath { get; set; }
        public Boolean IsImageCreativeCommons { get; set; }
        public Boolean IsLicensed { get; set; }
        public UInt32 Listens7Rank { get; set; }
        public String ImgTitle { get; set; }
        #endregion

        public List<Album> Albums; // must be init'ed with initial value.
    }
}