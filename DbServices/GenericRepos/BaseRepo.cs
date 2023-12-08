using Dapper.Contrib.Extensions;
using DbServices.GenericRepos.IGenericRepo;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using Dapper;
using System.ComponentModel.Design;

namespace DbServices.GenericRepos
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        IDbConnection _connection;
        IDbTransaction _transaction;
        readonly IConfiguration _configuration;

        public BaseRepo(IDbConnection dbConnection, IConfiguration configuration, IDbTransaction transaction)
        {
            _connection = dbConnection;
            _configuration = configuration;
            _transaction = transaction;
        }
        public bool Create(T entity)
        {
            bool created = false;
            try
            {
                var createdEntity = _connection.Insert<T>(entity);
                created = (createdEntity is > 0);
            }
            catch (SqlException sql)
            {
            }
            return created;
        }

        public T SelectById(long Id)
        {
            T entity = null;
            entity = _connection.Get<T>(Id);
            return entity;
        }

        public IQueryable<T> getWithCondition(object where, string[] Includecolunms = null)
        {
            Type entity = typeof(T);
            IQueryable<T> entites;
            try
            {
                string tablename = $"[dbo].[{entity.Name}]";
                string? colunms = (Includecolunms != null && Includecolunms.Length > 0) ? Includecolunms.ToString() : "*";
                string query = $"select {colunms} from {tablename}";
                string strWhereCondition = CreateCondtion(where);
                string finalQuery = query + strWhereCondition;
                entites = _connection.Query<T>(finalQuery).AsQueryable();
            }
            catch (Exception ex)
            {

                throw;
            }



            return entites;
        }

        public bool Update(T entity)
        {
            bool updated = false;
            try
            {
                updated = _connection.Update<T>(entity);

            }
            catch (SqlException sql)
            {
                throw;
            }
            return updated;
        }

        private string CreateCondtion(object where)
        {
            string whereConditionString = " where ";
            string param = string.Empty;
            if (!(where is null))
            {
                Type entity = where?.GetType();
                var stringParams = entity.GetProperties();
                int propertyCounts = stringParams.Length;
                string and = " and ";

                //foreach (var property in stringParams)
                //{
                    
                //}

                for (int i = 0; i < stringParams.Length; i++)
                {
                    param = $" {stringParams[i].Name} ='{stringParams[i].GetValue(where, null)}'";
                    if (propertyCounts > 1 && (i+1) < propertyCounts)
                        whereConditionString += param + and;
                    else
                        whereConditionString += param;
                }
            }
            return whereConditionString;
        }

    }
}
