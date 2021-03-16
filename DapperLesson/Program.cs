using DapperLesson.Data;
using DapperLesson.Models;
using DapperLesson.Services;
using System;
using System.Linq;

namespace DapperLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationService.Init();

            using (var playerDataAccess = new PlayerDataAccess())
            {
                foreach (var player in playerDataAccess.Select())
                {
                    Console.WriteLine($"[{player.Number}]{player.FullName}");
                }

                var teamDataAccess = new TeamDataAccess();

                var result = teamDataAccess.Select();

                playerDataAccess.Insert(new Player { FullName = "Петя", Number = 5, TeamId = result.First().Id });

                teamDataAccess.Dispose();

                Console.WriteLine();

                foreach (var player in playerDataAccess.Select())
                {
                    Console.WriteLine($"[{player.Number}]{player.FullName}");
                }
            }
        }
    }
}
