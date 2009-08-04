using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace Lala.API
{
    [Serializable]
    public class Song
    {
        #region Public Members
        public String Genre { get; set; }
        public Boolean IsDownloadAlbumOnly { get; set; }
        public Boolean IsLicensedForDownload { get; set; }
        public Int64 LinkDiscLalaId { get; set; } // can this be safely set to UInt64?
        public Boolean IsForecast { get; set; }
        public Boolean IsMissingFile { get; set; }
        public Boolean IsUploaded { get; set; }
        public Boolean IsDownloadPurchased { get; set; }
        public String PlayType { get; set; } // Check for enum's
        public UInt16 Listens { get; set; }
        public Int64 SongLalaId { get; set; }
        public String Timestamp { get; set; }
        public String SyncUrl { get; set; }
        public Int64 Disc { get; set; }
        public Int64 ID { get; set; }
        public Int16 AddPrice { get; set; }
        public String Title { get; set; }
        public UInt16 ListensRank { get; set; }
        public Int64 DiscLalaId { get; set; }
        public String DiscTitle { get; set; }
        public UInt16 Year { get; set; }
        public String Artist { get; set; }
        public String ArtistList { get; set; } // set modifers for get and set on this one format will be one::two::three (?)
        public UInt32 Listens7Rank { get; set; }
        public Boolean IsDigied { get; set; }
        public Int64 ListensTrend { get; set; } // WTF is this for?
        public Boolean IsLicensedForStreaming { get; set; }
        public DateTime LastListenTime { get; set; }
        public UInt16 DownloadPriceInCents { get; set; }
        public UInt16 TrackNumber { get; set; }
        public UInt32 Listens7 { get; set; }
        public Boolean IsQuasiUploaded { get; set; }
        public String DownloadQuality { get; set; }
        public Int64 GroupId { get; set; }
        public Int32 Duration { get; set; }
        public String PlayToken { get; set; }
        public UInt64 LinkSongLalaId { get; set; }
        public String SyncToken { get; set; }
        public Boolean IsFreeStreamingUpload { get; set; }
        public String SyncType { get; set; }
        public Boolean IsUserDownloadable { get; set; }
        public Boolean CanUserPurchaseDownload { get; set; }
        public String AlbumImage
        {
            get
            {
                if (this.DiscLalaId != 0)
                {
                    string albumID = this.DiscLalaId.ToString();
                    return "http://album-images.lala.com/servlet/ArtWorkServlet/" + albumID + "/s";
                }
                return "http://album-images.lala.com/servlet/ArtWorkServlet/72339069014638592/s";
            }
        }
        public String PlayLink
        {
            get
            {
                string hash = this.PlayToken + "Warning: Unauthorized reproduction, capture or distribution of this stream can result in civil and criminal liability. This stream and its content is owned by la la media, inc. and/or its licensors, and is protected by applicable intellectual property and other laws, including but not limited to copyright.   To prevent unauthorized and infringing uses of this stream, generation of an MD5 digest with this text is required and will be used for monitoring and tracking unauthorized and infringing activity." + "abs123";
                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(hash);
                data = x.ComputeHash(data);
                string ret = "";
                for (int i = 0; i < data.Length; i++)
                    ret += data[i].ToString("x2").ToLower();
                string md5t = ret;
                string T = this.PlayToken;
                T = T.Replace("-", "%2D").Replace("=", "%3D").Replace("+", "%2B").Replace("/", "%2F");
                string TS = this.Timestamp;
                string URL = "http://www.lala.com/api/Player/getPlaybackUrl?T=" + T + "&webSrc=lala&md5T=" + md5t + "&flash=true&TS=" + TS + "&debugPagePath=bigdPlayer&xml=true";
                string xml = API.HTTPRequests.GetData(URL);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xml);
                XmlNodeList result = xdoc.GetElementsByTagName("url");
                return result[0].InnerText;
            }
        }
        public String DisplayableDuration
        {
            get
            {
                return API.Functions.SecondsToTime(Duration, true);
            }
        }
        #endregion
    }
}
