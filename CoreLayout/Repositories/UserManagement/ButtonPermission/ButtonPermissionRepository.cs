using CoreLayout.Enum;
using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.ButtonPermission
{
    public class ButtonPermissionRepository : BaseRepository, IButtonPermissionRepository
    {
        public ButtonPermissionRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ButtonPermissionModel entity)
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
                        var query = "Usp_Crud_ButtonPermission";
                        var res = 0;

                        entity.IsRecordDeleted = 0;
                        //entity.URL = "/" + entity.Controller + "/" + entity.Action;
                        DynamicParameters parameters = new DynamicParameters();
                        //parameters.Add("Controller", entity.Controller, DbType.String);
                        //parameters.Add("Action", entity.Action, DbType.String);
                        //parameters.Add("URL", entity.URL, DbType.String);
                        parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                        parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                       // parameters.Add("IsRecordActive", entity.IsRecordActive, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("@Query", 1, DbType.Int32);
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (int buttonid in entity.ButtonList)
                        {
                            parameters.Add("ButtonId", buttonid, DbType.Int32);
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
        public async Task<int> DeleteAsync(ButtonPermissionModel entity)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.Int32);
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

        public async Task<List<ButtonPermissionModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ButtonPermissionModel> GetByIdAsync(int Id)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", Id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ButtonPermissionModel entity)
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
                        var query = "Usp_Crud_ButtonPermission";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        //entity.URL = "/" + entity.Controller + "/" + entity.Action;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        //parameters.Add("Controller", entity.Controller, DbType.String);
                        //parameters.Add("Action", entity.Action, DbType.String);
                        //parameters.Add("URL", entity.URL, DbType.String);
                        parameters.Add("MenuId", entity.MenuId, DbType.Int32);
                        parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("ButtonId", entity.ButtonId, DbType.Int32);
                        parameters.Add("RoleId", entity.RoleId, DbType.Int32);
                       // parameters.Add("IsRecordActive", entity.IsRecordActive, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("@Query", 2, DbType.Int32);
                        StringBuilder stringBuilder = new StringBuilder();
                        //foreach (int buttonid in entity.ButtonList)
                        //{
                        //   parameters.Add("ButtonId", buttonid, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        //}
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
        public async Task<List<RegistrationModel>> GetAllUserAsync(int roleid)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleid, DbType.String);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<RegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionUserWiseAsync(int userid)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserId", userid, DbType.String);
                    parameters.Add("@Query", 12, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionMenuWiseAsync(int menuid)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MenuId", menuid, DbType.String);
                    parameters.Add("@Query", 14, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonPermissionModel>> DistinctButtonPermissionAsync()
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 13, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonActionPermissionAsync(ViewAction viewAction, int userid, int roleid, string controller)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonName", viewAction.ToString(), DbType.String);
                    parameters.Add("UserId", userid, DbType.Int32);
                    parameters.Add("RoleId", roleid, DbType.Int32);
                    parameters.Add("Controller", controller, DbType.String);
                   // parameters.Add("Action", action, DbType.String);
                    parameters.Add("@Query", 10, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonPermissionModel>> CheckAllButtonActionPermissionAsync(int buttonid, int userid, int roleid, string controller, string action)
        {
            try
            {
                var query = "Usp_Crud_ButtonPermission";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonId", buttonid, DbType.Int32);
                    parameters.Add("UserId", userid, DbType.Int32);
                    parameters.Add("RoleId", roleid, DbType.Int32);
                    parameters.Add("Controller", controller, DbType.String);
                    parameters.Add("Action", action, DbType.String);
                    parameters.Add("@Query", 11, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonPermissionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<ButtonPermissionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

