using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.Menu
{
    public class MenuRepository : BaseRepository, IMenuRepository
    {
        public MenuRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(MenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
                    parameters.Add("SubMenuId", entity.SubMenuId, DbType.Int32);
                    parameters.Add("MenuName", entity.MenuName, DbType.String);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("Controller", entity.Controller, DbType.String);
                    parameters.Add("Action", entity.Action, DbType.String);
                    parameters.Add("Active", entity.Active, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
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

        public async Task<int> DeleteAsync(MenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuID", entity.MenuID, DbType.Int32);
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

        public async Task<List<MenuModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<MenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<MenuModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<MenuModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<MenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(MenuModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", entity.MenuID, DbType.Int32);
                    parameters.Add("ParentMenuId", entity.ParentMenuId, DbType.Int32);
                    parameters.Add("SubMenuId", entity.SubMenuId, DbType.Int32);
                    parameters.Add("MenuName", entity.MenuName, DbType.String);
                    parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                    parameters.Add("Controller", entity.Controller, DbType.String);
                    parameters.Add("Action", entity.Action, DbType.String);
                    parameters.Add("Active", entity.Active, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
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

        public async Task<List<MenuModel>> GetMenuByRoleAndUserForInstitute(int roleid, int userid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("UserId", userid, DbType.String);
                    parameters.Add("@Query", 8, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<MenuModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
