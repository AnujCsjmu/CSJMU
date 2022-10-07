using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using CoreLayout.Models.UserManagement;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPSendPaper
{
    public class PCPSendPaperRepository : BaseRepository, IPCPSendPaperRepository
    {
        public PCPSendPaperRepository(IConfiguration configuration)
: base(configuration)
        { }

        public async Task<int> CreateAsync(PCPSendPaperModel entity)
        {
            using (var connection = CreateConnection())
            {
                var res = 0;
                connection.Open();
                // create the transaction
                // You could use `var` instead of `SqlTransaction`
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                        
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("AgencyId", entity.UserId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        //parameters.Add("PaperId", entity.PaperId, DbType.Int32);
                        //parameters.Add("PaperSetterId", entity.PaperSetterId, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
                        parameters.Add("PaperOpenTime", entity.PaperOpenTime, DbType.DateTime);
                        parameters.Add("StaticIPAddress", entity.StaticIPAddress, DbType.String);
                        parameters.Add("@Query", 1, DbType.Int32);
                        String[] array = entity.paperids.Split(",");
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
                                parameters.Add("PaperId", Convert.ToInt32(array[i]), DbType.Int32);
                                res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                            }
                        }
                       
                        //res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        if (res == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        //return res;
                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        tran.Rollback();

                        // handle the error however you need to.
                       // throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return res;
                }
            }

        }

        public async Task<int> DeleteAsync(PCPSendPaperModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SendPaperId", entity.SendPaperId, DbType.Int32);
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

        public async Task<List<PCPSendPaperModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPSendPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPSendPaperModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PCPSendPaperModel> GetByIdAsync(int SendPaperId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SendPaperId", SendPaperId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPSendPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PCPSendPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("AgencyId", entity.UserId, DbType.Int32);
                        //parameters.Add("PaperId", entity.PaperId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        //parameters.Add("PaperSetterId", entity.PaperSetterId, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
                        parameters.Add("PaperOpenTime", entity.PaperOpenTime, DbType.DateTime);
                        parameters.Add("StaticIPAddress", entity.StaticIPAddress, DbType.String);
                        parameters.Add("@Query", 2, DbType.Int32);
                        //foreach (int paperid in entity.PaperList)
                        //{
                        //    parameters.Add("PaperId", paperid, DbType.Int32);
                        //    res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        //}
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

        public async Task<List<RegistrationModel>> GetAllPCPUser_UploadPaperAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
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

        //get server datetime
        public async Task<PCPSendPaperModel> GetServerDateTime()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 7, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPSendPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> FinalSubmitAsync(PCPSendPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPSendPaperToAgency";
                        var res = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Query", 8, DbType.Int32);
                        String[] array = entity.sendpaperids.Split(",");
                        for (int i = 0; i < array.Length; i++)
                        {
                            parameters.Add("SendPaperId", Convert.ToInt32(array[i]), DbType.Int32);
                            parameters.Add("AcceptedStatus", "Accepted", DbType.String);
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
    }
}
