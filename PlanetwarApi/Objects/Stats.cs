using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Stats
    {
        public int visibleShips;
        public int planets;
        public Owner owner;
        public int production;

        public Stats(int visibleShips, int planets, Owner owner, int production)
        {
            this.visibleShips = visibleShips;
            this.planets = planets;
            this.owner = owner;
            this.production = production;
        }
    }
}
