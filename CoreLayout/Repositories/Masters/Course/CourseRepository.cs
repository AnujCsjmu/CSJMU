using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Course
{
    public class CourseRepository: BaseRepository, ICourseRepository
    {
        public CourseRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CourseModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseCode", entity.CourseCode, DbType.String);
                    parameters.Add("CourseName", entity.CourseName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("MinDuration", entity.MinDuration, DbType.Int32);
                    parameters.Add("MaxDuration", entity.MaxDuration, DbType.Int32);
                    parameters.Add("ProgramId", entity.ProgramId, DbType.Int32);
                    parameters.Add("CourseTypeId", entity.CourseTypeId, DbType.Int32);
                    parameters.Add("CourseExamType", entity.CourseExamType, DbType.String);
                    parameters.Add("ShortName", entity.ShortName, DbType.String);
                    parameters.Add("AffOnline", entity.AffOnline, DbType.String);
                    parameters.Add("UserId", entity.CreatedBy, DbType.Int32);
                    parameters.Add("NominalDuration", entity.NominalDuration, DbType.Int32);
                    parameters.Add("OnCampus", entity.OnCampus, DbType.Int32);
                    parameters.Add("OffCampus", entity.OffCampus, DbType.Int32);
                    parameters.Add("CertificateName", entity.CertificateName, DbType.String);
                    parameters.Add("CertificateHindiName", entity.CertificateHindiName, DbType.String);
                    parameters.Add("DisplayName", entity.DisplayName, DbType.String);
                    parameters.Add("IsNEP", entity.IsNEP, DbType.Int32);
                    parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                    parameters.Add("IsLateralAllowed", entity.IsLateralAllowed, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    var res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(CourseModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 1;
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
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

        public async Task<List<CourseModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CourseModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CourseModel> GetByIdAsync(int CourseID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseID", CourseID, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<CourseModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CourseModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
                    parameters.Add("CourseCode", entity.CourseCode, DbType.String);
                    parameters.Add("CourseName", entity.CourseName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("MinDuration", entity.MinDuration, DbType.Int32);
                    parameters.Add("MaxDuration", entity.MaxDuration, DbType.Int32);
                    parameters.Add("ProgramId", entity.ProgramId, DbType.Int32);
                    parameters.Add("CourseTypeId", entity.CourseTypeId, DbType.Int32);
                    parameters.Add("CourseExamType", entity.CourseExamType, DbType.String);
                    parameters.Add("ShortName", entity.ShortName, DbType.String);
                    parameters.Add("AffOnline", entity.AffOnline, DbType.String);
                    parameters.Add("UserId", entity.ModifiedBy, DbType.Int32);
                    parameters.Add("NominalDuration", entity.NominalDuration, DbType.Int32);
                    parameters.Add("OnCampus", entity.OnCampus, DbType.Int32);
                    parameters.Add("OffCampus", entity.OffCampus, DbType.Int32);
                    parameters.Add("CertificateName", entity.CertificateName, DbType.String);
                    parameters.Add("CertificateHindiName", entity.CertificateHindiName, DbType.String);
                    parameters.Add("DisplayName", entity.DisplayName, DbType.String);
                    parameters.Add("IsNEP", entity.IsNEP, DbType.Int32);
                    parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                    parameters.Add("IsLateralAllowed", entity.IsLateralAllowed, DbType.Int32);
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
        public async Task<List<CourseTypeModel>> GetAllCourseType()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseTypeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<CourseTypeModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CourseModel>> GetAllCourseByInstitute(int instituteId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Course";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@@InstituteId", instituteId, DbType.Int32);
                    parameters.Add("@Query", 7, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CourseModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
