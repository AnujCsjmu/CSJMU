using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.Login
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(IConfiguration configuration)
   : base(configuration)
        { }
        //public async Task<RegistrationModel> GetUserDetail(string loginid, string password)
        //{

        //    try
        //    {
        //        var query = "SP_CRUD_USERLOGIN";
        //        using (var connection = CreateConnection())
        //        {
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("LoginID", loginid, DbType.String);
        //            parameters.Add("SaltedHash", password, DbType.String);
        //            parameters.Add("@Query", 6, DbType.Int32);
        //            var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            return lst.FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        public async Task<RegistrationModel> GetUserDetail(string loginid, string password)
        {

            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("LoginID", loginid, DbType.String);
                    parameters.Add("SaltedHash", password, DbType.String);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            //          try
            //          {
            //              var query = @"SELECT a.*,c.RoleName FROM UserLogin a
            //inner join UserRole b on a.UserID = b.RoleUserId
            //inner join Role c on b.RoleId = c.RoleID
            //WHERE LoginID = '"+ loginid + "' and SaltedHash = '"+ password + "'";

            //              var parameters = new DynamicParameters();
            //              parameters.Add("LoginID", loginid, DbType.String);
            //              parameters.Add("SaltedHash", password, DbType.String);

            //              using (var connection = CreateConnection())
            //              {
            //                  return (await connection.QueryFirstOrDefaultAsync<RegistrationModel>(query, parameters));
            //              }
            //          }
            //          catch (Exception ex)
            //          {
            //              throw new Exception(ex.Message, ex);
            //          }
        }
        public async Task<RegistrationModel> GetUserDetailByLoginId(string loginid)
        {

            try
            {
                var query = "SP_CRUD_USERLOGIN";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("LoginID", loginid, DbType.String);
                    //parameters.Add("SaltedHash", password, DbType.String);
                    parameters.Add("@Query", 8, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<RegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
            public async Task<int> CreateAsync(int registrationId, string LoginStatus)
        {
            try
            {
                var query = "Usp_InsertLogin_Information";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Registration_Id", registrationId, DbType.Int32);
                    parameters.Add("Login_Status", LoginStatus, DbType.String);
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
        public async Task<int> UpdateAsync(int registrationId, string Logout_Status)
        {
            try
            {
                var query = "Usp_InsertLogin_Information";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Registration_Id", registrationId, DbType.Int32);
                    parameters.Add("Logout_Status", Logout_Status, DbType.String);
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

        public async Task<ButtonModel> GetButtonDetail()
        {
            try
            {
                var query = "SELECT * FROM ButtonMaster WHERE Status='Active'";

                //var parameters = new DynamicParameters();
                //parameters.Add("@Query", 2, DbType.Int32);
                using (var connection = CreateConnection())
                {
                    return (await connection.QueryFirstOrDefaultAsync<ButtonModel>(query));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
