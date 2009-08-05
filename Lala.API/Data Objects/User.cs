using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lala.API
{
    [Serializable]
    public class User
    {
        public String Username { get; set; }
        public String UserID { get; set; }
        public String EmailAddress { get; set; }
        public Image UserImage { get; set; }
        public Library Library { get; set; }
        public List<Song> Queue { get; set; }
        public User()
        {
            Username = String.Empty;
            UserID = String.Empty;
            Library = new Library();
            Queue = new List<Song>();
        }
    }
}
