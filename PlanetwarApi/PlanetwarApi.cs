using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlanetwarApi.Data;
using PlanetwarApi.objects;
using PlanetwarApi.Objects;
using PlanetwarApi.objects.Information;
using PlanetwarApi.Objects.Information;
using PlanetwarApi.objects.Responses;
using PlanetwarApi.Objects.Responses;

namespace PlanetwarApi
{
    public class PlanetwarApi
    {
        private readonly string _accountKey;
        private bool inGame;
        private string gameKey;

        private readonly IDataReceiver _dataReceiver;

        public PlanetwarApi(Login login, string url)
        {
            _dataReceiver = new DataReceiver(url);

            string loginData = JsonConvert.SerializeObject(login);
            var response = _dataReceiver.SendRequest("login", new Dictionary<string, string>(), loginData);
            _accountKey = Decode<string>(response);
        }

        public CreateGameResponse CreateGame(int ships, int production, int players, int size)
        {
            var gameInformation = new CreateGameInformation(ships, production, players, size);
            var stringData = JsonConvert.SerializeObject(gameInformation);

            var response = _dataReceiver.SendRequest("create", GetAccountDictionary(), stringData);
            return Decode<CreateGameResponse>(response);
        }

        public void JoinGame(string gameId, string gameCode) => JoinGame(new CreateGameResponse(gameId, gameCode));

        public void JoinGame(CreateGameResponse gameInformation)
        {
            var stringData = JsonConvert.SerializeObject(gameInformation);
            var response = _dataReceiver.SendRequest("join", GetAccountDictionary(), stringData);
            inGame = true;
            gameKey = Decode<string>(response);
        }

        public void StartGame()
        {
            var response = _dataReceiver.SendRequest("start", GetGameDictionary(), "");
            Decode<string>(response);
        }

        public MoveShipsResponse MoveShips(Location start, Location end, int amount) =>
            MoveShips(new MoveShipsInformation(start, end, amount));

        public MoveShipsResponse MoveShips(MoveShipsInformation inf)
        {
            var stringData = JsonConvert.SerializeObject(inf);
            var response = _dataReceiver.SendRequest("move", GetGameDictionary(), stringData);
            return Decode<MoveShipsResponse>(response);
        }

        private Dictionary<string, string> GetAccountDictionary()
        {
            return new Dictionary<string, string>
            {
                { "Account-Key", _accountKey}
            };
        }

        private Dictionary<string, string> GetGameDictionary()
        {
            if (!inGame)
            {
                throw new Exception("Currently not in a game!");
            }

            return new Dictionary<string, string>
            {
                { "Account-Key", _accountKey},
                { "Game-Key", gameKey}
            };
        }

        private T Decode<T>(Response response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Data);
            }
             
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Endpoint not found");
            }

            var error = JsonConvert.DeserializeObject<Error>(response.Data);
            throw new Exception(error.description);
        }
    }
}
