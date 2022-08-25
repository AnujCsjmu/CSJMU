using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.SubMenu
{
    public class SubMenuRepository : BaseRepository, ISubMenuRepository
    {
        public SubMenuRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(SubMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubMenu";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.URL = "test/123";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubMenuName", entity.SubMenuName, DbType.String);
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
                    parameters.Add("URL", entity.URL, DbType.String);
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

        public async Task<int> DeleteAsync(SubMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubMenu";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubMenuId", entity.SubMenuId, DbType.Int32);
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

        public async Task<List<SubMenuModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubMenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<SubMenuModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<SubMenuModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubMenu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubMenuId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<SubMenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(SubMenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubMenu";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.URL = "test/123";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubMenuId", entity.SubMenuId, DbType.Int32);
                    parameters.Add("SubMenuName", entity.SubMenuName, DbType.String);
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
                    parameters.Add("URL", entity.URL, DbType.String);
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
