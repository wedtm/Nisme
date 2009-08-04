using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;

namespace Lala.API
{
    public class Images
    {
        public static Image AlbumImage(Song track, string size)
        {
            if (track.DiscLalaId != 0)
            {
                string albumID = track.DiscLalaId.ToString();
                if (!Directory.Exists("AlbumArt"))
                    Directory.CreateDirectory("AlbumArt");
                if (!File.Exists("AlbumArt/" + albumID + "_" + size + ".jpg"))
                {
                    string URL = "http://album-images.lala.com/servlet/ArtWorkServlet/" + albumID + "/" + size;
                    Stream dataStream = API.HTTPRequests.GetDataStream(URL);
                    Image image = Image.FromStream(dataStream, true, true);
                    dataStream.Close();
                    image.Save("AlbumArt/" + albumID + "_" + size + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    return image;
                }
                else
                {
                    return Image.FromFile("AlbumArt/" + albumID + "_" + size + ".jpg", true);
                }

            }
            return null;
        }

        public static Image AlbumImage(long AlbumID, string size)
        {
            if (!Directory.Exists("AlbumArt"))
                Directory.CreateDirectory("AlbumArt");
            if (!File.Exists("AlbumArt/" + AlbumID.ToString() + "_" + size + ".jpg"))
            {
                string URL = "http://album-images.lala.com/servlet/ArtWorkServlet/" + AlbumID.ToString() + "/" + size;
                Stream dataStream = API.HTTPRequests.GetDataStream(URL);
                Image image = Image.FromStream(dataStream, true, true);
                dataStream.Close();
                image.Save("AlbumArt/" + AlbumID.ToString() + "_" + size + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                return image;
            }
            else
            {
                return Image.FromFile("AlbumArt/" + AlbumID.ToString() + "_" + size + ".jpg", true);
            }
        }
    }
}
