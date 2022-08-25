using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.ParentMenu
{
    public class ParentMenuRepository : BaseRepository, IParentMenuRepository
    {
        public ParentMenuRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ParentMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ParentMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ParentMenuName", entity.ParentMenuName, DbType.String);
                    parameters.Add("SortBy", entity.SortBy, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("UserId", entity.UserId, DbType.String);
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

        public async Task<int> DeleteAsync(ParentMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ParentMenu";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
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

        public async Task<List<ParentMenuModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ParentMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ParentMenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<ParentMenuModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ParentMenuModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ParentMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ParentMenuId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ParentMenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ParentMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ParentMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
                    parameters.Add("ParentMenuName", entity.ParentMenuName, DbType.String);
                    parameters.Add("SortBy", entity.SortBy, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("UserId", entity.UserId, DbType.String);
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
