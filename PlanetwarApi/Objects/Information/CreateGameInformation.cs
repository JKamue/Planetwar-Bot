using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects.Information
{
    class CreateGameInformation
    {
        public int ships;
        public int production;
        public int players;
        public int size;

        public CreateGameInformation(int ships, int production, int players, int size)
        {
            this.ships = ships;
            this.production = production;
            this.players = players;
            this.size = size;
        }
    }
}
