using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.CourseDetails
{
    public class CourseDetailsRepository : BaseRepository, ICourseDetailsRepository
    {
        public CourseDetailsRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CourseDetailsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
                    parameters.Add("PramotionType", entity.PramotionType, DbType.String);
                    parameters.Add("CourseDurationType", entity.CourseDurationType, DbType.String);
                    parameters.Add("SessionId", entity.SessionId, DbType.Int32);
                    parameters.Add("NumberOfYear", entity.NumberOfYear, DbType.Int32);
                    parameters.Add("NumberOfSemester", entity.NumberOfSemester, DbType.Int32);
                    parameters.Add("ProcessGroupType", entity.ProcessGroupType, DbType.String);
                    parameters.Add("IsGrade", entity.IsGrade, DbType.Int32);
                    parameters.Add("IsCreadit", entity.IsCreadit, DbType.Int32);
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

        public async Task<int> DeleteAsync(CourseDetailsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 1;
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseDetailId", entity.CourseDetailId, DbType.Int32);
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

        public async Task<List<CourseDetailsModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CourseDetailsModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CourseDetailsModel> GetByIdAsync(int CourseDetailId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseDetailId", CourseDetailId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<CourseDetailsModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CourseDetailsModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseDetailId", entity.CourseDetailId, DbType.Int32);
                    parameters.Add("CourseID", entity.CourseID, DbType.Int32);
                    parameters.Add("PramotionType", entity.PramotionType, DbType.String);
                    parameters.Add("CourseDurationType", entity.CourseDurationType, DbType.String);
                    parameters.Add("SessionId", entity.SessionId, DbType.Int32);
                    parameters.Add("NumberOfYear", entity.NumberOfYear, DbType.Int32);
                    parameters.Add("NumberOfSemester", entity.NumberOfSemester, DbType.Int32);
                    parameters.Add("ProcessGroupType", entity.ProcessGroupType, DbType.String);
                    parameters.Add("IsGrade", entity.IsGrade, DbType.Int32);
                    parameters.Add("IsCreadit", entity.IsCreadit, DbType.Int32);
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
        public async Task<List<SessionModel>> GetAllSession()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseDetails";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<SessionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<SessionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
