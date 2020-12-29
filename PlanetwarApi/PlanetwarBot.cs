using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlanetwarApi.objects;
using PlanetwarApi.Objects.Information;

namespace PlanetwarApi
{
    public class PlanetwarBot
    {
        private readonly PlanetwarApi _api;
        private readonly Action<PlanetwarApi, string, Round, Map, List<Event>, List<Player>, List<Sent>> _loopFunction;
        private readonly string _username;

        public PlanetwarBot(Action<PlanetwarApi, string, Round, Map, List<Event>, List<Player>, List<Sent>> LoopFunction)
        {
            _loopFunction = LoopFunction;

            var login = Login();
            _username = login.username;

            _api = new PlanetwarApi(login, "https://planetwar.jkamue.de/api");

            ShowStartGameDialog();

            GameLoop();
        }

        private void GameLoop()
        {
            while (true)
            {
                // Gather round information
                var roundInfo = _api.QueryRoundInformation();    // Round Information like duration, round number and round end
                var map = _api.QueryMap();                       // The map consisting of a list of tiles
                var events = _api.QueryEvents();        // Events, who attacked / supported
                var players = _api.QueryPlayerList();  // Public stats of visible ships for every player
                var sent = _api.QuerySentShips();       // List of sent ships

                _loopFunction.Invoke(_api, _username, roundInfo, map, events, players, sent);
                
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var sourceUTCTime = TimeZoneInfo.ConvertTimeToUtc(roundInfo.start, timeZoneInfo);
                
                while (DateTime.Now.ToUniversalTime() < sourceUTCTime.AddSeconds(roundInfo.length))
                {
                    Thread.Sleep(500);
                }

                while (_api.QueryRoundInformation().number == roundInfo.number)
                {
                    Thread.Sleep(250);
                }

                Console.WriteLine("Reload!");
            }

        }

        private void ShowStartGameDialog()
        {
            Console.WriteLine("Press (1) to create new game, anything else to enter existing game");
            var choice = Console.ReadKey().KeyChar;
            Console.Clear();

            if (choice == '1')
            {
                CreateGamePrompt();
            }
            else
            {
                JoinGamePrompt();
            }
        }

        private void JoinGamePrompt()
        {
            Console.WriteLine("Join existing game");
            Console.Write("ID: ");
            var id = Console.ReadLine();
            Console.Write("Key: ");
            var key = Console.ReadLine();
            _api.JoinGame(id, key);

            Console.WriteLine("Press ENTER if owner started game");
            Console.ReadLine();
        }

        private void CreateGamePrompt()
        {
            Console.WriteLine("Enter game settings");
            var size = ReadInteger(" Size");
            var players = ReadInteger(" Players");
            var production = ReadInteger(" Production");
            var ships = ReadInteger(" Ships");
            Console.Clear();

            // Creates joins and starts a new game
            var data = _api.CreateGame(ships, production, players, size);
            Console.WriteLine("Created new game");
            Console.WriteLine(" ID: " + data.gameId);
            Console.WriteLine(" Code: " + data.gameCode);
            _api.JoinGame(data);
            Console.WriteLine("Press ENTER to start...");
            Console.ReadLine();
            _api.StartGame();
        }

        private static Login Login()
        {
            Console.WriteLine("Login");
            Console.Write(" Username: ");
            var username = Console.ReadLine();
            Console.Write(" Password: ");
            var password = Console.ReadLine();
            Console.Clear();
            return new Login(password, username);
        }

        private static int ReadInteger(string text)
        {
            Console.Write(text + ": ");
            return int.Parse(Console.ReadLine());
        }
    }
}
