using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Tehsil
{
    public class TehsilRepository : BaseRepository, ITehsilRepository
    {
        public TehsilRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(TehsilModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Tehsil";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DistrictId", entity.DistrictId, DbType.Int32);
                    parameters.Add("TehsilName", entity.TehsilName, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("UserId", entity.CreatedBy, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
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

        public async Task<int> DeleteAsync(TehsilModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Tehsil";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("TehsilId", entity.TehsilId, DbType.Int32);
                    parameters.Add("UserId", entity.ModifiedBy, DbType.Int32);
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

        public async Task<List<TehsilModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Tehsil";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<TehsilModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<TehsilModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<TehsilModel> GetByIdAsync(int TehsilId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Tehsil";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("TehsilId", TehsilId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<TehsilModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(TehsilModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Tehsil";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DistrictId", entity.DistrictId, DbType.Int32);
                    parameters.Add("TehsilId", entity.TehsilId, DbType.Int32);
                    parameters.Add("TehsilName", entity.TehsilName, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("UserId", entity.ModifiedBy, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
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
