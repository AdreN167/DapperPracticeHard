using Dapper;
using DapperLesson.Services;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System;

namespace DapperLesson.Data
{
    public abstract class DbDataAccess<T> : IDisposable
    {
        protected readonly DbConnection connection;

        public DbDataAccess()
        {
            connection = new SqlConnection();
            connection.ConnectionString = ConfigurationService.Configuration["ConnectionString"];
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public abstract void Insert(T entity);
        public abstract void Update(T oldEntity, T newEntity);
        public abstract void Delete(T entity);
        public abstract ICollection<T> Select();

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
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
