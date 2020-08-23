using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Tile
    {
        public Location location;
        public int ships;
        public Planet planet;
        public bool hasPlanet;
        public Owner owner;
        public bool hidden;

        public Tile(Location location, int ships, bool hidden, Owner owner = null, bool hasPlanet = false, Planet planet = null)
        {
            this.location = location;
            this.ships = ships;
            this.planet = planet;
            this.hasPlanet = hasPlanet;
            this.owner = owner;
            this.hidden = hidden;
        }
    }
}
