using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Branch
{
    public class BranchRepository : BaseRepository, IBranchRepository
    {

        public BranchRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(BranchModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                var query = "SP_InsertUpdateDelete_Branch";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("BranchCode", entity.BranchCode, DbType.String);
                    parameters.Add("BranchName", entity.BranchName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("CerificateName", entity.CerificateName, DbType.String);
                    parameters.Add("DisplayName", entity.DisplayName, DbType.String);
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

        public async Task<int> DeleteAsync(BranchModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Branch";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                   DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("BranchID", entity.BranchID, DbType.Int32);
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

        public async Task<List<BranchModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Branch";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<BranchModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<BranchModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<BranchModel> GetByIdAsync(int BranchID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Branch";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("BranchID", BranchID, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<BranchModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(BranchModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Branch";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("BranchID", entity.BranchID, DbType.Int32);
                    parameters.Add("BranchCode", entity.BranchCode, DbType.String);
                    parameters.Add("BranchName", entity.BranchName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("CerificateName", entity.CerificateName, DbType.String);
                    parameters.Add("DisplayName", entity.DisplayName, DbType.String);
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
    }
}
