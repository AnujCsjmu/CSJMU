using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Degree
{
    public class DegreeRepository : BaseRepository, IDegreeRepository
    {
        public DegreeRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(DegreeModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_Degree";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DegreeName", entity.DegreeName, DbType.String);
                    parameters.Add("DegreeCode", entity.DegreeCode, DbType.String);
                    parameters.Add("DegreeTempleteFile", entity.DegreeTempleteFile, DbType.String);
                    parameters.Add("DegreeSuffix", entity.DegreeSuffix, DbType.String);
                    parameters.Add("DegreePrefix", entity.DegreePrefix, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
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

        public async Task<int> DeleteAsync(DegreeModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Degree";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DegreeId", entity.DegreeId, DbType.Int32);
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

        public async Task<List<DegreeModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Degree";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<DegreeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<DegreeModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DegreeModel> GetByIdAsync(int DegreeId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Degree";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DegreeId", DegreeId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<DegreeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(DegreeModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_Degree";
                using (var connection = CreateConnection())
                {
                
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("DegreeId", entity.DegreeId, DbType.Int32);
                    parameters.Add("DegreeName", entity.DegreeName, DbType.String);
                    parameters.Add("DegreeCode", entity.DegreeCode, DbType.String);
                    parameters.Add("DegreeTempleteFile", entity.DegreeTempleteFile, DbType.String);
                    parameters.Add("DegreeSuffix", entity.DegreeSuffix, DbType.String);
                    parameters.Add("DegreePrefix", entity.DegreePrefix, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
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
