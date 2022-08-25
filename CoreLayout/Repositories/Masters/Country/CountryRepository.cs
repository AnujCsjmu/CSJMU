using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Country
{
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CountryModel countryModel)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Country";
                using (var connection = CreateConnection())
                {
                    countryModel.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryName", countryModel.CountryName, DbType.String);
                    parameters.Add("Description", countryModel.Description, DbType.String);
                    parameters.Add("UserId", countryModel.CreatedBy, DbType.Int32);
                    parameters.Add("IPAddress", countryModel.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", countryModel.IsRecordDeleted, DbType.Int32);
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

        public async Task<int> DeleteAsync(CountryModel countryModel)
        {
            try
            {
                
                var query = "SP_InsertUpdateDelete_Country";
                using (var connection = CreateConnection())
                {
                    countryModel.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("UserId", countryModel.CreatedBy, DbType.Int32);
                    parameters.Add("IPAddress", countryModel.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", countryModel.IsRecordDeleted, DbType.Int32);
                    parameters.Add("CountryId", countryModel.CountryId, DbType.Int32);
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

        public async Task<List<CountryModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Country";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CountryModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<CountryModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CountryModel> GetByIdAsync(int CountryId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Country";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", CountryId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst =  await SqlMapper.QueryAsync<CountryModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CountryModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Country";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("CountryName", entity.CountryName, DbType.String);
                    parameters.Add("Description", entity.Description, DbType.String);
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
    }
}
