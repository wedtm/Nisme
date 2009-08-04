using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lala.API
{
    public class Functions
    {

        public static String GetMyDocumentsDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        /// <summary>
        /// Saves the library in the users Document directory as LalaUserID.nlf
        /// </summary>
        /// <param name="library">Library Object to save</param>
        public static void SaveLibrary(API.Library library)
        {
            FileStream fs = new FileStream
                           ( GetMyDocumentsDir() + "\\" + API.Constants.UserID + ".nlf", FileMode.OpenOrCreate, FileAccess.Write);

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, library);
            }
            finally
            {
                fs.Close();
            }
        }

        public static Library OpenLibrary(ulong tracks, bool force)
        {
            string filename = GetMyDocumentsDir() + "\\" + API.Constants.UserID + ".nlf";
            if (File.Exists(filename) && force == false)
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                API.Library myLibrary = (API.Library)bf.Deserialize(fs);
                fs.Close();
                return myLibrary;
            }
            else
            {
                Library myLibrary = new Library();//new API.Library(API.HTTPRequests.GetLibrary(tracks, 0));
                myLibrary.Playlists.Add(new Playlist(API.HTTPRequests.GetLibrary(tracks, 0), "My Collection", "songs"));
                Hashtable pls = HTTPRequests.GetPlaylists();
                IDictionaryEnumerator en = pls.GetEnumerator();
                while (en.MoveNext())
                {
                    string URL = "http://www.lala.com/api/Playlists/getOwnSongs/" + API.Functions.CurrentLalaVersion() + "?playlistToken=" + en.Key.ToString() + "&includeHistos=false&count=50&skip=0&sortKey=Offset&sortDir=Asc&webSrc=nisme&xml=true";
                    myLibrary.Playlists.Add(new Playlist(URL, en.Value.ToString(), en.Key.ToString()));
                }
                SaveLibrary(myLibrary);
                return myLibrary;
            }
        }

        public static string CurrentLalaVersion()
        {
            string URL = "http://www.lala.com/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            string startStr = "lala.buildVersion =";
            int start = responseFromServer.IndexOf(startStr) + startStr.Length + 2; //The plus two is for the leading apostrophe and space.
            int end = responseFromServer.IndexOf("';\r\n", start);
            string sub = responseFromServer.Substring(start, end - start);
            return "v" + sub;
        }

        public static string LalaUserId()
        {
            string URL = "http://www.lala.com/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            string startStr = "lala.buildVersion =";
            int start = responseFromServer.IndexOf(startStr) + startStr.Length + 2; //The plus two is for the leading apostrophe and space.
            int end = responseFromServer.IndexOf("';\r\n", start);
            string sub = responseFromServer.Substring(start, end - start);
            return "v" + sub;
        }

        public static string SecondsToTime(double time, Boolean? leadingZero)
        {
            int s = (int)time;
            int m = 0;
            int h = 0;
            while ((int)s >= 60)
            {
                m++;
                if (m == 60)
                {
                    h++;
                    m = 0;
                }
                s -= 60;
            }
            string result = null;
            if (h == 0)
                result = m.ToString("D2") + ":" + s.ToString("D2");
            else
                result = h.ToString() + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            if (result.StartsWith("0"))
                result = result.Substring(1);
            return result;
        }

        public static ulong SetTrackCount()
        {
            string data = API.HTTPRequests.GetHistos();
            string startString = "\"total\": ";
            int start = data.IndexOf(startString);
            int end = data.IndexOf(",\n", start);
            string count = data.Substring(start + startString.Length, end - start - startString.Length);
            return Convert.ToUInt64(count);
        }
    }
}
