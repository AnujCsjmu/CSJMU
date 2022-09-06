using CoreLayout.Models.QPDetails;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.QPDetails.GradeDefinition
{
    public class GradeDefinitionRepository : BaseRepository, IGradeDefinitionRepository
    {
        public GradeDefinitionRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(GradeDefinitionModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_GradeDef";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("GradeName", entity.GradeName, DbType.String);
                    parameters.Add("StartPercentage", entity.StartPercentage, DbType.Int32);
                    parameters.Add("EndPercentage", entity.EndPercentage, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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

        public async Task<int> DeleteAsync(GradeDefinitionModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_GradeDef";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("GradeId", entity.GradeId, DbType.Int32);
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

        public async Task<List<GradeDefinitionModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_GradeDef";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<GradeDefinitionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<GradeDefinitionModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GradeDefinitionModel> GetByIdAsync(int GradeId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_GradeDef";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPTypeId", GradeId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<GradeDefinitionModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(GradeDefinitionModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_GradeDef";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("GradeId", entity.GradeId, DbType.Int32);
                    parameters.Add("GradeName", entity.GradeName, DbType.String);
                    parameters.Add("StartPercentage", entity.StartPercentage, DbType.Int32);
                    parameters.Add("EndPercentage", entity.EndPercentage, DbType.Int32);
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
    }
}
