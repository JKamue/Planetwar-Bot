using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects.Responses
{
    public class CreateGameResponse
    {
        public string gameId;
        public string gameCode;

        public CreateGameResponse(string gameId, string gameCode)
        {
            this.gameId = gameId;
            this.gameCode = gameCode;
        }
    }
}
