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

namespace CoreLayout.Repositories.WRN.WRNCourseDetails
{
    public class WRNCourseDetailsRepository : BaseRepository, IWRNCourseDetailsRepository
    {
        public WRNCourseDetailsRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(WRNCourseDetailsModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var res = 0;
                        //var res1 = 0;
                        var query = string.Empty;
                        //var query1 = string.Empty;
                        entity.IsRecordDeleted = 0;
                        query = "Usp_WRNCourseDetails";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("DistrictId", entity.DistrictId, DbType.Int32);
                        parameters.Add("InstituteId", entity.InstituteId, DbType.Int32);
                        parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        //if (res == 1)
                        //{
                        //    DynamicParameters parameters1 = new DynamicParameters();
                        //    query1 = "Usp_SequenceGenerate";
                        //    parameters1.Add("Id", 8, DbType.Int32);
                        //    parameters1.Add("@Query", 2, DbType.Int32);
                        //    parameters1.Add("@UserId", entity.CreatedBy, DbType.Int32);
                        //    res1 = await SqlMapper.ExecuteAsync(connection, query1, parameters1, tran, commandType: CommandType.StoredProcedure);
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
        public async Task<int> DeleteAsync(WRNCourseDetailsModel entity)
        {
            try
            {
                var query = "Usp_WRNCourseDetails";
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

        public async Task<List<WRNCourseDetailsModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_WRNCourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<WRNCourseDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<WRNCourseDetailsModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<WRNCourseDetailsModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "Usp_WRNCourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<WRNCourseDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateAsync(WRNCourseDetailsModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var res = 0;
                        //var res1 = 0;
                        var query = string.Empty;
                        //var query1 = string.Empty;
                        entity.IsRecordDeleted = 0;
                        query = "Usp_WRNCourseDetails";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        parameters.Add("DistrictId", entity.DistrictId, DbType.Int32);
                        parameters.Add("InstituteId", entity.InstituteId, DbType.Int32);
                        parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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
    }
}
