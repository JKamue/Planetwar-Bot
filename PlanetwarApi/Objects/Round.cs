using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Round
    {
        public int number;
        public DateTime start;
        public int length;
        public bool spectator;
        public string playerId;
        public string creatorId;
        public string gameId;

        public Round(int number, DateTime start, int length, bool spectator, string playerId, string creatorId, string gameId)
        {
            this.number = number;
            this.start = start;
            this.length = length;
            this.spectator = spectator;
            this.playerId = playerId;
            this.creatorId = creatorId;
            this.gameId = gameId;
        }
    }
}
