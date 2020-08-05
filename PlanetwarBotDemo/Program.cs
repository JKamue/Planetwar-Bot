using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetwarApi.data;
using PlanetwarApi.objects;

namespace PlanetwarBotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var login = new Login("asdf","Aacc3");
            var api = new PlanetwarApi.PlanetwarApi(login, "https://planetwar.jkamue.de/api");
            Console.ReadLine();
        }
    }
}
