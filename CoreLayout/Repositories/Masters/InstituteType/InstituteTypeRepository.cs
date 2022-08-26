using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.InstituteType
{
    public class InstituteTypeRepository : BaseRepository, IInstituteTypeRepository
    {
        public InstituteTypeRepository(IConfiguration configuration)
: base(configuration)
        { }

        public Task<int> CreateAsync(InstituteTypeModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(InstituteTypeModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InstituteTypeModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_InstituteType";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<InstituteTypeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<InstituteTypeModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Task<InstituteTypeModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(InstituteTypeModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
