using CoreLayout.Models.Exam;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.ExamCourseMapping
{
    public class ExamCourseMappingRepository : BaseRepository, IExamCourseMappingRepository
    {
        public ExamCourseMappingRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ExamCourseMappingModel entity)
        {
            try
            {
                int res = 0;
                var query = "SP_InsertUpdateDelete_ExamCourseMapping";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("PaperSetterPermission", entity.PaperSetterPermission, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    StringBuilder stringBuilder = new StringBuilder();
                    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(ExamCourseMappingModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamCourseMapping";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ECId", entity.ECId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
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

        public async Task<List<ExamCourseMappingModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamCourseMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ExamCourseMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<ExamCourseMappingModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //public async Task<List<ExamCourseMappingModel>> CheckAlreadyAsync(int menuid,int roleid)
        //{
        //    try
        //    {
        //        var query = "SP_InsertUpdateDelete_ExamCourseMapping";
        //        using (var connection = CreateConnection())
        //        {
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("MenuId", menuid, DbType.Int32);
        //            parameters.Add("RoleId", roleid, DbType.Int32);
        //            parameters.Add("@Query", 6, DbType.Int32);
        //            var list = await SqlMapper.QueryAsync<ExamCourseMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

        //            return (List<ExamCourseMappingModel>)list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        public async Task<ExamCourseMappingModel> GetByIdAsync(int ECId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamCourseMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ECId", ECId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ExamCourseMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ExamCourseMappingModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamCourseMapping";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ECId", entity.ECId, DbType.Int32);
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("PaperSetterPermission", entity.PaperSetterPermission, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
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
    }
}
