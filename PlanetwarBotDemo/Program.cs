using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlanetwarApi;
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
           new PlanetwarBot(BotLoop);
        }

        private static void BotLoop(PlanetwarApi.PlanetwarApi api, string username, Round roundInfo, Map map, List<Event> events, List<Player> players, List<Sent> sent)
        {

            // Output some basic info
            Console.Clear();
            Console.WriteLine($"Round Nr.{roundInfo.number}");

            // Generate list of own planets
            var myPlanets = map.tiles
                .Where(t => t.owner != null)
                .Where(t => t.owner.name == username).ToList();

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

                    if (tile.owner.name == username)
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
                        (int)Math.Round((double)planet.ships - roundInfo.number / 2));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Planet has less ships than expected...");
                }
            }
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
