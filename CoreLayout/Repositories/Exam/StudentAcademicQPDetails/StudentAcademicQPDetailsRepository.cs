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

namespace CoreLayout.Repositories.Exam.StudentAcademicQPDetails
{
    public class StudentAcademicQPDetailsRepository : BaseRepository, IStudentAcademicQPDetailsRepository
    {
        public StudentAcademicQPDetailsRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(StudentAcademicQPDetailsModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        int res = 0;
                        entity.IsRecordDeleted = 0;
                        var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("AcademicId", entity.AcademicId, DbType.Int32);
                        parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                        parameters.Add("SubjectId", entity.SubjectId, DbType.Int32);
                        parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                        parameters.Add("SyllabusSessionId", entity.SyllabusSessionId, DbType.Int32);
                        //parameters.Add("QPId", entity.QPId, DbType.Int32);
                        parameters.Add("QPCode", entity.QPCode, DbType.String);
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        
                        foreach (int qpid in entity.QPListForInsert)
                        {
                            parameters.Add("@Query", 1, DbType.Int32);
                            parameters.Add("QPId", qpid, DbType.Int32);
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

        public async Task<int> DeleteAsync(StudentAcademicQPDetailsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 1;
                var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentAcademicQPId", entity.StudentAcademicQPId, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
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

        public async Task<List<StudentAcademicQPDetailsModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<StudentAcademicQPDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<StudentAcademicQPDetailsModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<StudentAcademicQPDetailsModel> GetByIdAsync(int StudentAcademicQPId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentAcademicQPId", StudentAcademicQPId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<StudentAcademicQPDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(StudentAcademicQPDetailsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentAcademicQPId", entity.StudentAcademicQPId, DbType.Int32);
                    parameters.Add("AcademicId", entity.AcademicId, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("SubjectId", entity.SubjectId, DbType.Int32);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("SyllabusSessionId", entity.SyllabusSessionId, DbType.Int32);
                    parameters.Add("QPId", entity.QPId, DbType.Int32);
                    parameters.Add("QPCode", entity.QPCode, DbType.String);
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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
        public async Task<List<StudentAcademicQPDetailsModel>> GetFilterStudentAcademicsQPData(int academicid,int courseid, int subjectid, int semyearid, int syllabussessionid, int examid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_StudentAcademicsQPDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@AcademicId", academicid, DbType.Int32);
                    parameters.Add("@CourseId", courseid, DbType.Int32);
                    parameters.Add("@SubjectId", subjectid, DbType.Int32);
                    parameters.Add("@SemYearId", semyearid, DbType.Int32);
                    parameters.Add("@SyllabusSessionId", syllabussessionid, DbType.Int32);
                    parameters.Add("@ExamId", examid, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<StudentAcademicQPDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<StudentAcademicQPDetailsModel>)lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
