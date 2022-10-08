using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoreLayout.Repositories.PCP.PCPDetailsReport
{
    public class PCPDetailsReportRepository : BaseRepository, IPCPDetailsReportRepository
    {
        public PCPDetailsReportRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<List<PCPDetailsReportModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_PCPDetailsReport";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPDetailsReportModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPDetailsReportModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
