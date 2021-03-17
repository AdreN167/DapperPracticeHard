using Dapper;
using System;
using System.Linq;
using System.Data.Common;
using DapperLesson.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DapperLesson.Data
{
    public class SportContext : IDisposable
    {
        private readonly DbConnection connection;
        private IConfigurationRoot configuration;

        public ICollection<Player> Players { get; set; }
        public ICollection<Team> Teams { get; set; }

        public SportContext()
        {
            if (configuration == null)
            {
                var configurationBuilder = new ConfigurationBuilder();
                configuration = configurationBuilder.AddJsonFile("appSettings.json").Build();
            }

            connection = new SqlConnection();

            connection.ConnectionString = configuration["ConnectionString"];

            UpdateProperties();
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public void Add<T>(T entity)
        {
            string sql = $"insert into {entity.GetType().Name}s(";
            var count = 0;

            foreach (var property in entity.GetType().GetProperties())
            {
                sql = $"{sql} {property.Name}";

                if (count++ != entity.GetType().GetProperties().Length - 1)
                    sql = $"{sql},";
            }

            sql = $"{sql}) values(";

            count = 0;

            foreach (var property in entity.GetType().GetProperties())
            {
                sql = $"{sql} @{property.Name}";

                if (count++ != entity.GetType().GetProperties().Length - 1)
                    sql = $"{sql},";
            }

            sql = $"{sql})";

            connection.Execute(sql, entity);

            UpdateProperties();
        }

        public void Update<T>(T entity)
        {
            string sql = $"update {entity.GetType().Name}s set ";
            var count = 1;

            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.Name != "Id")
                {
                    if (!property.GetType().IsValueType || property.GetType() == typeof(Guid))
                        sql = $"{sql} {property.Name} = '{property.GetValue(entity)}'";
                    else
                        sql = $"{sql} {property.Name} = {property.GetValue(entity)}";

                    if (count++ != entity.GetType().GetProperties().Length - 1)
                        sql = $"{sql},";
                }
            }

            sql = $"{sql} where Id = '{entity.GetType().GetProperty("Id").GetValue(entity)}'";

            connection.Execute(sql, entity);

            UpdateProperties();
        }

        public void Remove<T>(T entity)
        {
            string sql = $"delete from {entity.GetType().Name}s where Id = '{entity.GetType().GetProperty("Id").GetValue(entity)}'";

            connection.Execute(sql, entity);

            UpdateProperties();
        }

        private ICollection<T> Select<T>()
        {
            return connection.Query<T>($"select * from {typeof(T).Name}s").ToList();
        }

        private void UpdateProperties()
        {
            Players = Select<Player>();
            Teams = Select<Team>();
        }

        public void ExecuteTransaction(params string[] commands)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {
                        connection.Execute(command, transaction);
                    }

                    transaction.Commit();

                    UpdateProperties();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
