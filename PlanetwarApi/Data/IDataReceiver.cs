using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using PlanetwarApi.Data;

namespace PlanetwarApi.Data
{
    public interface IDataReceiver
    {
        Response SendRequest(string endpoint, Dictionary<string, string> headers, string body = null);
    }
}
