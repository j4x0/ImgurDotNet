using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImgurDotNet
{
    public class ImgurImage
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Caption { get; private set; }
        public string Hash { get; private set; }
        public string DeleteHash { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Type { get; private set; }
        public bool Animated { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Size { get; private set; }
        public int Views { get; private set; }
        public int Bandwith { get; private set; }

        public Uri OriginalUrl { get; private set; }
        public Uri ImgurUrl { get; private set; }
        public Uri DeleteUrl { get; private set; }
        public Uri SmallSquareUrl { get; private set; }
        public Uri LargeThumbnail { get; private set; }

        public override string ToString()
        {
            return this.ImgurUrl.ToString();
        }

        public static ImgurImage Create(IDictionary<string, object> data)
        {
            var image = (IDictionary<string, object>)data["image"];
            var links = (IDictionary<string, object>)data["links"];
            string imgName = image.ContainsKey("name") ? (string)image["name"] :null;
            string deleteHash = image.ContainsKey("deletehash") ? (string)image["deletehash"] : null;
            Uri deleteUrl = image.ContainsKey("delete_page") ? new Uri((string)image["delete_page"]) : null;
            return new ImgurImage
            {
                Name = imgName,
                Title = (string)image["title"],
                Caption = (string)image["caption"],
                Hash = (string)image["hash"],
                DeleteHash = deleteHash,
                DateTime = DateTime.Parse((string)image["datetime"]),
                Type = (string)image["type"],
                Animated = Convert.ToBoolean(image["animated"]),
                Height = Convert.ToInt32(image["height"]),
                Width = Convert.ToInt32(image["width"]),
                Size = Convert.ToInt32(image["size"]),
                Views = Convert.ToInt32(image["views"]),
                Bandwith = Convert.ToInt32(image["bandwidth"]),
                OriginalUrl = new Uri((string)links["original"]),
                ImgurUrl = new Uri((string)links["imgur_page"]),
                DeleteUrl = deleteUrl,
                SmallSquareUrl = new Uri((string)links["small_square"]),
                LargeThumbnail = new Uri((string)links["large_thumbnail"])
            };
        }
    }
}
