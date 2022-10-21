using CoreLayout.Models;
using CoreLayout.Models.Masters;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Institute
{
    public class InstituteRepository : BaseRepository, IInstituteRepository
    {
        public InstituteRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(InstituteModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteCode", entity.InstituteCode, DbType.String);
                    parameters.Add("InstituteName", entity.InstituteName, DbType.String);
                    parameters.Add("UniversityID", entity.UniversityID, DbType.Int32);
                    parameters.Add("InstituteTypeId", entity.InstituteTypeId, DbType.Int32);
                    parameters.Add("InstituteCategoryId", entity.InstituteCategoryId, DbType.Int32);
                    parameters.Add("IsMinority", entity.IsMinority, DbType.String);
                    parameters.Add("Address", entity.Address, DbType.String);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("DistrictID", entity.DistrictID, DbType.Int32);
                    parameters.Add("TehsilId", entity.TehsilId, DbType.Int32);
                    parameters.Add("Pincode", entity.Pincode, DbType.String);
                    parameters.Add("EmailId", entity.EmailId, DbType.String);
                    parameters.Add("MobileNumber", entity.MobileNumber, DbType.String);
                    parameters.Add("Phone", entity.Phone, DbType.String);
                    parameters.Add("Fax", entity.Fax, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                    parameters.Add("IsAffiliated", entity.IsAffiliated, DbType.Int32);
                    parameters.Add("GroupId", entity.GroupId, DbType.String);
                    parameters.Add("WebURL", entity.WebURL, DbType.String);
                    parameters.Add("BankId", entity.BankId, DbType.String);
                    parameters.Add("AccountNo", entity.AccountNo, DbType.String);
                    parameters.Add("IFSCCode", entity.IFSCCode, DbType.String);
                    parameters.Add("EstablishmentYear", entity.EstablishmentYear, DbType.String);
                    parameters.Add("Latitude", entity.Latitude, DbType.String);
                    parameters.Add("Longitude", entity.Longitude, DbType.String);
                    parameters.Add("ChairmanAdhaar", entity.ChairmanAdhaar, DbType.String);
                    parameters.Add("ChairmanEmail", entity.ChairmanEmail, DbType.String);
                    parameters.Add("ChairmanMobileNumber", entity.ChairmanMobileNumber, DbType.String);
                    parameters.Add("ChairmanName", entity.ChairmanName, DbType.String);
                    parameters.Add("RegistrarAdhaar", entity.RegistrarAdhaar, DbType.String);
                    parameters.Add("RegistrarEmail", entity.RegistrarEmail, DbType.String);
                    parameters.Add("RegistrarMobileNumber", entity.RegistrarMobileNumber, DbType.String);
                    parameters.Add("RegistrarName", entity.RegistrarName, DbType.String);
                    parameters.Add("DirectorAdhaar", entity.DirectorAdhaar, DbType.String);
                    parameters.Add("DirectorEmail", entity.DirectorEmail, DbType.String);
                    parameters.Add("DirectorMobileNumber", entity.DirectorMobileNumber, DbType.String);
                    parameters.Add("DirectorName", entity.DirectorName, DbType.String);
                    parameters.Add("StaticIP", entity.StaticIP, DbType.String);
                    parameters.Add("Vision", entity.Vision, DbType.String);
                    parameters.Add("Mission", entity.Mission, DbType.String);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("UserId", entity.CreatedBy, DbType.Int32);
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

        public async Task<int> DeleteAsync(InstituteModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteID", entity.InstituteID, DbType.Int32);
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

        public async Task<List<InstituteModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";       
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<InstituteModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<InstituteModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<InstituteModel> GetByIdAsync(int InstituteID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteID", InstituteID, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<InstituteModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(InstituteModel entity)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("InstituteID", entity.InstituteID, DbType.Int32);
                    parameters.Add("InstituteCode", entity.InstituteCode, DbType.String);
                    parameters.Add("InstituteName", entity.InstituteName, DbType.String);
                    parameters.Add("UniversityID", entity.UniversityID, DbType.Int32);
                    parameters.Add("InstituteTypeId", entity.InstituteTypeId, DbType.Int32);
                    parameters.Add("InstituteCategoryId", entity.InstituteCategoryId, DbType.Int32);
                    parameters.Add("IsMinority", entity.IsMinority, DbType.String);
                    parameters.Add("Address", entity.Address, DbType.String);
                    parameters.Add("StateId", entity.StateId, DbType.Int32);
                    parameters.Add("DistrictID", entity.DistrictID, DbType.Int32);
                    parameters.Add("TehsilId", entity.TehsilId, DbType.Int32);
                    parameters.Add("Pincode", entity.Pincode, DbType.String);
                    parameters.Add("EmailId", entity.EmailId, DbType.String);
                    parameters.Add("MobileNumber", entity.MobileNumber, DbType.String);
                    parameters.Add("Phone", entity.Phone, DbType.String);
                    parameters.Add("Fax", entity.Fax, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Int32);
                    parameters.Add("IsAffiliated", entity.IsAffiliated, DbType.Int32);
                    parameters.Add("GroupId", entity.GroupId, DbType.String);
                    parameters.Add("WebURL", entity.WebURL, DbType.String);
                    parameters.Add("BankId", entity.BankId, DbType.String);
                    parameters.Add("AccountNo", entity.AccountNo, DbType.String);
                    parameters.Add("IFSCCode", entity.IFSCCode, DbType.String);
                    parameters.Add("EstablishmentYear", entity.EstablishmentYear, DbType.String);
                    parameters.Add("Latitude", entity.Latitude, DbType.String);
                    parameters.Add("Longitude", entity.Longitude, DbType.String);
                    parameters.Add("ChairmanAdhaar", entity.ChairmanAdhaar, DbType.String);
                    parameters.Add("ChairmanEmail", entity.ChairmanEmail, DbType.String);
                    parameters.Add("ChairmanMobileNumber", entity.ChairmanMobileNumber, DbType.String);
                    parameters.Add("ChairmanName", entity.ChairmanName, DbType.String);
                    parameters.Add("RegistrarAdhaar", entity.RegistrarAdhaar, DbType.String);
                    parameters.Add("RegistrarEmail", entity.RegistrarEmail, DbType.String);
                    parameters.Add("RegistrarMobileNumber", entity.RegistrarMobileNumber, DbType.String);
                    parameters.Add("RegistrarName", entity.RegistrarName, DbType.String);
                    parameters.Add("DirectorAdhaar", entity.DirectorAdhaar, DbType.String);
                    parameters.Add("DirectorEmail", entity.DirectorEmail, DbType.String);
                    parameters.Add("DirectorMobileNumber", entity.DirectorMobileNumber, DbType.String);
                    parameters.Add("DirectorName", entity.DirectorName, DbType.String);
                    parameters.Add("StaticIP", entity.StaticIP, DbType.String);
                    parameters.Add("Vision", entity.Vision, DbType.String);
                    parameters.Add("Mission", entity.Mission, DbType.String);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("UserId", entity.ModifiedBy, DbType.Int32);
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

        public async Task<List<InstituteModel>> AffiliationInstituteIntakeData()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Institute";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 6, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<InstituteModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return (List<InstituteModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
