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

namespace CoreLayout.Repositories.UserManagement.Registration
{
    public class RegistrationRepository : BaseRepository, IRegistrationRepository
    {
        public RegistrationRepository(IConfiguration configuration)
   : base(configuration)
        { }
        public async Task<int> CreateAsync(RegistrationModel registrationModel)
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
                        var query = "SP_CRUD_USERLOGIN";
                        var res = 0;
                        var res1 = 0;
                        int newID = 0;
                        registrationModel.IsRoleActive = 1;//2nd tabl2
                        registrationModel.IsMainRole = 0;//2nd tabl2
                        registrationModel.IsUserActive = 1;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserName", registrationModel.UserName, DbType.String);
                        parameters.Add("LoginID", registrationModel.LoginID, DbType.String);
                        parameters.Add("MobileNo", registrationModel.MobileNo, DbType.String);
                        parameters.Add("EmailID", registrationModel.EmailID, DbType.String);
                        parameters.Add("Salt", registrationModel.Salt, DbType.String);
                        parameters.Add("SaltedHash", registrationModel.SaltedHash, DbType.String);
                        parameters.Add("IsUserActive", registrationModel.IsUserActive, DbType.String);
                        parameters.Add("RefType", registrationModel.RefType, DbType.String);
                        parameters.Add("IPAddress", registrationModel.IPAddress, DbType.String);
                        parameters.Add("InstituteId", registrationModel.InstituteId, DbType.Int32);
                        parameters.Add("ReturnUserId", registrationModel.ReturnUserId, DbType.Int32, direction: ParameterDirection.Output);
                       

                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        newID = parameters.Get<int>("ReturnUserId");
                        //parameters.Get("ReturnUserId", registrationModel.ReturnUserId, DbType.Int32);


                        if (res == 1 && newID != 0)
                        {
                            parameters.Add("RoleId", registrationModel.RoleId, DbType.Int32);
                            parameters.Add("IsRoleActive", registrationModel.IsRoleActive, DbType.Int32);
                            parameters.Add("IsMainRole", registrationModel.IsMainRole, DbType.Int32);
                            parameters.Add("@RoleUserId", newID, DbType.Int32);
                            parameters.Add("IPAddress", registrationModel.IPAddress, DbType.String);
                            parameters.Add("@UserId", registrationModel.CreatedBy, DbType.Int32);
                            parameters.Add("@Query", 7, DbType.Int32);
                            res1 = await SqlMapper.ExecuteAsync(connection, query, parameters,tran, commandType: CommandType.StoredProcedure);
                        }

                        if (res == 1 && res1 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res1;

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

       
        //public void InsertChildRecord(RegistrationModel registrationModel,int retrunUserId)
        //{
        //    try
        //    {
        //        var res = 0;
        //        var query = "SP_CRUD_USERLOGIN";
        //        using (var connection = CreateConnection())
        //        {
        //            //var UserId = connection.ExecuteScalar("SELECT Max(UserId)+1 AS ID FROM UserLogin");
        //            //if (UserId == null)
        //            //{
        //            //    UserId = 1;
        //            //}
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("roleid", registrationModel.RoleId, DbType.Int32);
        //            //parameters.Add("UserRoleId", UserId, DbType.Int32);
        //            parameters.Add("RoleUserId", retrunUserId, DbType.Int32);
        //            parameters.Add("IsRoleActive", registrationModel.IsRoleActive, DbType.Int32);
        //            parameters.Add("IsMainRole", registrationModel.IsMainRole, DbType.Int32);
        //            parameters.Add("IPAddress", ":11");
        //            parameters.Add("UserId", registrationModel.CreatedBy, DbType.String);
        //            parameters.Add("@Query", 7, DbType.Int32);
        //            res = SqlMapper.Execute(connection, query, parameters, commandType: CommandType.StoredProcedure);



        //            //if (registrationModel.RoleName == "Administrator")
        //            //{
        //            //    StringBuilder stringBuilder = new StringBuilder();
        //            //    foreach (int roleid in registrationModel.roleid)
        //            //    {
        //            //        parameters.Add("RoleId", roleid);
        //            //        res = SqlMapper.Execute(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    parameters.Add("RoleId", 16);
        //            //    res = SqlMapper.Execute(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        public async Task<int> DeleteAsync(RegistrationModel entity)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    entity.IsUserActive = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("IsUserActive", entity.IsUserActive);
                    parameters.Add("UserID", entity.UserID, DbType.Int32);
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

        public async Task<List<RegistrationModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<RegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RegistrationModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", id, DbType.Int32);
                    parameters.Add("@Query", 9, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(RegistrationModel registrationModel)
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
                        var query = "SP_CRUD_USERLOGIN";
                        var res = 0;
                        var res1 = 0;
                        int newID = 0;
                        registrationModel.IsRoleActive = 1;//2nd tabl2
                        registrationModel.IsMainRole = 0;//2nd tabl2
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserID", registrationModel.UserID, DbType.Int32);
                        parameters.Add("UserName", registrationModel.UserName, DbType.String);
                        parameters.Add("LoginID", registrationModel.LoginID, DbType.String);
                        parameters.Add("MobileNo", registrationModel.MobileNo, DbType.String);
                        parameters.Add("EmailID", registrationModel.EmailID, DbType.String);
                        if (registrationModel.IsPasswordChange == "1")
                        {
                            parameters.Add("IsPasswordChange", 1, DbType.Int32);
                            parameters.Add("Salt", registrationModel.Salt, DbType.String);
                            parameters.Add("SaltedHash", registrationModel.SaltedHash, DbType.String);
                        }
                        else
                        {
                            parameters.Add("IsPasswordChange", 0, DbType.Int32);
                        }
                        parameters.Add("IsUserActive", registrationModel.IsUserActive, DbType.String);
                        parameters.Add("RefType", registrationModel.RefType, DbType.String);
                        parameters.Add("IPAddress", registrationModel.IPAddress);
                        parameters.Add("InstituteId", registrationModel.InstituteId);
                        parameters.Add("ReturnUserId", registrationModel.ReturnUserId, DbType.Int32);
                        parameters.Add("@Query", 2, DbType.Int32);

                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        //newID = parameters.Get<int>("ReturnUserId");
                        //parameters.Get("ReturnUserId", registrationModel.ReturnUserId, DbType.Int32);


                        if (res == 1)
                        {
                            parameters.Add("UserRoleId", registrationModel.UserRoleId, DbType.Int32);
                            parameters.Add("RoleId", registrationModel.RoleId, DbType.Int32);
                            parameters.Add("IsRoleActive", registrationModel.IsRoleActive, DbType.Int32);
                            parameters.Add("IsMainRole", registrationModel.IsMainRole, DbType.Int32);
                            parameters.Add("RoleUserId", registrationModel.UserID, DbType.Int32);
                            parameters.Add("IPAddress", registrationModel.IPAddress, DbType.String);
                            parameters.Add("UserId", registrationModel.ModifiedBy, DbType.Int32);
                            parameters.Add("@Query", 11, DbType.Int32);
                            res1 = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        }

                        if (res == 1 && res1 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res1;

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

        public async Task<List<RegistrationModel>> GetAllInstituteAsync()
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 10, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<RegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangePassword(RegistrationModel entity)
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
                        var query = "SP_CRUD_USERLOGIN";
                        var res = 0;
                        var res1 = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserID", entity.UserID, DbType.Int32);
                        parameters.Add("Salt", entity.Salt, DbType.String);
                        parameters.Add("SaltedHash", entity.SaltedHash, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailID", entity.EmailID, DbType.String);
                       // parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
                        parameters.Add("@Query", 12, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        if (res == 1)
                        {
                            DynamicParameters parameters1 = new DynamicParameters();
                            parameters1.Add("UserID", entity.UserID, DbType.Int32);
                            parameters1.Add("UserName", entity.UserName, DbType.String);
                            parameters1.Add("RoleId", entity.RoleId, DbType.Int32);
                            parameters1.Add("Salt", entity.OldSalt, DbType.String);
                            parameters1.Add("SaltedHash", entity.OldSaltedHash, DbType.String);
                            parameters1.Add("MobileNo", entity.MobileNo, DbType.String);
                            parameters1.Add("EmailID", entity.EmailID, DbType.String);
                            parameters1.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                            parameters1.Add("IPAddress", entity.IPAddress, DbType.String);
                            parameters1.Add("@Query", 13, DbType.Int32);
                            res1 = await SqlMapper.ExecuteAsync(connection, query, parameters1, tran, commandType: CommandType.StoredProcedure);
                        }

                        if (res1 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res1;

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

        public async Task<int> InsertEmailSMSHistory(RegistrationModel entity)
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
                        var query = "SP_CRUD_USERLOGIN";
                        var res = 0;
                        var res1 = 0;
                        var res2 = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserID", entity.UserID, DbType.Int32);
                        parameters.Add("Salt", entity.Salt, DbType.String);
                        parameters.Add("SaltedHash", entity.SaltedHash, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailID", entity.EmailID, DbType.String);
                        parameters.Add("@Query", 12, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        if (res == 1)
                        {
                            DynamicParameters parameters1 = new DynamicParameters();
                            parameters1.Add("UserID", entity.UserID, DbType.Int32);
                            parameters1.Add("UserName", entity.UserName, DbType.String);
                            parameters1.Add("RoleId", entity.RoleId, DbType.Int32);
                            parameters1.Add("Salt", entity.Salt, DbType.String);
                            parameters1.Add("SaltedHash", entity.SaltedHash, DbType.String);
                            parameters1.Add("MobileNo", entity.MobileNo, DbType.String);
                            parameters1.Add("EmailID", entity.EmailID, DbType.String);
                            parameters1.Add("CreatedBy", entity.UserID, DbType.Int32);
                            parameters1.Add("IPAddress", entity.IPAddress, DbType.String);
                            parameters1.Add("@Query", 13, DbType.Int32);
                            res1 = await SqlMapper.ExecuteAsync(connection, query, parameters1, tran, commandType: CommandType.StoredProcedure);
                        }

                        if (res1 == 1)
                        {
                            DynamicParameters parameters2 = new DynamicParameters();
                            parameters2.Add("MobileNo", entity.MobileNo, DbType.String);
                            parameters2.Add("EmailID", entity.EmailID, DbType.String);
                            parameters2.Add("UserName", entity.UserName, DbType.String);
                            parameters2.Add("LoginID", entity.LoginID, DbType.String);
                            parameters2.Add("IPAddress", entity.IPAddress, DbType.String);
                            parameters2.Add("CreatedBy", entity.UserID, DbType.Int32);
                            parameters2.Add("MobileReminder", entity.MobileReminder, DbType.String);
                            parameters2.Add("EmailReminder", entity.EmailReminder, DbType.String);
                            parameters2.Add("Remarks", entity.Remarks, DbType.String);
                            parameters2.Add("@Query", 14, DbType.Int32);
                            res2 = await SqlMapper.ExecuteAsync(connection, query, parameters2, tran, commandType: CommandType.StoredProcedure);
                        }
                        if (res2 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res2;

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

        public async Task<RegistrationModel> ForgetPassword(string emailid, string mobileno, string loginid)
        {
            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("LoginID", loginid, DbType.String);
                    parameters.Add("EmailID", emailid, DbType.String);
                    parameters.Add("MobileNo", mobileno, DbType.String);
                    parameters.Add("@Query", 15, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
