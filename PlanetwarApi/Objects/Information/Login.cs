using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.Objects.Information
{
    public class Login
    {
        public string password;
        public string username;

        public Login(string password, string username)
        {
            this.password = password;
            this.username = username;
        }
    }
}
