using CoreLayout.Models.PCP;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPUploadOldPaper
{
    public class PCPUploadOldPaperRepository : BaseRepository, IPCPUploadOldPaperRepository
    {
        public PCPUploadOldPaperRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(PCPUploadOldPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("QPId", entity.QPId, DbType.String);
                        parameters.Add("CourseID", entity.CourseId, DbType.Int32);
                        parameters.Add("SubjectId", entity.BranchId, DbType.Int32);
                        parameters.Add("SyllabusId", entity.SessionId, DbType.Int32);
                        parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("OldPaperPath", entity.OldPaperPath, DbType.String);
                        parameters.Add("OldSyllabusPath", entity.OldSyllabusPath, DbType.String);
                        parameters.Add("OldPatternPath", entity.OldPatternPath, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
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

        public async Task<int> DeleteAsync(PCPUploadOldPaperModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("OldPaperId", entity.OldPaperId, DbType.Int32);
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

        public async Task<List<PCPUploadOldPaperModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPUploadOldPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPUploadOldPaperModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PCPUploadOldPaperModel> GetByIdAsync(int OldPaperId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("OldPaperId", OldPaperId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPUploadOldPaperModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PCPUploadOldPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("OldPaperId", entity.OldPaperId, DbType.String);
                        parameters.Add("QPId", entity.QPId, DbType.String);
                        parameters.Add("CourseID", entity.CourseId, DbType.Int32);
                        parameters.Add("SubjectId", entity.BranchId, DbType.Int32);
                        parameters.Add("SyllabusId", entity.SessionId, DbType.Int32);
                        parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("OldPaperPath", entity.OldPaperPath, DbType.String);
                        parameters.Add("OldSyllabusPath", entity.OldSyllabusPath, DbType.String);
                        parameters.Add("OldPatternPath", entity.OldPatternPath, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
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

        public async Task<int> FinalSubmitAsync(PCPUploadOldPaperModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPUploadOldPaper";
                        var res = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Query", 6, DbType.Int32);
                        String[] array = entity.oldpaperids.Split(",");
                        for (int i = 0; i < array.Length; i++)
                        {
                            parameters.Add("OldPaperId", Convert.ToInt32(array[i]), DbType.Int32);
                            parameters.Add("FinalSubmit", "FinalSubmit", DbType.String);
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
