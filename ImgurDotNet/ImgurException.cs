using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;

namespace ImgurDotNet
{
    public class ImgurException : Exception
    {
        public string Request { get; private set; }
        public string Method { get; private set; }
        public string Format { get; private set; }
        public string Parameters { get; private set; }

        public ImgurException(string message, string request, string method, string format, string parameters)
            : base(message)
        {
            this.Request = request;
            this.Method = method;
            this.Format = format;
            this.Parameters = parameters;
        }

        public static ImgurException Create(IDictionary<string, object> data)
        {
            return new ImgurException(
                (string)data["message"],
                (string)data["request"],
                (string)data["method"],
                (string)data["format"],
                (string)data["parameters"]
                );
        }
    }
}
