using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Planet
    {
        public int production;
        public int skin;

        public Planet(int production, int skin)
        {
            this.production = production;
            this.skin = skin;
        }
    }
}
