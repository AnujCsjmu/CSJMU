using CoreLayout.Models.Circular;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Circular
{
    public class CircularRepository : BaseRepository, ICircularRepository
    {
        public CircularRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(CircularModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                // create the transaction
                // You could use `var` instead of `SqlTransaction`
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        entity.IsRecordDeleted = 0;
                        entity.Flag = "A";
                        var query = "SP_InsertUpdateDelete_Circular";
                        var res = 0;
                        var res1 = 0;
                        int newID = 0;

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Title", entity.Title, DbType.String);
                        parameters.Add("DisplayInWebSite", entity.DisplayInWebSite, DbType.Int32);
                        parameters.Add("DisplayInCollage", entity.DisplayInCollage, DbType.Int32);
                        parameters.Add("OriginalFileName", entity.OriginalFileName, DbType.String);
                        parameters.Add("FileName", entity.FileName, DbType.String);
                        parameters.Add("Related", entity.Related, DbType.String);
                        parameters.Add("CircularPath", entity.CircularPath, DbType.String);
                        parameters.Add("UploadDate", entity.UploadDate, DbType.Date);
                        parameters.Add("Flag", entity.Flag, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("ReturnCircularId", entity.ReturnCircularId, DbType.Int32, direction: ParameterDirection.Output);

                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        newID = parameters.Get<int>("ReturnCircularId");

                        if (res == 1 && newID != 0)
                        {
                            foreach(int instituteid in entity.InstituteList)
                            {
                                parameters.Add("CircularId", newID, DbType.Int32);
                                parameters.Add("InstituteID", instituteid, DbType.Int32);
                                parameters.Add("@Query", 6, DbType.Int32);
                                res1 = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                            }
                            
                        }
                        if (res == 1 && res1 == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return res1;
                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        tran.Rollback();

                        // handle the error however you need to.
                        throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<int> DeleteAsync(CircularModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CircularId", entity.CircularId, DbType.Int32);
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

        public async Task<List<CircularModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CircularModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CircularModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CircularModel> GetByIdAsync(int CircularId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CircularId", CircularId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<CircularModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(CircularModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 0;
                    entity.Flag = "A";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CircularId", entity.CircularId, DbType.Int32);
                    parameters.Add("Title", entity.Title, DbType.String);
                    parameters.Add("DisplayInWebSite", entity.DisplayInWebSite, DbType.Int32);
                    parameters.Add("DisplayInCollage", entity.DisplayInCollage, DbType.Int32);
                    //parameters.Add("OriginalFileName", entity.OriginalFileName, DbType.String);
                    //parameters.Add("FileName", entity.FileName, DbType.String);
                    parameters.Add("Related", entity.Related, DbType.String);
                    parameters.Add("UploadDate", entity.UploadDate, DbType.Date);
                    parameters.Add("Flag", entity.Flag, DbType.String);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
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

        public async Task<List<CircularModel>> GetAllCircularByCollageId(int instituteid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 7, DbType.Int32);
                    parameters.Add("@InstituteID", instituteid, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CircularModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CircularModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<CircularModel>> GetAllInstituteByCircular(int circularid)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Circular";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 8, DbType.Int32);
                    parameters.Add("@CircularId", circularid, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<CircularModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<CircularModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
