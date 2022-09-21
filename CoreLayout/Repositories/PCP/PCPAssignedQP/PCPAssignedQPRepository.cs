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
namespace CoreLayout.Repositories.PCP.PCPAssignedQP
{
    public class PCPAssignedQPRepository : BaseRepository, IPCPAssignedQPRepository
    {
        public PCPAssignedQPRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(PCPAssignedQPModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPAssignedQP";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("QPId", entity.QPId, DbType.String);
                        //parameters.Add("PCPRegID", entity.PCPRegID, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        //parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
                        parameters.Add("@Query", 1, DbType.Int32);
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (int userid in entity.UserList)
                        {
                            parameters.Add("UserId", userid, DbType.Int32);
                            res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        }


                        //res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);

                        if (res == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return res;
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

        public async Task<int> DeleteAsync(PCPAssignedQPModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPAssignedQP";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AssignedQPId", entity.AssignedQPId, DbType.Int32);
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

        public async Task<List<PCPAssignedQPModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPAssignedQP";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<PCPAssignedQPModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<PCPAssignedQPModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PCPAssignedQPModel> GetByIdAsync(int AssignedQPId)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_PCPAssignedQP";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AssignedQPId", AssignedQPId, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<PCPAssignedQPModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(PCPAssignedQPModel entity)
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
                        var query = "SP_InsertUpdateDelete_PCPAssignedQP";
                        var res = 0;
                        entity.IsRecordDeleted = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("AssignedQPId", entity.AssignedQPId, DbType.String);
                        parameters.Add("QPId", entity.QPId, DbType.String);
                        //parameters.Add("PCPRegID", entity.PCPRegID, DbType.String);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.String);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        //parameters.Add("UserId", entity.UserId, DbType.Int32);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.String);
                        parameters.Add("@Query", 2, DbType.Int32);
                        foreach (int userid in entity.UserList)
                        {
                            parameters.Add("UserId", userid, DbType.Int32);
                            res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        }
                        if (res == 1)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }

                        return res;

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

        //public async Task<List<PCPAssignedQPModel>> GetAllUserByQPIdAsync(int qpid)
        //{
        //    try
        //    {
        //        var query = "SP_InsertUpdateDelete_PCPAssignedQP";
        //        using (var connection = CreateConnection())
        //        {
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("QPId", qpid, DbType.String);
        //            parameters.Add("@Query", 6, DbType.Int32);
        //            var list = await SqlMapper.QueryAsync<PCPAssignedQPModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            return (List<PCPAssignedQPModel>)list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        //public async Task<List<PCPAssignedQPModel>> GetAllQPByUserIdAsync(int Userid)
        //{
        //    try
        //    {
        //        var query = "SP_InsertUpdateDelete_PCPAssignedQP";
        //        using (var connection = CreateConnection())
        //        {
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("Userid", Userid, DbType.String);
        //            parameters.Add("@Query", 7, DbType.Int32);
        //            var list = await SqlMapper.QueryAsync<PCPAssignedQPModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
        //            return (List<PCPAssignedQPModel>)list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}
    }
}
