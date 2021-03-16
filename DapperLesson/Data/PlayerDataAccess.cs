using Dapper;
using DapperLesson.Models;
using System.Collections.Generic;
using System.Linq;

namespace DapperLesson.Data
{
    public class PlayerDataAccess : DbDataAccess<Player>
    {
        public override void Insert(Player entity)
        {
            connection.Execute("insert into Players(Id, FullName, Number, TeamId) values(@Id, @FullName, @Number, @TeamId)", entity);
        }

        public override void Update(Player oldEntity, Player newEntity)
        {
            connection.Execute($"update Players where Id = {oldEntity.Id} set FullName = {newEntity.FullName}, Number = {newEntity.Number}, TeamId = {newEntity.TeamId}");
        }

        public override void Delete(Player entity)
        {
            connection.Execute($"delete Players where Id = {entity.Id}");
        }

        public override ICollection<Player> Select()
        {
            return connection.Query<Player>("select * from Players").ToList();
        }
    }
}
