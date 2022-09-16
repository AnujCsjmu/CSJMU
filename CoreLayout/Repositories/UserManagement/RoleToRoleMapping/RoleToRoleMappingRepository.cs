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

namespace CoreLayout.Repositories.UserManagement.RoleToRoleMapping
{
    public class RoleToRoleMappingRepository : BaseRepository, IRoleToRoleMappingRepository
    {
        public RoleToRoleMappingRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(RoleToRoleMappingModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                // create the transaction
                // You could use `var` instead of `SqlTransaction`
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        int res = 0;
                        var query = "SP_InsertUpdateDelete_RoleToRoleMapping";

                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("FromRoleId", entity.FromRoleId, DbType.Int32);
                        //parameters.Add("ToRoleId", entity.ToRoleId, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("@Query", 1, DbType.Int32);
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (int ToRoleId in entity.ToRoleList)
                        {
                            parameters.Add("ToRoleId", ToRoleId, DbType.Int32);
                            res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        }
                        if (res == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return res;
                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        tran.Rollback();

                        // handle the error however you need to.
                        throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<int> DeleteAsync(RoleToRoleMappingModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_RoleToRoleMapping";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleMappingId", entity.RoleMappingId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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

        public async Task<List<RoleToRoleMappingModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_RoleToRoleMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RoleToRoleMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<RoleToRoleMappingModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RoleToRoleMappingModel> GetByIdAsync(int RoleMappingId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_RoleToRoleMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleMappingId", RoleMappingId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RoleToRoleMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(RoleToRoleMappingModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                // create the transaction
                // You could use `var` instead of `SqlTransaction`
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        int res = 0;
                        var query = "SP_InsertUpdateDelete_RoleToRoleMapping";

                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("FromRoleId", entity.FromRoleId, DbType.Int32);
                        //parameters.Add("ToRoleId", entity.ToRoleId, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                        parameters.Add("@Query", 2, DbType.Int32);
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (int ToRoleId in entity.ToRoleList)
                        {
                            parameters.Add("ToRoleId", ToRoleId, DbType.Int32);
                            res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        }

                        if (res == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res;

                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        tran.Rollback();

                        // handle the error however you need to.
                        throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

       
    }
}
