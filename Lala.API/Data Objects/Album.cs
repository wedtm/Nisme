using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    [Serializable]
    public class Album
    {
        #region Public Members
        public Boolean Genre { get; set; }
        public Boolean IsLicensedForDownload { get; set; }
        public UInt16 AddMissingTracksCount { get; set; }
        public Boolean CanBuy { get; set; }
        public UInt16 Adds { get; set; }
        public UInt32 Listens { get; set; }
        public String Type { get; set; } //This should be an enum...
        public Boolean Wanted { get; set; }
        public UInt16 AlbumDownloadableTrackCount { get; set; }
        public UInt64 AlbumDuration { get; set; }
        public Int64 ID { get; set; }
        public Boolean IsDeprecated { get; set; }
        public UInt32 WebAlbumPriceInCents { get; set; }
        public String Title { get; set; }
        public UInt16 DownloadMissingTracksCount { get; set; }
        public DateTime ReleaseDate { get; set; }
        public UInt32 ListenRank { get; set; }
        public UInt16 AddMissingTracksPriceInCents { get; set; }
        public UInt16 DiscNo { get; set; }
        public String SubGenre { get; set; }
        public UInt16 Year { get; set; }
        public String Artist { get; set; }
        public String[] ArtistList { get; set; }
        public UInt32 Listens7Rank { get; set; }
        public UInt16 TrackCount { get; set; }
        public UInt16 NbDisc { get; set; }
        public UInt32 ListensTrend { get; set; }
        public Boolean IsDigied { get; set; }
        public UInt32 DownloadAlbumPriceInCents { get; set; }
        public Boolean IsLicensedForStreaming { get; set; }
        public String Label { get; set; }
        public UInt32 Listens7 { get; set; }
        public String[] Tracks { get; set; } // Need to id what this actually returns.
        public UInt64 Upc { get; set; }
        public Boolean Samples { get; set; }
        public Boolean IsAllTracksDigied { get; set; }
        public Boolean Owned { get; set; }
        public String DownloadQuality { get; set; }
        public Boolean IsAllTracksDownloadPurchased { get; set; }
        public Int64 GroupID { get; set; }
        public UInt16 AlbumTrackCount { get; set; }
        public String Price { get; set; }
        public Boolean IsDigital { get; set; }
        public UInt32 DownloadMissingTracksPriceInCents { get; set; }
        public String[] BundleIds { get; set; }
        #endregion
        public List<Song> Songs; // Must be init'ed first.
    }
}
