using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using ImgurDotNet;

namespace ImgurDotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string testImageUri = "C:\\Users\\Jaco\\Pictures\\test.jpg";
            var image = Image.FromFile(testImageUri);
            
            var imgur = new Imgur(String.Empty);
            
            var imgurImage = imgur.UploadImage(image, "API test_1", "API test_2", "API test_3", System.Drawing.Imaging.ImageFormat.Jpeg);
            Program.DumpImage(imgurImage);
           
            var imgurStatsMonth = imgur.GetStats(ImgurStats.ViewTime.Month);
            Console.WriteLine("Images viewed this month: {0}\n", imgurStatsMonth.ImagesViewed);

            var imgurStatsWeek = imgur.GetStats(ImgurStats.ViewTime.Week);
            Console.WriteLine("Images viewed this week: {0}\n", imgurStatsWeek.ImagesViewed);

            var imgurStatsToday = imgur.GetStats(ImgurStats.ViewTime.Today);
            Console.WriteLine("Images viewed today: {0}\n", imgurStatsToday.ImagesViewed);

            var album = imgur.GetAlbum("hf1pO");
            Console.WriteLine("Album name: {0}\nImage count: {1}\nType: {2}\n", album.Title, album.Images.Count, album.Layout);
            foreach (var img in album.Images)
                Program.DumpImage(img);

            var imgurImage2 = imgur.GetImage(imgurImage.Hash);
            Program.DumpImage(imgurImage2);

            imgur.DeleteImage(imgurImage.DeleteHash);
            Console.WriteLine("Image deleted\nDelete hash: {0}", imgurImage.DeleteHash);

            Thread.Sleep(10000);

            try
            {
                imgur.GetImage(imgurImage.Hash);
            }
            catch (ImgurException e)
            {
                Console.WriteLine("ImgurException\nMessage: {0}\nRequest: {1}\n", e.Message, e.Request);
            }

            Console.ReadLine();
        }

        private static void DumpImage(ImgurImage img)
        {
            Console.WriteLine("Image title: {0}\nImage caption: {1}\nImage url: {2}\n", img.Title, img.Caption, img.ImgurUrl);
        }
    }
}
