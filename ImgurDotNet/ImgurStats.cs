using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImgurDotNet
{
    public class ImgurStats
    {

        public enum ViewTime
        {
            Today,
            Week,
            Month
        }

        public List<string> MostPopularImages { get; private set; }
        public long ImagesUploaded { get; private set; }
        public long ImagesViewed { get; private set; }
        public string BandwithUsed { get; private set; }
        public string AverageImageSize { get; private set; }

        public static ImgurStats Create(IDictionary<string, object> data)
        {
            var mpi = (List<object>)data["most_popular_images"];
            List<string> mpiList = new List<string>();
            foreach (var image in mpi)
                mpiList.Add((string)image);
            return new ImgurStats
            {
                MostPopularImages = mpiList,
                ImagesUploaded = (long)data["images_uploaded"],
                ImagesViewed = (long)data["images_veiwed"],
                BandwithUsed = (string)data["bandwidth_used"],
                AverageImageSize = (string)data["average_image_size"]
            };
        }
    }
}
