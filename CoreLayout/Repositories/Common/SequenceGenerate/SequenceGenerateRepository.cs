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

namespace CoreLayout.Repositories.Common.SequenceGenerate
{
    public class SequenceGenerateRepository : BaseRepository, ISequenceGenerateRepository
    {
        public SequenceGenerateRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(SequenceGenerateModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "Usp_SequenceGenerate";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("SequenceFor", entity.SequenceFor, DbType.String);
                        parameters.Add("Prefix", entity.Prefix, DbType.String);
                        parameters.Add("SeqLength", entity.SeqLength, DbType.Int32);
                        parameters.Add("Sample", entity.Sample, DbType.String);
                        parameters.Add("CurrentCount", entity.CurrentCount, DbType.Int32);
                        parameters.Add("Description", entity.Description, DbType.String);
                        parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
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

        public async Task<int> DeleteAsync(SequenceGenerateModel entity)
        {
            try
            {
                var query = "Usp_SequenceGenerate";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.Int32);
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

        public async Task<List<SequenceGenerateModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_SequenceGenerate";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SequenceGenerateModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<SequenceGenerateModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<SequenceGenerateModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "Usp_SequenceGenerate";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<SequenceGenerateModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(SequenceGenerateModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var query = "Usp_SequenceGenerate";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        //parameters.Add("SequenceFor", entity.SequenceFor, DbType.String);
                        //parameters.Add("Prefix", entity.Prefix, DbType.String);
                        //parameters.Add("SeqLength", entity.SeqLength, DbType.Int32);
                        //parameters.Add("Sample", entity.Sample, DbType.String);
                        parameters.Add("CurrentCount", entity.CurrentCount, DbType.Int32);
                        //parameters.Add("Description", entity.Description, DbType.String);
                        parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("@Query", 2, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
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
