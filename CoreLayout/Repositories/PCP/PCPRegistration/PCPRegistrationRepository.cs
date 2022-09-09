using CoreLayout.Models.PCP;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPRegistration
{
    public class PCPRegistrationRepository : BaseRepository, IPCPRegistrationRepository
    {
        public PCPRegistrationRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(PCPRegistrationModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCP";
                        var res = 0;
                        entity.IsUserActive = 1;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserName", entity.UserName, DbType.String);
                        parameters.Add("LoginID", entity.LoginID, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailID", entity.EmailID, DbType.String);
                        parameters.Add("Salt", entity.Salt, DbType.String);
                        parameters.Add("SaltedHash", entity.SaltedHash, DbType.String);
                        parameters.Add("IsUserActive", entity.IsUserActive, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("InstituteId", entity.InstituteId, DbType.Int32);
                        parameters.Add("QPId", entity.QPId, DbType.Int32);
                        parameters.Add("CourseId", entity.CourseID, DbType.Int32);
                        parameters.Add("SubjectId", entity.BranchID, DbType.Int32);
                        parameters.Add("@Query", 1, DbType.Int32);

                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);

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

        public async Task<int> DeleteAsync(PCPRegistrationModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCP";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", entity.UserID, DbType.Int32);
                    parameters.Add("IsUserActive", entity.IsUserActive, DbType.Int32);
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

        public async Task<List<PCPRegistrationModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCP";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPRegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PCPRegistrationModel> GetByIdAsync(int UserID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCP";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", UserID, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PCPRegistrationModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCP";
                        var res = 0;
                        entity.IsUserActive = 1;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserID", entity.UserID, DbType.Int32);
                        parameters.Add("UserName", entity.UserName, DbType.String);
                        parameters.Add("LoginID", entity.LoginID, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailID", entity.EmailID, DbType.String);
                        if (entity.IsPasswordChange == "1")
                        {
                            parameters.Add("IsPasswordChange", 1, DbType.Int32);
                            parameters.Add("Salt", entity.Salt, DbType.String);
                            parameters.Add("SaltedHash", entity.SaltedHash, DbType.String);
                        }
                        else
                        {
                            parameters.Add("IsPasswordChange", 0, DbType.Int32);
                        }
                        parameters.Add("IsUserActive", entity.IsUserActive, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress);
                        parameters.Add("InstituteId", entity.InstituteId);
                        parameters.Add("QPId", entity.QPId, DbType.Int32);
                        parameters.Add("CourseId", entity.CourseID, DbType.Int32);
                        parameters.Add("SubjectId", entity.BranchID, DbType.Int32);
                        parameters.Add("@Query", 2, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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
