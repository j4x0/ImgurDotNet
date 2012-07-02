using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImgurDotNet
{
    public class ImgurAlbum
    {
        public enum LayoutType
        {
            Blog,
            Grid,
            Vertical,
            Horizontal
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Cover { get; private set; }
        public LayoutType Layout { get; private set; }
        public List<ImgurImage> Images { get; private set; }

        public static ImgurAlbum Create(IDictionary<string, object> data)
        {
            var layoutRaw = (string)data["layout"];
            LayoutType layout;
            switch (layoutRaw)
            {
                case "blog":
                    layout = LayoutType.Blog;
                    break;
                case "grid":
                    layout = LayoutType.Grid;
                    break;
                case "vertical":
                    layout = LayoutType.Vertical;
                    break;
                case "horizontal":
                    layout = LayoutType.Horizontal;
                    break;
                default:
                    throw new Exception("Couldn't parse album layout: " + layoutRaw);
            }
            var imageArrayRaw = (List<object>)data["images"];
            var imageArray = new List<ImgurImage>();
            foreach (var image in imageArrayRaw)
                imageArray.Add(ImgurImage.Create((IDictionary<string, object>)image));
            return new ImgurAlbum
            {
                Title = (string)data["title"],
                Description = (string)data["description"],
                Cover = (string)data["cover"],
                Layout = layout,
                Images = imageArray
            };
        }
    }
}
