using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetwarApi.Objects;
using PlanetwarApi.Data;
using PlanetwarApi.Objects.Information;

namespace PlanetwarBotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var login = new Login("asdf","Acc3");
            var api = new PlanetwarApi.PlanetwarApi(login, "https://planetwar.jkamue.de/api");

            var data = api.CreateGame(50, 12, 2, 10);
            api.JoinGame(data);
            api.StartGame();
            var round = api.QueryRoundInformation();
            var map = api.QueryMap();
            var events = api.QueryEvents();
            var players = api.QueryPlayerList();
            var sent = api.QuerySentShips();
        }
    }
}
