using CoreLayout.Models.PCP;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPSendReminder
{
    public class PCPSendReminderRepository : BaseRepository, IPCPSendReminderRepository
    {
        public PCPSendReminderRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<List<PCPRegistrationModel>> GetAllAssingedQP()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendReminder";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 1, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPRegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PCPRegistrationModel>> GetReminderById(int UserID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendReminder";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserID", UserID, DbType.Int32);
                    parameters.Add("@Query", 2, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPRegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateReminderAsync(PCPRegistrationModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPSendReminder";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailID", entity.EmailID, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);

                        parameters.Add("IsMobileReminder", entity.IsEmailReminder, DbType.String);
                        parameters.Add("IsEmailReminder", entity.IsMobileReminder, DbType.String);
                        parameters.Add("QPId", entity.QPId, DbType.Int32);
                        parameters.Add("@Query", 3, DbType.Int32);
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
