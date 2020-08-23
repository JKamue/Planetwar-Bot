using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Location
    {
        public int X;
        public int Y;

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Distance(Location l2) => (int) Math.Round(Math.Sqrt(Math.Pow((l2.X - X), 2) + Math.Pow((l2.Y - Y), 2)));
    }
}
