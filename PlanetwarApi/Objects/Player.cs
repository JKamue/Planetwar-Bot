using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Player
    {
        public int visibleShips;
        public int planet;
        public Owner owner;
        public int production;

        public Player(int visibleShips, int planet, Owner owner, int production)
        {
            this.visibleShips = visibleShips;
            this.planet = planet;
            this.owner = owner;
            this.production = production;
        }
    }
}
