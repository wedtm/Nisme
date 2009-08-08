using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Lala.API
{
    [Serializable]
    public class User
    {

        public const int FILE_VERSION = 3; // Change this with each and every change to the User Object structure // = WedTM
        public String Username { get; set; }
        public String UserID { get; set; }
        public String EmailAddress { get; set; }
        public Image UserImage { get; set; }
        public Library Library { get; set; }
        public List<Song> Queue { get; set; }
        public List<Song> Listened { get; set; }
        public Hashtable Settings { get; set; }
        public ulong TotalCount { get; set; }
        public User()
        {
            JObject info = HTTPRequests.GetUserInfo();
            Username = (string)info["nickName"];
            UserID = (string)info["userToken"];
            EmailAddress = (string)info["email"];
            TotalCount = API.Functions.GetTrackCount();
            Library = new Library();
            Queue = new List<Song>();
            Listened = new List<Song>();
        }
    }
}
