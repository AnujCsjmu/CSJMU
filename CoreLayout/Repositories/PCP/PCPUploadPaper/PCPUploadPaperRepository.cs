using CoreLayout.Models.PCP;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPUploadPaper
{
    public class PCPUploadPaperRepository : BaseRepository, IPCPUploadPaperRepository
    {
        public PCPUploadPaperRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(PCPUploadPaperModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                // create the transaction
                // You could use `var` instead of `SqlTransaction`
                using (var tran = connection.BeginTransaction())
                {
                    var res = 0;
                    var res1 = 0;
                    int newID = 0;
                    try
                    {
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                       
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("PaperPath", entity.PaperPath, DbType.String);
                        parameters.Add("PaperPassword", entity.PaperPassword, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("AssignedQPId", entity.AssignedQPId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("AnswerPath", entity.AnswerPath, DbType.String);

                        parameters.Add("ReturnUploadId", entity.ReturnUploadId, DbType.Int32, direction: ParameterDirection.Output);
                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        newID = parameters.Get<int>("ReturnUploadId");
                        if (res == 1 && newID != 0)
                        {
                            parameters.Add("PaperId", newID, DbType.Int32);
                            parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                            parameters.Add("DownloadStatus", "Upload", DbType.String);
                            parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                            parameters.Add("PaperPath", entity.PaperPath, DbType.String);
                            parameters.Add("@Query", 7, DbType.Int32);
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
                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        tran.Rollback();

                        //// handle the error however you need to.
                        //throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return res1;
                }
            }

        }

        public async Task<int> DeleteAsync(PCPUploadPaperModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("PaperId", entity.PaperId, DbType.Int32);
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

        public async Task<List<PCPUploadPaperModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPUploadPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPUploadPaperModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PCPUploadPaperModel> GetByIdAsync(int PaperId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("PaperId", PaperId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPUploadPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PCPUploadPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("PaperId", entity.PaperId, DbType.Int32);
                        parameters.Add("PaperPath", entity.PaperPath, DbType.String);
                        parameters.Add("PaperPassword", entity.PaperPassword, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("AssignedQPId", entity.AssignedQPId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("AnswerPath", entity.AnswerPath, DbType.String);
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
        public async Task<List<PCPUploadPaperModel>> BothUserPaperUploadAndNotUpload()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPUploadPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPUploadPaperModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> InsertDownloadLogAsync(PCPUploadPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                        var res = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("PaperId", entity.PaperId, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("DownloadStatus", entity.DownloadStatus, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("PaperPath", entity.PaperPath, DbType.String);
                        parameters.Add("@Query", 7, DbType.Int32);

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
        public async Task<int> FinalSubmitAsync(PCPUploadPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                        var res = 0;
                      
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Query", 8, DbType.Int32);
                        String[] array = entity.paperids.Split(",");
                        for (int i = 0; i < array.Length; i++)
                        {
                            parameters.Add("PaperId", Convert.ToInt32(array[i]), DbType.Int32);
                            parameters.Add("FinalSubmit", "FinalSubmit", DbType.String);
                            parameters.Add("PaperPath", entity.PaperPath, DbType.String);
                            parameters.Add("AnswerPath", entity.AnswerPath, DbType.String);
                            parameters.Add("FinalSubmitBy", entity.CreatedBy, DbType.Int32);
                            parameters.Add("PaperPassword", entity.PaperPassword, DbType.String);
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

        public async Task<int> RequestQuestionPassword(PCPUploadPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                        var res = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@PaperId", entity.PaperId, DbType.Int32);
                        parameters.Add("@RequestQuestionPwdStatus", entity.RequestQuestionPwdStatus, DbType.String);
                        parameters.Add("@CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("@Query", 9, DbType.Int32);
                       
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

        public async Task<int> RequestAnswerPassword(PCPUploadPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadPaper";
                        var res = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@PaperId", entity.PaperId, DbType.Int32);
                        parameters.Add("@RequestAnswerPwdStatus", entity.RequestAnswerPwdStatus, DbType.String);
                        parameters.Add("@CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("@Query", 10, DbType.Int32);

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
