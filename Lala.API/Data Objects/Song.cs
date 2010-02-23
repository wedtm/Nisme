using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace Lala.API
{
    [Serializable]
    public class Song
    {
        #region Public Members
        public String Genre { get; set; }
        public Int64 LinkDiscLalaId { get; set; } // can this be safely set to UInt64?
        public UInt64 Listens { get; set; }
        public Int64 SongLalaId { get; set; }
        public String Timestamp { get; set; }
        public Int64 ID { get; set; }
        public String Title { get; set; }
        public Int64 DiscLalaId { get; set; }
        public String DiscTitle { get; set; }
        public UInt32 Year { get; set; }
        public String Artist { get; set; }
        public String ArtistList { get; set; } // set modifers for get and set on this one format will be one::two::three (?)
        public UInt32 TrackNumber { get; set; }
        public UInt32 Duration { get; set; }
        public String PlayToken { get; set; }
        public UInt64 LinkSongLalaId { get; set; }
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

        public Song(JObject song)
        {

            this.Genre = (String)song["genre"];
            this.LinkDiscLalaId = Convert.ToInt64((String)song["linkDiscLalaId"]);
            this.Listens = Convert.ToUInt64(song["listens"].ToString());
            this.SongLalaId = Convert.ToInt64((String)song["songLalaId"]);
            this.Timestamp = (String)song["timestamp"];
            this.ID = Convert.ToInt64((String)song["id"]);
            this.Title = (String)song["title"];
            this.DiscLalaId = Convert.ToInt64((String)song["discLalaId"]);
            this.DiscTitle = (String)song["discTitle"];
            if((String)song["year"] != String.Empty)
                this.Year = Convert.ToUInt32((String)song["year"]);
            this.Artist = (String)song["artist"];
            this.ArtistList = (String)song["artistList"];
            this.TrackNumber = Convert.ToUInt32((Int32)song["trackNumber"]);
            this.Duration = Convert.ToUInt32((Int32)song["duration"]);
            this.PlayToken = (String)song["playToken"];
            this.LinkSongLalaId = Convert.ToUInt64((String)song["linkSongLalaId"]);
        }
        #endregion
    }
}
