using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Common.OTPVerification
{
    public class OTPVerificationRepository : BaseRepository, IOTPVerificationRepository
    {
        public OTPVerificationRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(OTPVerificationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "Usp_OTPVerification";
                        DynamicParameters parameters = new DynamicParameters();
                        //parameters.Add("UniqueId", entity.UniqueId, DbType.Int32);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("OTP", entity.OTP, DbType.String);
                        parameters.Add("IsVerified", entity.IsVerified, DbType.Boolean);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("GeoLocation", entity.GeoLocation, DbType.String);
                        parameters.Add("@Query", 1, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(OTPVerificationModel entity)
        {
            try
            {
                var query = "Usp_OTPVerification";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UniqueId", entity.UniqueId, DbType.Int32);
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

        public async Task<List<OTPVerificationModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_OTPVerification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<OTPVerificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<OTPVerificationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<OTPVerificationModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "Usp_OTPVerification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UniqueId", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<OTPVerificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<OTPVerificationModel> GetOTPVerificationByMobileAsync(string mobileno)
        {
            try
            {
                var query = "Usp_OTPVerification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MobileNo", mobileno, DbType.String);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<OTPVerificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<OTPVerificationModel> GetOTPVerificationByMobileAndOTPAsync(string mobileno,string OTP)
        {
            try
            {
                var query = "Usp_OTPVerification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MobileNo", mobileno, DbType.String);
                    parameters.Add("OTP", OTP, DbType.String);
                    parameters.Add("@Query", 8, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<OTPVerificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateAsync(OTPVerificationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "Usp_OTPVerification";
                        DynamicParameters parameters = new DynamicParameters();
                        //parameters.Add("UniqueId", entity.UniqueId, DbType.Int32);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("OTP", entity.OTP, DbType.String);
                        parameters.Add("IsVerified", entity.IsVerified, DbType.Boolean);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("GeoLocation", entity.GeoLocation, DbType.String);
                        parameters.Add("@Query", 2, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateOTPVerificationByMobileAsync(OTPVerificationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "Usp_OTPVerification";
                        DynamicParameters parameters = new DynamicParameters();
                        //parameters.Add("UniqueId", entity.UniqueId, DbType.Int32);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("OTP", entity.OTP, DbType.String);
                        parameters.Add("IsVerified", entity.IsVerified, DbType.Boolean);
                        //parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        //parameters.Add("GeoLocation", entity.GeoLocation, DbType.String);
                        parameters.Add("@Query", 7, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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
