using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Map
    {
        public List<Tile> tiles;
        public int size;

        public Map(List<Tile> tiles, int size)
        {
            this.tiles = tiles;
            this.size = size;
        }
    }
}
