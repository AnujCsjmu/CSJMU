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

namespace CoreLayout.Repositories.Exam.ExamMaster
{
    public class ExamMasterRepository : BaseRepository, IExamMasterRepository
    {
        public ExamMasterRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ExamMasterModel entity)
        {
            try
            {
                int res = 0;
                var query = "SP_InsertUpdateDelete_ExamMaster";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ExamName", entity.ExamName, DbType.String);
                    parameters.Add("SessionId", entity.SessionId, DbType.Int32);
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

        public async Task<int> DeleteAsync(ExamMasterModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamMaster";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
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

        public async Task<List<ExamMasterModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamMaster";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ExamMasterModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<ExamMasterModel>)list;
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

        public async Task<ExamMasterModel> GetByIdAsync(int ExamId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamMaster";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ExamId", ExamId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<ExamMasterModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ExamMasterModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_ExamMaster";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ExamId", entity.ExamId, DbType.Int32);
                    parameters.Add("ExamName", entity.ExamName, DbType.String);
                    parameters.Add("SessionId", entity.SessionId, DbType.Int32);
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
