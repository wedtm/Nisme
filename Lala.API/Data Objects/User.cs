using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lala.API.Data_Objects
{
    class User
    {
        public String Username { get; set; }
        public String UserID { get; set; }
        public String EmailAddress { get; set; }
        public Image UserImage { get; set; }
        public Library Library { get; set; }
    }
}
