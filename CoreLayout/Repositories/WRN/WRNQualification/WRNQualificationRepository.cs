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

namespace CoreLayout.Repositories.WRN.WRNQualification
{
    public class WRNQualificationRepository : BaseRepository, IWRNQualificationRepository
    {
        public WRNQualificationRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(WRNQualificationModel entity)
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
                        query = "Usp_WRNQualification";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("QualificationId", entity.QualificationId, DbType.Int32);
                        parameters.Add("QualificationDetails", entity.QualificationDetails, DbType.String);
                        parameters.Add("BoardUniversityId", entity.BoardUniversityId, DbType.Int32);
                        parameters.Add("BoardUniversityDetails", entity.BoardUniversityDetails, DbType.String);
                        parameters.Add("ResultStatus", entity.ResultStatus, DbType.String);
                        parameters.Add("PassingYear", entity.PassingYear, DbType.Int32);
                        parameters.Add("PassingMonth", entity.PassingMonth, DbType.Int32);
                        parameters.Add("MarksCriteria", entity.MarksCriteria, DbType.String);
                        parameters.Add("PercentagOfMarksObtained", entity.PercentagOfMarksObtained, DbType.String);
                        parameters.Add("DivisionClassGrade", entity.DivisionClassGrade, DbType.String);
                        parameters.Add("Subjects", entity.Subjects, DbType.String);
                      //parameters.Add("BoardUniversity", entity.BoardUniversity, DbType.String);
                        parameters.Add("ResultCriteria", entity.ResultCriteria, DbType.String);
                        parameters.Add("OtherUniversity", entity.OtherUniversity, DbType.String);
                        parameters.Add("QualificationType", entity.QualificationType, DbType.String);
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

        public async Task<int> DeleteAsync(WRNQualificationModel entity)
        {
            try
            {
                var query = "Usp_WRNQualification";
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

        public async Task<List<WRNQualificationModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<WRNQualificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<WRNQualificationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<WRNQualificationModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<WRNQualificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateAsync(WRNQualificationModel entity)
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
                        query = "Usp_WRNQualification";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("QualificationId", entity.QualificationId, DbType.Int32);
                        parameters.Add("QualificationDetails", entity.QualificationDetails, DbType.String);
                        parameters.Add("BoardUniversityId", entity.BoardUniversityId, DbType.Int32);
                        parameters.Add("BoardUniversityDetails", entity.BoardUniversityDetails, DbType.String);
                        parameters.Add("ResultStatus", entity.ResultStatus, DbType.String);
                        parameters.Add("PassingYear", entity.PassingYear, DbType.Int32);
                        parameters.Add("PassingMonth", entity.PassingMonth, DbType.Int32);
                        parameters.Add("PercentagOfMarksObtained", entity.PercentagOfMarksObtained, DbType.Decimal);
                        parameters.Add("DivisionClassGrade", entity.DivisionClassGrade, DbType.String);
                        parameters.Add("Subjects", entity.Subjects, DbType.String);
                      //parameters.Add("BoardUniversity", entity.BoardUniversity, DbType.String);
                        parameters.Add("ResultCriteria", entity.ResultCriteria, DbType.String);
                        parameters.Add("OtherUniversity", entity.OtherUniversity, DbType.String);
                        parameters.Add("QualificationType", entity.QualificationType, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
                        parameters.Add("@Query", 2, DbType.Int32);
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

        public async Task<List<EducationalQualificationModel>> GetAllEducationalQualification()
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<EducationalQualificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<EducationalQualificationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string Type)
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@QualificationType", Type, DbType.String);
                    parameters.Add("@Query", 7, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<BoardUniversityModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<BoardUniversityModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<BoardUniversityModel>> GetAllBoardUniversityType()
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 8, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<BoardUniversityModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<BoardUniversityModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id)
        {
            try
            {
                var query = "Usp_WRNQualification";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Id", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<WRNQualificationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<WRNQualificationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
