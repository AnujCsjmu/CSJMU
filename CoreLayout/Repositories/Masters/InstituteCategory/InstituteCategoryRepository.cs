using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.InstituteCategory
{
    public class InstituteCategoryRepository : BaseRepository, IInstituteCategoryRepository
    {
        public InstituteCategoryRepository(IConfiguration configuration)
: base(configuration)
        { }

        public Task<int> CreateAsync(InstituteCategoryModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(InstituteCategoryModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InstituteCategoryModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_InstituteCategory";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<InstituteCategoryModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<InstituteCategoryModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Task<InstituteCategoryModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(InstituteCategoryModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
