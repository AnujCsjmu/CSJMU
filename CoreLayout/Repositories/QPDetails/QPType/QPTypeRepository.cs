using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.QPDetails.QPType
{
    public class QPTypeRepository : BaseRepository, IQPTypeRepository
    {
        public QPTypeRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(QPTypeModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_QPType";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPTypeName", entity.QPTypeName, DbType.String);
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

        public async Task<int> DeleteAsync(QPTypeModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPType";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPTypeId", entity.QPTypeId, DbType.Int32);
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

        public async Task<List<QPTypeModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPType";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<QPTypeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<QPTypeModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<QPTypeModel> GetByIdAsync(int QPTypeId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPType";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPTypeId", QPTypeId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<QPTypeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(QPTypeModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_QPType";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("QPTypeId", entity.QPTypeId, DbType.Int32);
                    parameters.Add("QPTypeName", entity.QPTypeName, DbType.String);
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
