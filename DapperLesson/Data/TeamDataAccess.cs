using Dapper;
using System.Linq;
using DapperLesson.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DapperLesson.Data
{
    public class TeamDataAccess : DbDataAccess<Team>
    {    
        public override void Insert(Team entity)
        {
            connection.Execute("insert into Teams(Id, Name) values(@Id, @Name)", entity);
        }

        public override void Update(Team oldEntity, Team newEntity)
        {
            connection.Execute($"update Teams where Id = {oldEntity.Id} set Name = {newEntity.Name}");
        }

        public override void Delete(Team entity)
        {
            connection.Execute($"delete Teams where Id = {entity.Id}");
        }

        public override ICollection<Team> Select()
        {
            return connection.Query<Team>("select * from Teams").ToList();
        }
    }
}
