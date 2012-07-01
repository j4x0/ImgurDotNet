using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ImgurDotNet;

namespace ImgurDotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string testImageUri = "C:\\Users\\Jaco\\Pictures\\test.jpg";
            var image = Image.FromFile(testImageUri);
            
            var imgur = new Imgur("1867b8e2493c1d9205eb08dffa70afaa");
            
            var imgurImage = imgur.UploadImage(image, "test", "test", "test", System.Drawing.Imaging.ImageFormat.Jpeg);
            Console.WriteLine(imgurImage.ImgurUrl);
           
            var imgurStatsMonth = imgur.GetStats(ImgurStats.ViewTime.Month);
            Console.WriteLine(imgurStatsMonth.ImagesViewed);

            var imgurStatsWeek = imgur.GetStats(ImgurStats.ViewTime.Week);
            Console.WriteLine(imgurStatsWeek.ImagesViewed);

            var imgurStatsToday = imgur.GetStats(ImgurStats.ViewTime.Today);
            Console.WriteLine(imgurStatsToday.ImagesViewed);
            
            Console.ReadLine();
        }
    }
}
