using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.AssignMenuByUser
{
    public class AssignMenuByUserRepository : BaseRepository, IAssignMenuByUserRepository
    {
        public AssignMenuByUserRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(AssignMenuByUserModel entity)
        {
            try
            {
                int res = 0;
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("Active", entity.Active, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    StringBuilder stringBuilder = new StringBuilder();
                    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(AssignMenuByUserModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", entity.MenuPermissionId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("ModifiedBy", entity.CreatedBy, DbType.Int32);
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

        public async Task<List<AssignMenuByUserModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<AssignMenuByUserModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<AssignMenuByUserModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<AssignMenuByUserModel>> CheckAlreadyAsync(int menuid, int userid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", menuid, DbType.Int32);
                    parameters.Add("UserId", userid, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<AssignMenuByUserModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<AssignMenuByUserModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<AssignMenuByUserModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<AssignMenuByUserModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(AssignMenuByUserModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByUser";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", entity.MenuPermissionId, DbType.Int32);
                    parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("Active", entity.Active, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
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
