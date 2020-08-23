using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Sent
    {
        public string id;
        public Location start;
        public Location end;
        public int sentAt;
        public int arriveAt;
        public int amount;

        public Sent(string id, Location start, Location end, int sentAt, int arriveAt, int amount)
        {
            this.id = id;
            this.start = start;
            this.end = end;
            this.sentAt = sentAt;
            this.arriveAt = arriveAt;
            this.amount = amount;
        }
    }
}
