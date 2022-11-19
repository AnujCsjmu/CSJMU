using CoreLayout.Models.Exam;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.SubjectProfile
{
    public class SubjectProfileRepository : BaseRepository, ISubjectProfileRepository
    {
        public SubjectProfileRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(SubjectProfileModel entity)
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
                        var res = 0;
                        var res1 = 0;
                        int newID = 0;
                        var query = "SP_InsertUpdateDelete_SubjectProfile";
                        entity.IsRecordDeleted = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                        parameters.Add("CourseID", entity.CourseId, DbType.Int32);
                        parameters.Add("FacultyID", entity.FacultyID, DbType.Int32);
                        parameters.Add("IsOtherFaculty", entity.IsOtherFaculty, DbType.Boolean);
                        parameters.Add("OtherFacultyId", entity.OtherFacultyId, DbType.Int32);
                        parameters.Add("IsAddMinor", entity.IsAddMinor, DbType.Boolean);
                        parameters.Add("MinorFacultyId", entity.MinorFacultyId, DbType.Int32);
                        parameters.Add("MinorSubjectId", entity.MinorSubjectId, DbType.Int32);
                        parameters.Add("VocationalSubjectId", entity.VocationalSubjectId, DbType.Int32);
                        parameters.Add("CoCurricularSubjectId", entity.CoCurricularSubjectId, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("ReturnSubjectProfileId", entity.ReturnSubjectProfileId, DbType.Int32, direction: ParameterDirection.Output);
                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        newID = parameters.Get<int>("ReturnSubjectProfileId");

                        if (res == 1 && newID != 0)
                        {
                            foreach (int subjectid in entity.SubjectList)
                            {
                                parameters.Add("SubjectProfileId", newID, DbType.Int32);
                                parameters.Add("SubjectId", subjectid, DbType.Int32);
                                parameters.Add("@Query", 6, DbType.Int32);
                                res1 = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                            }
                        }
                        if (res == 1 && res1 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return res1;
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

        public async Task<int> DeleteAsync(SubjectProfileModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubjectProfileId", entity.SubjectProfileId, DbType.Int32);
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

        public async Task<List<SubjectProfileModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<SubjectProfileModel> GetByIdAsync(int SubjectProfileId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubjectProfileId", SubjectProfileId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(SubjectProfileModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("SubjectProfileId", entity.SubjectProfileId, DbType.Int32);
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("CourseID", entity.CourseId, DbType.Int32);
                    parameters.Add("FacultyID", entity.FacultyID, DbType.Int32);
                    parameters.Add("IsOtherFaculty", entity.IsOtherFaculty, DbType.Int32);
                    parameters.Add("OtherFacultyId", entity.OtherFacultyId, DbType.Int32);
                    parameters.Add("IsAddMinor", entity.IsAddMinor, DbType.Int32);
                    parameters.Add("MinorFacultyId", entity.MinorFacultyId, DbType.Int32);
                    parameters.Add("MinorSubjectId", entity.MinorSubjectId, DbType.Int32);
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
        public async Task<List<SubjectProfileModel>> GetCourseFromAff_SubjectProfile(int sessioninstituteid, int sessionid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteId", sessioninstituteid, DbType.Int32);
                    parameters.Add("SessionId", sessionid, DbType.Int32);
                    parameters.Add("@Query", 7, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<SubjectProfileModel>> GetFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteId", sessioninstituteid, DbType.Int32);
                    parameters.Add("SessionId", sessionid, DbType.Int32);
                    parameters.Add("CourseID", courseid, DbType.Int32);
                    parameters.Add("@Query", 8, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<SubjectProfileModel>> GetOtherFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteId", sessioninstituteid, DbType.Int32);
                    parameters.Add("SessionId", sessionid, DbType.Int32);
                    // parameters.Add("CourseID", courseid, DbType.Int32);
                    parameters.Add("@Query", 9, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<SubjectProfileModel>> GetSubjectFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteId", sessioninstituteid, DbType.Int32);
                    parameters.Add("SessionId", sessionid, DbType.Int32);
                    parameters.Add("CourseID", courseid, DbType.Int32);
                    parameters.Add("@Query", 10, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<SubjectProfileModel>> GetMinorFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteId", sessioninstituteid, DbType.Int32);
                    parameters.Add("SessionId", sessionid, DbType.Int32);
                    // parameters.Add("CourseID", courseid, DbType.Int32);
                    parameters.Add("@Query", 11, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SubjectProfileModel>> GetSubjectFromSubjectProfileMapping()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_SubjectProfile";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 12, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SubjectProfileModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<SubjectProfileModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
