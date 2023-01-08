using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.WRN.WRNRegistration
{
    public class WRNRegistrationRepository : BaseRepository, IWRNRegistrationRepository
    {
        public WRNRegistrationRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(WRNRegistrationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var res= 0;
                        var res1 = 0;
                        var query = string.Empty;
                        var query1 = string.Empty;
                        entity.IsActive = true;
                        entity.IsRecordDeleted = 0;
                         query = "Usp_WRNRegistration";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("ModeOfAdmission", entity.ModeOfAdmission, DbType.String);
                        parameters.Add("ApplicationNo", entity.ApplicationNo, DbType.String);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("FirstName", entity.FirstName, DbType.String);
                        parameters.Add("MiddleName", entity.MiddleName, DbType.String);
                        parameters.Add("LastName", entity.LastName, DbType.String);
                        parameters.Add("HindiName", entity.HindiName, DbType.String);
                        parameters.Add("FatherName", entity.FatherName, DbType.String);
                        parameters.Add("MotherName", entity.MotherName, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("AadharNumber", entity.AadharNumber, DbType.String);
                        parameters.Add("Gender", entity.Gender, DbType.String);
                        parameters.Add("DOB", entity.DOB, DbType.String);
                        parameters.Add("CategoryId", entity.CategoryId, DbType.Int32);
                        parameters.Add("Nationality", entity.Nationality, DbType.String);
                        parameters.Add("ReligionId", entity.ReligionId, DbType.Int32);
                        parameters.Add("PhysicalDisabled", entity.PhysicalDisabled, DbType.String);
                        parameters.Add("PermanentAddress", entity.PermanentAddress, DbType.String);
                        parameters.Add("PermanentStateId", entity.PermanentStateId, DbType.Int32);
                        parameters.Add("PermanentDistrictId", entity.PermanentDistrictId, DbType.Int32);
                        parameters.Add("PermanentPincode", entity.PermanentPincode, DbType.String);
                        parameters.Add("CommunicationAddress", entity.CommunicationAddress, DbType.String);
                        parameters.Add("CommunicationStateId", entity.CommunicationStateId, DbType.Int32);
                        parameters.Add("CommunicationDistrictId", entity.CommunicationDistrictId, DbType.Int32);
                        parameters.Add("CommunicationPincode", entity.CommunicationPincode, DbType.String);
                        parameters.Add("TermsConditions", entity.TermsConditions, DbType.Boolean);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                        parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("@Query", 1, DbType.Int32);
                        res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
                        if (res == 1)
                        {
                            DynamicParameters parameters1 = new DynamicParameters();
                            query1 = "Usp_SequenceGenerate";
                            parameters1.Add("Id", 8, DbType.Int32);
                            parameters1.Add("@Query", 2, DbType.Int32);
                            parameters1.Add("@UserId", entity.CreatedBy, DbType.Int32);
                            res1 = await SqlMapper.ExecuteAsync(connection, query1, parameters1, tran, commandType: CommandType.StoredProcedure);
                        }


                        if (res1 == 1)
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

        public async Task<int> DeleteAsync(WRNRegistrationModel entity)
        {
            try
            {
                var query = "Usp_WRNRegistration";
                using (var connection = CreateConnection())
                {
                    entity.IsRecordDeleted = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", entity.Id, DbType.Int32);
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

        public async Task<List<WRNRegistrationModel>> GetAllAsync()
        {
            try
            {
                var query = "Usp_WRNRegistration";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<WRNRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);

                    return (List<WRNRegistrationModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<WRNRegistrationModel> GetByIdAsync(int id)
        {
            try
            {
                var query = "Usp_WRNRegistration";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<WRNRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<WRNRegistrationModel> GetWRNRegistrationByLoginAsync(string RegistrationNo, string MobileNo, string DOB)
        {
            try
            {
                var query = "Usp_WRNRegistration";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RegistrationNo", RegistrationNo, DbType.String);
                    parameters.Add("MobileNo", MobileNo, DbType.String);
                    parameters.Add("DOB", DOB, DbType.String);
                    parameters.Add("@Query", 6, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<WRNRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<WRNRegistrationModel> GetWRNRegistrationByMobileAsync(string MobileNo)
        {
            try
            {
                var query = "Usp_WRNRegistration";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("MobileNo", MobileNo, DbType.String);
                    parameters.Add("@Query", 7, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<WRNRegistrationModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateAsync(WRNRegistrationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        entity.IsActive = true;
                        entity.IsRecordDeleted = 0;
                        var query = "Usp_WRNRegistration";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        parameters.Add("ModeOfAdmission", entity.ModeOfAdmission, DbType.String);
                        parameters.Add("ApplicationNo", entity.ApplicationNo, DbType.String);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("FirstName", entity.FirstName, DbType.String);
                        parameters.Add("MiddleName", entity.MiddleName, DbType.String);
                        parameters.Add("LastName", entity.LastName, DbType.String);
                        parameters.Add("HindiName", entity.HindiName, DbType.String);
                        parameters.Add("FatherName", entity.FatherName, DbType.String);
                        parameters.Add("MotherName", entity.MotherName, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("AadharNumber", entity.AadharNumber, DbType.String);
                        parameters.Add("Gender", entity.Gender, DbType.String);
                        parameters.Add("DOB", entity.DOB, DbType.String);
                        parameters.Add("CategoryId", entity.CategoryId, DbType.Int32);
                        parameters.Add("Nationality", entity.Nationality, DbType.String);
                        parameters.Add("ReligionId", entity.ReligionId, DbType.Int32);
                        parameters.Add("PhysicalDisabled", entity.PhysicalDisabled, DbType.String);
                        parameters.Add("PermanentAddress", entity.PermanentAddress, DbType.String);
                        parameters.Add("PermanentStateId", entity.PermanentStateId, DbType.Int32);
                        parameters.Add("PermanentDistrictId", entity.PermanentDistrictId, DbType.Int32);
                        parameters.Add("PermanentPincode", entity.PermanentPincode, DbType.String);
                        parameters.Add("CommunicationAddress", entity.CommunicationAddress, DbType.String);
                        parameters.Add("CommunicationStateId", entity.CommunicationStateId, DbType.Int32);
                        parameters.Add("CommunicationDistrictId", entity.CommunicationDistrictId, DbType.Int32);
                        parameters.Add("CommunicationPincode", entity.CommunicationPincode, DbType.String);
                        parameters.Add("TermsConditions", entity.TermsConditions, DbType.Boolean);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                        parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("AcademicSession", entity.AcademicSession, DbType.String);
                        parameters.Add("FinalSubmit", entity.FinalSubmit, DbType.Int32);
                        parameters.Add("@Query", 2, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateFinalSubmitAsync(WRNRegistrationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        entity.IsActive = true;
                        entity.IsRecordDeleted = 0;
                        var query = "Usp_WRNRegistration";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Id", entity.Id, DbType.Int32);
                        parameters.Add("ModeOfAdmission", entity.ModeOfAdmission, DbType.String);
                        parameters.Add("ApplicationNo", entity.ApplicationNo, DbType.String);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("FirstName", entity.FirstName, DbType.String);
                        parameters.Add("MiddleName", entity.MiddleName, DbType.String);
                        parameters.Add("LastName", entity.LastName, DbType.String);
                        parameters.Add("HindiName", entity.HindiName, DbType.String);
                        parameters.Add("FatherName", entity.FatherName, DbType.String);
                        parameters.Add("MotherName", entity.MotherName, DbType.String);
                        parameters.Add("MobileNo", entity.MobileNo, DbType.String);
                        parameters.Add("EmailId", entity.EmailId, DbType.String);
                        parameters.Add("AadharNumber", entity.AadharNumber, DbType.String);
                        parameters.Add("Gender", entity.Gender, DbType.String);
                        parameters.Add("DOB", entity.DOB, DbType.String);
                        parameters.Add("CategoryId", entity.CategoryId, DbType.Int32);
                        parameters.Add("Nationality", entity.Nationality, DbType.String);
                        parameters.Add("ReligionId", entity.ReligionId, DbType.Int32);
                        parameters.Add("PhysicalDisabled", entity.PhysicalDisabled, DbType.String);
                        parameters.Add("PermanentAddress", entity.PermanentAddress, DbType.String);
                        parameters.Add("PermanentStateId", entity.PermanentStateId, DbType.Int32);
                        parameters.Add("PermanentDistrictId", entity.PermanentDistrictId, DbType.Int32);
                        parameters.Add("PermanentPincode", entity.PermanentPincode, DbType.String);
                        parameters.Add("CommunicationAddress", entity.CommunicationAddress, DbType.String);
                        parameters.Add("CommunicationStateId", entity.CommunicationStateId, DbType.Int32);
                        parameters.Add("CommunicationDistrictId", entity.CommunicationDistrictId, DbType.Int32);
                        parameters.Add("CommunicationPincode", entity.CommunicationPincode, DbType.String);
                        parameters.Add("TermsConditions", entity.TermsConditions, DbType.Boolean);
                        parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                        parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
                        parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                        parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                        parameters.Add("AcademicSession", entity.AcademicSession, DbType.String);
                        parameters.Add("FinalSubmit", entity.FinalSubmit, DbType.Int32);
                        parameters.Add("@Query", 8, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdatePhotoSignatureAsync(WRNRegistrationModel entity)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        entity.IsActive = true;
                        entity.IsRecordDeleted = 0;
                        var query = "Usp_WRNRegistration";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("PhotoPath", entity.PhotoPath, DbType.String);
                        parameters.Add("SignaturePath", entity.SignaturePath, DbType.String);
                        parameters.Add("RegistrationNo", entity.RegistrationNo, DbType.String);
                        parameters.Add("@Query", 9, DbType.Int32);
                        var res = await SqlMapper.ExecuteAsync(connection, query, parameters, tran, commandType: CommandType.StoredProcedure);
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
    }
}
