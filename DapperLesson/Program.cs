using DapperLesson.Data;
using DapperLesson.Models;
using System;
using System.Linq;

namespace DapperLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstTeam = new Team 
            { 
                Name = "Реал мадрид" 
            };

            var secondTeam = new Team 
            { 
                Name = "Барсилона"
            };

            var firstPlayer = new Player
            {
                FullName = "Месси",
                Number = 5,
                TeamId = secondTeam.Id
            };
            
            var secondPlayer = new Player
            {
                FullName = "Роналду",
                Number = 17,
                TeamId = firstTeam.Id
            };

            using (var context = new SportContext())
            {
                context.Add(firstTeam);
                context.Add(secondTeam);

                context.Add(firstPlayer);
                context.Add(secondPlayer);

                foreach (var team in context.Teams)
                {
                    Console.WriteLine($"\nКоманда {team.Name}");
                    Console.WriteLine("Игроки:");

                    foreach (var player in context.Players.Where(player => player.TeamId == team.Id).ToList())
                    {
                        Console.WriteLine($"[{player.Number}] {player.FullName}");
                    }
                }

                var result = context.Players.First();

                var teamIdForRemove = result.TeamId;

                result.TeamId = context.Teams.Where(team => team.Id != result.TeamId).First().Id;

                context.Update(result);
                context.Remove(context.Teams.Where(team => team.Id == teamIdForRemove).First());

                foreach (var team in context.Teams)
                {
                    Console.WriteLine($"\nКоманда {team.Name}");
                    Console.WriteLine("Игроки:");

                    foreach (var player in context.Players.Where(player => player.TeamId == team.Id).ToList())
                    {
                        Console.WriteLine($"[{player.Number}] {player.FullName}");
                    }
                }
            }
        }
    }
}
