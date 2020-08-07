using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlanetwarApi.objects.Information
{
    public class MoveShipsInformation
    {
        public Location Start;
        public Location End;
        public int Amount;

        public MoveShipsInformation(Location start, Location end, int amount)
        {
            Start = start;
            End = end;
            Amount = amount;
        }
    }
}
