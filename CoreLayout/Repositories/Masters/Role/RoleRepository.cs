using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Role
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(RoleModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleName", entity.RoleName, DbType.String);
                    parameters.Add("Description", entity.Description, DbType.String);
                    parameters.Add("IsRecordDeleted", 0);
                    parameters.Add("IPAddress", "11");
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(RoleModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleID", entity.RoleID, DbType.Int32);
                    parameters.Add("IsRecordDeleted", 1);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("@Query", 3, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<RoleModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RoleModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<RoleModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RoleModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleID", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RoleModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(RoleModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Role";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleID", entity.RoleID, DbType.Int32);
                    parameters.Add("RoleName", entity.RoleName, DbType.String);
                    parameters.Add("Description", entity.Description, DbType.String);
                    parameters.Add("IsRecordDeleted", 0);
                    parameters.Add("IPAddress", "11");
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("@Query", 2, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
