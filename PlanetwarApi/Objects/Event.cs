using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Event
    {
        public string id;
        public Owner attacker;
        public Owner defender;
        public bool attackerWon;
        public Location location;
        public int attackerShips;
        public int defenderShips;
        public int attackerShipsLost;
        public int defenderShipsLost;
        public int round;

        public Event(string id, Owner attacker, Owner defender, bool attackerWon, Location location, int attackerShips, int defenderShips, int attackerShipsLost, int defenderShipsLost, int round)
        {
            this.id = id;
            this.attacker = attacker;
            this.defender = defender;
            this.attackerWon = attackerWon;
            this.location = location;
            this.attackerShips = attackerShips;
            this.defenderShips = defenderShips;
            this.attackerShipsLost = attackerShipsLost;
            this.defenderShipsLost = defenderShipsLost;
            this.round = round;
        }
    }
}
