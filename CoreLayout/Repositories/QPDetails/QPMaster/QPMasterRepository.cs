using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.QPDetails.QPMaster
{
    public class QPMasterRepository : BaseRepository, IQPMasterRepository
    {
        public QPMasterRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(QPMasterModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_QPMaster";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPCode", entity.QPCode, DbType.String);
                    parameters.Add("QPName", entity.QPName, DbType.String);
                    parameters.Add("QPHindiName", entity.QPHindiName, DbType.String);
                    parameters.Add("QPTypeId", entity.QPTypeId, DbType.Int32);
                    parameters.Add("FacultyId", entity.FacultyId, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("SubjectId", entity.BranchId, DbType.Int32);
                    parameters.Add("SubjectType", entity.SubjectType, DbType.String);
                    parameters.Add("OMRCode", entity.OMRCode, DbType.String);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("IsElective", entity.IsElective, DbType.Int32);
                    parameters.Add("Credits", entity.Credits, DbType.String);
                    parameters.Add("InternalMarks", entity.InternalMarks, DbType.Int32);
                    parameters.Add("ExternalMarks", entity.ExternalMarks, DbType.Int32);
                    parameters.Add("IsSingleFaculty", entity.IsSingleFaculty, DbType.Int32);
                    parameters.Add("MainGroup", entity.MainGroup, DbType.String);
                    parameters.Add("SubGroup", entity.SubGroup, DbType.String);
                    parameters.Add("NoOfPaper", entity.NoOfPaper, DbType.Int32);
                    parameters.Add("IsGrade", entity.IsGrade, DbType.String);
                    parameters.Add("GradeId", entity.GradeId, DbType.Int32);
                    parameters.Add("SyllabusId", entity.SyllabusId, DbType.Int32);
                    parameters.Add("ClubingCode", entity.ClubingCode, DbType.String);
                    //parameters.Add("IsQualifyingPaper", entity.IsQualifyingPaper, DbType.Int32);
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

        public async Task<int> DeleteAsync(QPMasterModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPMaster";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPId", entity.QPId, DbType.Int32);
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

        public async Task<List<QPMasterModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPMaster";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<QPMasterModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<QPMasterModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<QPMasterModel> GetByIdAsync(int QPId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPMaster";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPId", QPId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<QPMasterModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(QPMasterModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPMaster";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPId", entity.QPId, DbType.Int32);
                    parameters.Add("QPTypeId", entity.QPTypeId, DbType.Int32);
                    parameters.Add("QPCode", entity.QPCode, DbType.String);
                    parameters.Add("QPName", entity.QPName, DbType.String);
                    parameters.Add("QPHindiName", entity.QPHindiName, DbType.String);
                    parameters.Add("QPTypeId", entity.QPTypeId, DbType.Int32);
                    parameters.Add("FacultyId", entity.FacultyId, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("SubjectId", entity.BranchId, DbType.Int32);
                    parameters.Add("SubjectType", entity.SubjectType, DbType.String);
                    parameters.Add("OMRCode", entity.OMRCode, DbType.String);
                    parameters.Add("SemYearId", entity.SemYearId, DbType.Int32);
                    parameters.Add("IsElective", entity.IsElective, DbType.Int32);
                    parameters.Add("Credits", entity.Credits, DbType.String);
                    parameters.Add("InternalMarks", entity.InternalMarks, DbType.Int32);
                    parameters.Add("ExternalMarks", entity.ExternalMarks, DbType.Int32);
                    parameters.Add("IsSingleFaculty", entity.IsSingleFaculty, DbType.Int32);
                    parameters.Add("MainGroup", entity.MainGroup, DbType.String);
                    parameters.Add("SubGroup", entity.SubGroup, DbType.String);
                    parameters.Add("NoOfPaper", entity.NoOfPaper, DbType.Int32);
                    parameters.Add("IsGrade", entity.IsGrade, DbType.String);
                    parameters.Add("GradeId", entity.GradeId, DbType.Int32);
                    parameters.Add("SyllabusId", entity.SyllabusId, DbType.Int32);
                    parameters.Add("ClubingCode", entity.ClubingCode, DbType.String);
                    parameters.Add("IsQualifyingPaper", entity.IsQualifyingPaper, DbType.Int32);
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
