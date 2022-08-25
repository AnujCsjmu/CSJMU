using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.AssignRole
{
    public class AssignRoleRepository : BaseRepository, IAssignRoleRepository
    {
        public AssignRoleRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(RegistrationRoleMapping entity)
        {
            try
            {
                int res = 0;
                var query = "SP_InsertUpdateDelete_AssignRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("RoleUserId", entity.RoleUserId, DbType.Int32);
                    parameters.Add("IsRoleActive", entity.IsRoleActive, DbType.Int32);
                    parameters.Add("IsMainRole", entity.IsMainRole, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress,DbType.String);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    StringBuilder stringBuilder = new StringBuilder();
                    //foreach (int roleid in entity.RoleList)
                    //{
                    //    parameters.Add("RoleId", roleid);
                    //    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    //}
                    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(RegistrationRoleMapping entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignRole";
                using (var connection = CreateConnection())
                {
                    entity.IsRoleActive = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserRoleId", entity.UserRoleId, DbType.Int32);
                    parameters.Add("IsRoleActive", entity.IsRoleActive, DbType.Int32);
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

        public async Task<List<RegistrationRoleMapping>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationRoleMapping>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<RegistrationRoleMapping>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RegistrationRoleMapping> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserRoleId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationRoleMapping>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(RegistrationRoleMapping entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_AssignRole";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserRoleId", entity.UserRoleId, DbType.Int32);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("IsRoleActive", entity.IsRoleActive, DbType.Int32);
                    parameters.Add("IsMainRole", entity.IsMainRole, DbType.Int32);
                    parameters.Add("RoleUserId", entity.RoleUserId, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
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
