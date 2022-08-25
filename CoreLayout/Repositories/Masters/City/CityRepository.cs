using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.City
{
    public class CityRepository : BaseRepository, ICityRepository
    {
        public CityRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CityModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_City";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("CityName", entity.CityName, DbType.String);
                    parameters.Add("Status", entity.Status, DbType.String);
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

        public async Task<int> DeleteAsync(CityModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_City";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CityId", entity.CityId, DbType.Int32);
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

        public async Task<List<CityModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_City";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CityModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CityModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CityModel> GetByIdAsync(int CityId)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_City";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CityId", CityId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<CityModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CityModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_City";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("CityId", entity.CityId, DbType.Int32);
                    parameters.Add("CityName", entity.CityName, DbType.String);
                    parameters.Add("Status", entity.Status, DbType.String);
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
