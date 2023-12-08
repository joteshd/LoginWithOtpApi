using DbServices.DbModels;
using DbServices.GenericRepos;
using DbServices.GenericRepos.IGenericRepo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBaseRepo<UserAuth> userAuthRepo { get; }
        IBaseRepo<User>  userRepo  { get; }
        IBaseRepo<Audit> auditLogsRepo { get; }

        IBaseRepo<OtpDetails> userOtpDetalRepo { get; }

        IConfiguration getConfig();
        void Begin();
        void Commit();
        void Rollback();
        void auditLogs();
        void Dispose();
    }
}
