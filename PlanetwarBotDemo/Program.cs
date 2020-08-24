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
            Console.WriteLine("Login:");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            Console.Clear();

            // Login
            var login = new Login(password,username);
            var api = new PlanetwarApi.PlanetwarApi(login, "https://planetwar.jkamue.de/api");

            Console.WriteLine("Press (1) to create new game, anything else to enter existing game");
            var choice = Console.ReadKey().KeyChar;
            Console.Clear();

            if (choice == '1')
            {
                Console.WriteLine("Enter game settings:");
                Console.Write("Size: ");
                var size = Int32.Parse(Console.ReadLine());
                Console.Write("Players: ");
                var players = Int32.Parse(Console.ReadLine());
                Console.Write("Production: ");
                var production = Int32.Parse(Console.ReadLine());
                Console.Write("Ships: ");
                var ships = Int32.Parse(Console.ReadLine());
                Console.Clear();

                // Creates joins and starts a new game
                var data = api.CreateGame(ships, production, players, size);
                Console.WriteLine("Created new game!");
                Console.WriteLine("ID: " + data.gameId);
                Console.WriteLine("Code: " + data.gameCode);
                api.JoinGame(data);
                Console.WriteLine("Press ENTER to start...");
                Console.ReadLine();
                api.StartGame();
            }
            else
            {
                Console.WriteLine("Join existing game");
                Console.Write("ID: ");
                var id = Console.ReadLine();
                Console.Write("Key: ");
                var key = Console.ReadLine();
                api.JoinGame(id, key);

                Console.WriteLine("Press ENTER if owner started game");
                Console.ReadLine();
            }


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

                // Output some basic info
                Console.Clear();
                Console.WriteLine($"Round Nr.{roundInfo.number}");

                // Generate list of own planets
                var myPlanets = map.tiles
                    .Where(t => t.owner != null)
                    .Where(t => t.owner.name == login.username).ToList();

                foreach (var planet in myPlanets)
                {
                    if (planet.ships < roundInfo.number)
                        continue; // Make sure the selected planet has ships and also establish a small defense fleet

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

                    try
                    {
                        api.MoveShips(planet.location, target.location,
                            (int) Math.Round((double) planet.ships - roundInfo.number / 2));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Planet has less ships than expected...");
                    }
                }

                while (DateTime.Now < roundInfo.start.AddSeconds(roundInfo.length))
                {
                    Thread.Sleep(500);
                }

                while (api.QueryRoundInformation().number == roundInfo.number)
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
