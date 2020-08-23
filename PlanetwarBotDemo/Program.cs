using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlanetwarApi.Objects;
using PlanetwarApi.Data;
using PlanetwarApi.objects;
using PlanetwarApi.Objects.Information;

namespace PlanetwarBotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Login
            var login = new Login("asdf","Acc3");
            var api = new PlanetwarApi.PlanetwarApi(login, "https://planetwar.jkamue.de/api");

            // Creates joins and starts a new game
            var data = api.CreateGame(50, 12, 2, 10);
            Console.WriteLine("Created new game!");
            Console.WriteLine("ID: " + data.gameId);
            Console.WriteLine("Code: " + data.gameCode);
            api.JoinGame(data);
            Console.WriteLine("Press ENTER to start...");
            Console.ReadLine();
            api.StartGame();

            // The game takes place within the loop
            while (true)
            {
                // Gather round information
                var roundInfo = api.QueryRoundInformation();    // Round Information like duration, round number and round end
                var map = api.QueryMap();                       // The map consisting of a list of tiles
                var events = api.QueryEvents();        // Events, who attacked / supported
                var players = api.QueryPlayerList();  // Public stats of visible ships for every player
                var sent = api.QuerySentShips();       // List of sent ships

                var myPersonalStats = players.First(p => p.owner.name == login.username);

                var myShipCount = CalculateAmountOfShips(login.username, map);      // Calculate the total number of own ships (including hidden ships) (excluding ships in hyperspace)
                var sentShipCount = sent.Sum(s => s.amount);
                var hiddenShipCount = myShipCount - myPersonalStats.visibleShips;   // Total - Visible = hidden ships

                // Output some basic info
                Console.Clear();
                Console.WriteLine($"Round Nr.{roundInfo.number}");
                Console.WriteLine($"{sentShipCount + myShipCount} total Ships\n {myPersonalStats.visibleShips} visible Ships\n {hiddenShipCount} hidden Ships\n {sentShipCount} jumping Ships");
                Console.WriteLine($"Planets: {myPersonalStats.planet}   Production: {myPersonalStats.production}");

                // Generate list of own planets
                var myPlanets = map.tiles
                    .Where(t => t.owner != null)
                    .Where(t => t.owner.name == login.username).ToList();

                foreach (var planet in myPlanets)
                {
                    if (planet.ships == 0)
                        continue; // Make sure the selected planet has ships

                    // Rank each planet by distance && production && enemy ships combined
                    var ranking = new List<TileRating>();
                    foreach (var tile in map.tiles)
                    {
                        if (!tile.hasPlanet)
                            continue; // Only rate tiles with planets

                        if (tile.owner.name == login.username)
                            continue; // Don't conquer own planets

                        var distanceFromPlanet = tile.location.Distance(planet.location);
                        // Arbitrary formula to rate planets - there most likely are way better ones
                        // Formular could also change dependend on round number
                        var score = Math.Pow(distanceFromPlanet, 1.5) - tile.planet.production * 1.5;
                        ranking.Add(new TileRating(score, tile));
                    }

                    // Order tile list by rating
                    ranking = ranking.OrderBy(t => t.rating).ToList();

                    if (ranking.Count == 0)
                        continue; // Check to make sure there are available planets

                    var target = ranking[0].tile;

                    api.MoveShips(planet.location, target.location, planet.ships);
                }

                while (DateTime.Now < roundInfo.start.AddSeconds(roundInfo.length))
                {
                    Thread.Sleep(500);
                }

                while (api.QueryRoundInformation().number != roundInfo.number + 1)
                {
                    Thread.Sleep(250);
                }

                Console.WriteLine("Reload!");
            }

        }

        private static int CalculateAmountOfShips(string username, Map map)
        {
            return map.tiles
                .Where(tile => tile.owner != null)
                .Where(tile => tile.owner.name == username)
                .Sum(tile => tile.ships);
        }

        private struct TileRating
        {
            public double rating;
            public Tile tile;

            public TileRating(double rating, Tile tile)
            {
                this.rating = rating;
                this.tile = tile;
            }
        }
    }
}
