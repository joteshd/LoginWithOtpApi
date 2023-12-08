using DbServices.DbModels;
using DbServices.GenericRepos;
using DbServices.GenericRepos.IGenericRepo;
using DbServices.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbConnection _connection;
        readonly IConfiguration _configuration;
        IDbTransaction transaction; 
        readonly string connectionString;

        public UnitOfWork()
        {
            _configuration = getConfig();
            connectionString = _configuration["ConnectionStrings:DbConnectionString"].ToString();
            _connection = new SqlConnection(connectionString);
        }

        public IBaseRepo<UserAuth> userAuthRepo => new BaseRepo<UserAuth>(_connection,_configuration, transaction);
            
        public IBaseRepo<User> userRepo => new BaseRepo<User>(_connection, _configuration, transaction);

        public IBaseRepo<Audit> auditLogsRepo => new BaseRepo<Audit>(_connection, _configuration, transaction);

        public IBaseRepo<OtpDetails> userOtpDetalRepo => new BaseRepo<OtpDetails>(_connection, _configuration, transaction);

        public void auditLogs()
        {
            if (!_connection.State.Equals(ConnectionState.Open))
            _connection.Open();
            Audit CurrentAudit = new Audit()
            {

            };
            Begin();
            bool isCreated =  auditLogsRepo.Create(CurrentAudit);
            if (isCreated)
            Commit();
            else
                Rollback();
        }

        public void Begin()
        {
            transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if(!(transaction is null))
            transaction.Commit();
        }

        public void Rollback()
        {
            if (!(transaction is null))
                transaction.Rollback();
        }

        public IConfiguration getConfig()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string fakepath = Directory.GetParent(currentDirectory).FullName;
            string filepath = Path.Combine(fakepath, "DbServices", "Dbsettings.json");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(filepath);
            return builder.Build();
        }
        public void Dispose()
        {
            if (!(transaction is null))
            {
                transaction.Dispose();
            }
            if (!(_connection is null))
            {
                _connection.Dispose();
            }
        }

       
    }
}
