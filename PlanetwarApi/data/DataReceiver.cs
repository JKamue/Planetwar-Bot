using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.data
{
    public class DataReceiver : IDataReceiver
    {
        private readonly string _url;

        public DataReceiver(string url)
        {
            _url = url;
        }

        public string Post(string endpoint, Dictionary<string, string> headers, string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUri(endpoint));
            headers.ToList().ForEach(h => request.Headers[h.Key] = h.Value);

            request.Method = "POST";

            var byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;


            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public string Get(string endpoint, Dictionary<string, string> headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUri(endpoint));
            headers.ToList().ForEach(h => request.Headers[h.Key] = h.Value);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private string GetUri(string endpoint) => $"{_url}/{endpoint}";
    }
}
