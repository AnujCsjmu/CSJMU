using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.AssignMenuByRole
{
    public class AssignMenuByRoleRepository : BaseRepository, IAssignMenuByRoleRepository
    {
        public AssignMenuByRoleRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(AssignMenuByRoleModel entity)
        {
            try
            {
                int res = 0;
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
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

        public async Task<int> DeleteAsync(AssignMenuByRoleModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", entity.MenuPermissionId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
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

        public async Task<List<AssignMenuByRoleModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<AssignMenuByRoleModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<AssignMenuByRoleModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<AssignMenuByRoleModel>> CheckAlreadyAsync(int menuid,int roleid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", menuid, DbType.Int32);
                    parameters.Add("RoleId", roleid, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<AssignMenuByRoleModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<AssignMenuByRoleModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<AssignMenuByRoleModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<AssignMenuByRoleModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(AssignMenuByRoleModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignMenuByRole";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuPermissionId", entity.MenuPermissionId, DbType.Int32);
                    parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
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
