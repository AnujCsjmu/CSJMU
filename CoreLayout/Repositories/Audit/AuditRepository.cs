using CoreLayout.Models;
using CoreLayout.Models.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Audit
{
    public class AuditRepository: BaseRepository, IAuditRepository
    {
        public AuditRepository(IConfiguration configuration)
: base(configuration)
        { 
        }

        public Task<int> CreateAsync(AuditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AuditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<AuditModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuditModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertAuditLogs(AuditModel entity)
        {
            try
            {

                var query = "[SP_InsertLogs]";
                using (var connection = CreateConnection())
                {
                 
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("Userbrowser", entity.Userbrowser, DbType.String);
                    parameters.Add("UrlReferrer", entity.UrlReferrer, DbType.String);
                    parameters.Add("SessionId", entity.SessionId, DbType.String);
                    parameters.Add("PageAccessed", entity.PageAccessed, DbType.String);
                    parameters.Add("LoginStatus", entity.LoginStatus, DbType.String);
                    parameters.Add("LoggedOutAt", entity.LoggedOutAt, DbType.String);
                    parameters.Add("LoggedInAt", entity.LoggedInAt, DbType.String);
                    parameters.Add("IpAddress", entity.IpAddress, DbType.String);
                    parameters.Add("ControllerName", entity.ControllerName, DbType.String);
                    parameters.Add("AuthorizationToken", entity.AuthorizationToken, DbType.String);
                    parameters.Add("Area", entity.Area, DbType.String);
                    parameters.Add("ActionName", entity.ActionName, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    var Result = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return Result > 0 ? true : false;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<int> UpdateAsync(AuditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
