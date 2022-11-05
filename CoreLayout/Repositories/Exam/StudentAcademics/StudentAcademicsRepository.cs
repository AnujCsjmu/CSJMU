using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.StudentAcademics
{
    public class StudentAcademicsRepository : BaseRepository, IStudentAcademicsRepository
    {
        public StudentAcademicsRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(StudentAcademicsModel entity)
        {
            try
            {
                int res = 0;
                entity.IsActive = true;
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentID", entity.StudentID, DbType.Int32);
                    parameters.Add("InstituteID", entity.InstituteID, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("SubjectId", entity.SubjectId, DbType.Int32);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("SyllabusSessionId", entity.SyllabusSessionId, DbType.Int32);
                    parameters.Add("ExamCenterId", entity.ExamCenterId, DbType.Int32);
                    parameters.Add("ExamCategoryCode", entity.ExamCategoryCode, DbType.String);
                    parameters.Add("ApprovalLetterPath", entity.ApprovalLetterPath, DbType.String);
                    parameters.Add("AcademicSessionId", entity.AcademicSessionId, DbType.Int32);
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("PreviousSessionId", entity.PreviousSessionId, DbType.Int32);
                    parameters.Add("PreviousResultStatus", entity.PreviousResultStatus, DbType.String);
                    parameters.Add("CurrentExamMonth", entity.CurrentExamMonth, DbType.Int32);
                    parameters.Add("OldExamMonth", entity.OldExamMonth, DbType.Int32);
                    parameters.Add("Batch", entity.Batch, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Boolean);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(StudentAcademicsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 1;
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AcademicId", entity.AcademicId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
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

        public async Task<List<StudentAcademicsModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<StudentAcademicsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<StudentAcademicsModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<StudentAcademicsModel> GetByIdAsync(int AcademicId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AcademicId", AcademicId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<StudentAcademicsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(StudentAcademicsModel entity)
        {
            try
            {
                entity.IsActive = true;
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AcademicId", entity.AcademicId, DbType.Int32);
                    parameters.Add("StudentID", entity.StudentID, DbType.Int32);
                    parameters.Add("InstituteID", entity.InstituteID, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("SubjectId", entity.SubjectId, DbType.Int32);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("SyllabusSessionId", entity.SyllabusSessionId, DbType.Int32);
                    parameters.Add("ExamCenterId", entity.ExamCenterId, DbType.Int32);
                    parameters.Add("ExamCategoryCode", entity.ExamCategoryCode, DbType.String);
                    parameters.Add("ApprovalLetterPath", entity.ApprovalLetterPath, DbType.String);
                    parameters.Add("AcademicSessionId", entity.AcademicSessionId, DbType.Int32);
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("PreviousSessionId", entity.PreviousSessionId, DbType.Int32);
                    parameters.Add("PreviousResultStatus", entity.PreviousResultStatus, DbType.String);
                    parameters.Add("CurrentExamMonth", entity.CurrentExamMonth, DbType.Int32);
                    parameters.Add("OldExamMonth", entity.OldExamMonth, DbType.Int32);
                    parameters.Add("Batch", entity.Batch, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Boolean);
                    parameters.Add("ApprovedStatus", entity.ApprovedStatus, DbType.String);
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
        public async Task<List<StudentAcademicsModel>> GetFilterStudentAcademicsData(int? hdnInstituteID, int? hdnCourseId, int? hdnSubjectId, int? hdnSemYearId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademics";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@InstituteID", hdnInstituteID, DbType.Int32);
                    parameters.Add("@CourseId", hdnCourseId, DbType.Int32);
                    parameters.Add("@SubjectId", hdnSubjectId, DbType.Int32);
                    parameters.Add("@SemYearId", hdnSemYearId, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<StudentAcademicsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<StudentAcademicsModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
