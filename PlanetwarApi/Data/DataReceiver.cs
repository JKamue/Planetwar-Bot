using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PlanetwarApi.Data;

namespace PlanetwarApi.Data
{
    public class DataReceiver : IDataReceiver
    {
        private readonly string _url;

        public DataReceiver(string url)
        {
            _url = url;
        }

        public Response SendRequest(string endpoint, Dictionary<string, string> headers, string body = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUri(endpoint));
            headers.ToList().ForEach(h => request.Headers[h.Key] = h.Value);

            if (body != null)
            {
                request.Method = "POST";
                var byteArray = Encoding.UTF8.GetBytes(body);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new Response(reader.ReadToEnd(), response.StatusCode);
                }
            }
            catch (WebException e)
            {
                using (HttpWebResponse response = (HttpWebResponse)e.Response)
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new Response(reader.ReadToEnd(), response.StatusCode);
                }
            }
        }

        private string GetUri(string endpoint) => $"{_url}/{endpoint}";
    }
}
