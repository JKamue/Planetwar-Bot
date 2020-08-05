using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.data
{
    public class Response
    {
        public readonly string Data;
        public readonly HttpStatusCode StatusCode;

        public Response(string data, HttpStatusCode statusCode)
        {
            Data = data;
            StatusCode = statusCode;
        }
    }
}
