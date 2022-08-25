using CoreLayout.Models;
using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Common
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        public CommonRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<DashboardModel>> GetByRoleAndUserAsync(int roleid, int userid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Menu";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("UserId", userid, DbType.String);
                    parameters.Add("@Query", 7, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<DashboardModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public async Task<List<ButtonPermissionModel>> GetSingleButtonByRoleAndUserAsync(int roleid, int userid, int buttonId)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("UserId", userid, DbType.String);
                    parameters.Add("ButtonId", buttonId, DbType.String);
                    parameters.Add("@Query", 8, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public async Task<List<ButtonPermissionModel>> GetMultipleButtonByRoleAndUserAsync(int roleid, int userid)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("UserId", userid, DbType.String);
                    parameters.Add("@Query", 9, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
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
