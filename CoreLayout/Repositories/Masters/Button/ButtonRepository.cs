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
    public class ButtonRepository : BaseRepository, IButtonRepository
    {
        public ButtonRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(ButtonModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonName", entity.ButtonName, DbType.String);
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

        public async Task<int> DeleteAsync(ButtonModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CountryId", entity.ButtonId, DbType.Int32);
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

        public async Task<List<ButtonModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<ButtonModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ButtonModel>> GetAllButtonMapping(int buttonid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonId", buttonid, DbType.Int32);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<ButtonModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<ButtonModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ButtonModel> GetByIdAsync(int buttonid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonId", buttonid, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst =  await SqlMapper.QueryAsync<ButtonModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(ButtonModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Button";
                using (var connection = CreateConnection())
                {
                    entity.IPAddress = ":11";
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ButtonId", entity.ButtonId, DbType.Int32);
                    parameters.Add("ButtonName", entity.ButtonName, DbType.String);
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
