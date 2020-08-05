using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlanetwarApi.Data;
using PlanetwarApi.Objects;
using PlanetwarApi.Objects.Information;
using PlanetwarApi.Objects.Responses;

namespace PlanetwarApi
{
    public class PlanetwarApi
    {
        private readonly string _accountKey;
        private readonly IDataReceiver _dataReceiver;

        public PlanetwarApi(Login login, string url)
        {
            _dataReceiver = new DataReceiver(url);

            string loginData = JsonConvert.SerializeObject(login);
            var response = _dataReceiver.SendRequest("login", new Dictionary<string, string>(), loginData);

            _accountKey = Decode<string>(response);
            Console.WriteLine(_accountKey);
        }

        private T Decode<T>(Response response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Data);
            }
            
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new HttpRequestException("Endpoint not found");
            }

            var error = JsonConvert.DeserializeObject<Error>(response.Data);
            throw new Exception(error.description);
        }
    }
}
