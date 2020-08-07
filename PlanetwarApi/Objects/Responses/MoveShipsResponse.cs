using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects.Responses
{
    public class MoveShipsResponse
    {
        public int arrival;
        public int time;

        public MoveShipsResponse(int arrival, int time)
        {
            this.arrival = arrival;
            this.time = time;
        }
    }
}
