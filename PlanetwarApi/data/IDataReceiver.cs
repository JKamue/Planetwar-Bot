﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.data
{
    public interface IDataReceiver
    {
        Response SendRequest(string endpoint, Dictionary<string, string> headers, string body = null);
    }
}
