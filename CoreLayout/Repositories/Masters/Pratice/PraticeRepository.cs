using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Pratice
{
    public class PraticeRepository : BaseRepository, IPraticeRepository
    {
        public PraticeRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(PraticeModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_Pratice";
                //var query1 = "select max(id)+1 id from pratice";
                using (var connection = CreateConnection())
                {
                    //var Id = connection.ExecuteAsync(query1);
                    var Id = connection.ExecuteScalar("SELECT Max(Id)+1 AS ID FROM Pratice");
                    if(Id==null)
                    {
                        Id = 1;
                    }

                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (int deptid in entity.departmentid)
                    {
                        stringBuilder.Append("insert into Pratice_Department(PraticeId,DepartmentId, CreatedDate, CreatedBy) values('"+ Id + "','" + deptid + "', getDate(), '" + entity.CreatedBy + "')");
                        stringBuilder.Append("; ");

                    }
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", Id, DbType.Int32);
                    parameters.Add("Name", entity.Name, DbType.String);
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("CityId", entity.CityId, DbType.Int32);
                    parameters.Add("RoleId", entity.roleid, DbType.Int32);
                    parameters.Add("Gender", entity.gender, DbType.String);
                    parameters.Add("UploadFileName", entity.UploadFileName, DbType.String);
                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("DeptQuery", stringBuilder.ToString(), DbType.String);
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
        public async Task<int> DeleteAsync(PraticeModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_Pratice";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.Int32);
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

        public async Task<List<PraticeModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_Pratice";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PraticeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<PraticeModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<PraticeChildModel>> GetAllChildAsync()
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_PraticeChild";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PraticeChildModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<PraticeChildModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<PraticeModel> GetByIdAsync(int Id)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_Pratice";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", Id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PraticeModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PraticeModel entity)
        {
            try
            {
                var query = "Usp_InsertUpdateDelete_Pratice";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.String);
                    parameters.Add("Name", entity.Name, DbType.String);
                    parameters.Add("CountryId", entity.CountryId, DbType.Int32);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("CityId", entity.CityId, DbType.Int32);
                    parameters.Add("RoleId", entity.roleid, DbType.Int32);
                    parameters.Add("Gender", entity.gender, DbType.String);
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

