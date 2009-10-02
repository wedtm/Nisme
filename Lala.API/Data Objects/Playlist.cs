using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.XPath;
using System.Data;
using Microsoft.Windows.Controls;

namespace Lala.API
{
    [Serializable]
    public class Playlist : CollectionBase
    {
        public override string ToString()
        {
            return Name;
        }
         public String Name { get; set; }
        public String ID { get; set; }
        public Song Playing { get; set; }
        public List<Histo> Histos { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
        public List<Artist> Artists { get; set; }
        private void ConvertFrom(List<Object> item)
        {
            if (item[0].GetType() == typeof(Song))
            {
                List<Song> rList = new List<Song>();
                foreach (Song obj in item)
                    rList.Add(obj);
                Songs = rList;
            }

            if (item[0].GetType() == typeof(Histo))
            {
                List<Histo> rList = new List<Histo>();
                foreach (Histo obj in item)
                    rList.Add(obj);
                Histos = rList;
            }

            if (item[0].GetType() == typeof(Album))
            {
                List<Album> rList = new List<Album>();
                foreach (Album obj in item)
                    rList.Add(obj);
                Albums = rList;
            }

            if (item[0].GetType() == typeof(Artist))
            {
                List<Artist> rList = new List<Artist>();
                foreach (Artist obj in item)
                    rList.Add(obj);
                Artists = rList;
            }
        }
        public Playlist(List<Song> Songs)
        {
            this.Songs = Songs;
        }
        public Playlist(List<Song> Songs, List<Artist> Artists, List<Album> Albums, List<Histo> Histos)
        {
            throw new NotImplementedException("Please use Playlist(string URL) for now");
        }
        public Playlist(string URL, string Name, string ID)
        {
            API.XmlParser parser = new API.XmlParser();
            XPathNavigator nav = parser.XPathNavFromStream(URL);
            List<Object> placeholder = parser.SingleNodeCollection(typeof(Song), XPathStatements.Playlists.Songs, nav);
            ConvertFrom(placeholder);
            this.Name = Name;
            this.ID = ID;
        }
        public Playlist() { }
        public Song CurrentSong { get; set; }
        public Boolean HasMoreSongs
        {
            get
            {
                try
                {
                    int NextIndex = Songs.FindIndex(this.SongByToken);
                    if (Songs[NextIndex + 1] == null)
                        return false;
                    else
                        return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }


        public Song GetNext(Song CurrentSong)
        {

            int CurrentIndex = Songs.FindIndex(SongByToken);
            Song NextSong = Songs[CurrentIndex + 1];
            return NextSong;
        }

        public bool SongByToken(Song s)
        {
            if (s.PlayToken == CurrentSong.PlayToken)
                return true;
            else
                return false;
        }

        public Song GetNext()
        {
            Song NextSong = Songs[0];
            return NextSong;
        }

        public Song GetPrevious(Song CurrentSong)
        {
            int CurrentIndex = Songs.FindIndex(SongByToken);
            Song NextSong = Songs[CurrentIndex - 1];
            return NextSong;
        }

        public Song this[int index]
        {
            get
            {
                return Songs[index];
            }
            set
            {
                Songs[index] = value;
            }
        }
   }
}