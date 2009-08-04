using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Lala.API
{
    public static class HTTPRequests
    {

        public static string GetData(string URL)
        {
            WebRequest request = WebRequest.Create(URL);
            request.Headers.Add("Cookie:" + API.Constants.Cookie);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public static Stream GetDataStream(string URL)
        {
            WebRequest request = WebRequest.Create(URL);
            request.Headers.Add("Cookie:" + API.Constants.Cookie);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            return dataStream;
        }


        public static string GetLibrary(ulong Number, long Offset)
        {
            string URL = "http://www.lala.com/api/Playlists/getOwnSongs/" + API.Functions.CurrentLalaVersion() + "?playlistToken=songs&includeHistos=false&count=" + Number.ToString() + "&skip=" + Offset.ToString() + "&sortKey=Artist&sortDir=Asc&webSrc=nisme&xml=true";
            return URL;
        }

        public static bool GetLoginCookie(string username, string password)
        {
            string URL = "https://www.lala.com/api/User/signin/" + API.Functions.CurrentLalaVersion() + "?email=" + username + "&userpwd=" + password + "&xml=true";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string plate = "uiFc=homeNoAuthBSignin; ";
            foreach (Cookie cook in response.Cookies)
            {
                plate = plate + cook.ToString() + "; ";
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(responseFromServer);
            XmlNodeList id = xDoc.GetElementsByTagName("userToken");
            XmlNodeList err = xDoc.GetElementsByTagName("errorCode");
            string UserID = id[0].InnerText;
            string ErrorCode = err[0].InnerText;
            if (ErrorCode != String.Empty)
            {
                return false;
            }
            else
            {
                API.Constants.Cookie = plate;
                API.Constants.UserID = UserID;
                return true;
            }
        }

        public static string GetHistos()
        {
            string URL = "http://www.lala.com/api/Playlists/getOwnSongs/" + API.Functions.CurrentLalaVersion() + "?playlistToken=songs&includeHistos=true&count=0&skip=0&sortKey=Artist&sortDir=Asc&webSrc=nisme&counterIds=f.client.byPage.Home.click.link.leftNav.AllSongs%2Cf.click.fromLeftNav.Home";
            return GetData(URL);
        }

        public static JObject GetUserInfo()
        {
            string URL = "http://www.lala.com/";
            string Data = GetData(URL);
            string starterString = "g_pageData = ";
            int start = Data.IndexOf(starterString) + starterString.Length;
            int end = Data.IndexOf("</script>", start);
            string json = Data.Substring(start, end - start);
            return JObject.Parse(json);
        }

        public static Hashtable GetPlaylists()
        {
            JObject o = GetUserInfo();
            JArray pls = (JArray)o["playlists"];
            Hashtable ht = new Hashtable();
            foreach (JObject item in pls)
            {
                ht.Add((string)item["id"], (string)item["title"]);
            }
            return ht;
        }

        public static string GetNewestVersion()
        {
            try
            {
                string URL = "http://www.vimae.com/nismeversion.txt";
                return GetData(URL);
            }
            catch (Exception)
            {
                return API.Constants.VERSION;
            }
        }
    }
}
