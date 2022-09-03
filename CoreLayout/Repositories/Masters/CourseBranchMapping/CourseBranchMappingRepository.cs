using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.CourseBranchMapping
{
    public class CourseBranchMappingRepository : BaseRepository, ICourseBranchMappingRepository
    {
        public CourseBranchMappingRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CourseBranchMappingModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("BranchId", entity.BranchId, DbType.Int32);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("UserId", entity.CreatedBy, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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

        public async Task<int> DeleteAsync(CourseBranchMappingModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CBId", entity.CBId, DbType.Int32);
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

        public async Task<List<CourseBranchMappingModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseBranchMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CourseBranchMappingModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CourseBranchMappingModel> GetByIdAsync(int CBId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CBId", CBId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<CourseBranchMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CourseBranchMappingModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CBId", entity.CBId, DbType.Int32);
                    parameters.Add("CourseId", entity.CourseId, DbType.Int32);
                    parameters.Add("BranchId", entity.BranchId, DbType.Int32);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                    parameters.Add("UserId", entity.ModifiedBy, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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

        public async Task<List<CourseBranchMappingModel>> alreadyExit(int courseid, int branchid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_CourseBranchMapping";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CourseId", courseid, DbType.Int32);
                    parameters.Add("BranchId", branchid, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CourseBranchMappingModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CourseBranchMappingModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
