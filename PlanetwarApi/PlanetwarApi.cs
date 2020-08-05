using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlanetwarApi.Data;
using PlanetwarApi.Objects;

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

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<Error>(response.Data);
                throw new Exception(error.description);
            }

            _accountKey = JsonConvert.DeserializeObject<string>(response.Data);
            Console.WriteLine(_accountKey);
        }
    }
}
