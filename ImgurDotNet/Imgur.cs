using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SimpleJson;

namespace ImgurDotNet
{
    public class Imgur
    {
        private static readonly string UPLOAD_URL = "api.imgur.com/2/upload.json";
        private static readonly string STATS_URL = "api.imgur.com/2/stats.json?view={0}";
        private static readonly string ALBUM_URL = "api.imgur.com/2/album/{0}.json";
        private static readonly string IMAGE_URL = "api.imgur.com/2/image/{0}.json";

        private string key;
        public string Key
        {
            get
            {
                if (this.key == null)
                    throw new Exception("API key isn't set!");
                else
                    return this.key;
            }
            set
            {
                this.key = value;
            }
        }
        public bool UseSsl { get; set; }

        public Imgur(string key)
        {
            this.Key = key;
        }

        public Imgur() : this(null) { }

        public ImgurImage UploadImage(byte[] imgData, string name, string title, string caption)
        {
            var data = String.Format("key={0}&name={1}&title={2}&caption={3}&image={4}", 
                this.Key, 
                name, 
                title, 
                caption,  
                Imgur.EscapeBase64(Convert.ToBase64String(imgData))
                );
            var response = Imgur.GetParsedJsonResponse(this.GetProtocol() + Imgur.UPLOAD_URL, data);
            var parsed = (IDictionary<string, object>)response["upload"];
            var first = parsed.First();
            if (first.Key == "error")
                throw ImgurException.Create(parsed);
            else if (first.Key == "image")
                return ImgurImage.Create(parsed);
            else
                throw new Exception("Couldn't parse response: " + first.Key);
        }

        public ImgurImage UploadImage(Image image, string name, string title, string caption, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            return this.UploadImage(stream.ToArray(), name, title, caption);
        }

        public ImgurImage UploadImage(string imgUri, string name, string title, string caption)
        {
            var stream = File.OpenRead(imgUri);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();
            return this.UploadImage(data, name, title, caption);
        }

        public ImgurStats GetStats(ImgurStats.ViewTime viewTime)
        {
            string time = "";
            switch (viewTime)
            {
                case ImgurStats.ViewTime.Today:
                    time = "today";
                    break;
                case ImgurStats.ViewTime.Week:
                    time = "week";
                    break;
                case ImgurStats.ViewTime.Month:
                    time = "month";
                    break;
            }
            var response = Imgur.GetParsedJsonResponse(this.GetProtocol() + String.Format(Imgur.STATS_URL, time));
            var first = response.First();
            if (first.Key == "error")
                throw ImgurException.Create((IDictionary<string, object>)first.Value);
            else if (first.Key == "stats")
                return ImgurStats.Create((IDictionary<string, object>)first.Value);
            else
                throw new Exception("Couldn't parse response: " + first.Key);
        }

        private string GetProtocol()
        {
            return this.UseSsl ? "https://" : "http://";
        }

        private static string EscapeBase64(string str)
        {
            string escaped = ""; 
            for (var i = 0; i < str.Length; i ++)
            {
                escaped += Uri.EscapeDataString(str.Substring(i, 1));
            }
            return escaped;
        }

        private static IDictionary<string, object> GetParsedJsonResponse(string url)
        {
            var request = WebRequest.Create(url);
            Stream resp = null;
            try
            {
                resp = request.GetResponse().GetResponseStream();
            }
            catch (WebException e)
            {
                resp = e.Response.GetResponseStream();
            }
            var reader = new StreamReader(resp);
            var response = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(reader.ReadToEnd());
            reader.Close();
            resp.Close();
            return response;
        }

        private static IDictionary<string, object> GetParsedJsonResponse(string url, string postData)
        {
            var request = WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            var data = UTF8Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;

            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();

            Stream resp = null;
            try
            {
                resp = request.GetResponse().GetResponseStream();
            }
            catch (WebException e)
            {
                resp = e.Response.GetResponseStream();
            }
            var reader = new StreamReader(resp);
            var response = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(reader.ReadToEnd());
            reader.Close();
            resp.Close();
            return response;
        }
    }
}
