using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.Student
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        public StudentRepository(IConfiguration configuration)
: base(configuration)
        { }
        public async Task<int> CreateAsync(StudentModel entity)
        {
            try
            {
                int res = 0;
                entity.IsRecordDeleted = 0;
                entity.IsActive = true;
                entity.DataSource = "New";
                var query = "SP_InsertUpdateDelete_Student";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RollNo", entity.RollNo, DbType.String);
                    parameters.Add("EnrolmentNo", entity.EnrolmentNo, DbType.String);
                    parameters.Add("EntranceApplicationNo", entity.EntranceApplicationNo, DbType.String);
                    parameters.Add("FirstName", entity.FirstName, DbType.String);
                    parameters.Add("MiddleName", entity.MiddleName, DbType.String);
                    parameters.Add("LastName", entity.LastName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("FatherName", entity.FatherName, DbType.String);
                    parameters.Add("MotherName", entity.MotherName, DbType.String);
                    parameters.Add("DOB", entity.DOB, DbType.Date);
                    parameters.Add("Gender", entity.Gender, DbType.String);
                    parameters.Add("Mobile", entity.Mobile, DbType.String);
                    parameters.Add("ParentMobileNo", entity.ParentMobileNo, DbType.String);
                    parameters.Add("EmailId", entity.EmailId, DbType.String);
                    parameters.Add("CAddress", entity.CAddress, DbType.String);
                    parameters.Add("CStateId", entity.CStateId, DbType.Int32);
                    parameters.Add("CDistrictId", entity.CDistrictId, DbType.Int32);
                    parameters.Add("CTehsilId", entity.CTehsilId, DbType.Int32);
                    parameters.Add("CPinCode", entity.CPinCode, DbType.String);
                    parameters.Add("PAddress", entity.PAddress, DbType.String);
                    parameters.Add("PStateId", entity.PStateId, DbType.Int32);
                    parameters.Add("PDistrictId", entity.PDistrictId, DbType.Int32);
                    parameters.Add("PTehsilId", entity.PTehsilId, DbType.Int32);
                    parameters.Add("PPinCode", entity.PPinCode, DbType.String);
                    parameters.Add("IdProofType", entity.IdProofType, DbType.String);
                    parameters.Add("IdProofNo", entity.IdProofNo, DbType.String);
                    parameters.Add("Category", entity.Category, DbType.String);
                    parameters.Add("IsEWS", entity.IsEWS, DbType.Boolean);
                    parameters.Add("IsDisability", entity.IsDisability, DbType.Boolean);
                    parameters.Add("DisabilityType", entity.DisabilityType, DbType.String);
                    parameters.Add("Nationality", entity.Nationality, DbType.String);
                    parameters.Add("BloodGroup", entity.BloodGroup, DbType.String);
                    parameters.Add("IsVaccinated", entity.IsVaccinated, DbType.Boolean);
                    parameters.Add("ReligionId", entity.ReligionID, DbType.Int32);
                    parameters.Add("IsMinority", entity.IsMinority, DbType.Boolean);
                    parameters.Add("PhotographPath", entity.PhotographPath, DbType.String);
                    parameters.Add("SignaturePath", entity.SignaturePath, DbType.String);
                    parameters.Add("SeatType", entity.SeatType, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Boolean);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);

                    parameters.Add("IsLiteral", entity.IsLiteral, DbType.Boolean);
                    parameters.Add("ReservationCategory", entity.ReservationCategory, DbType.String);
                    parameters.Add("AdmissionSessionId", entity.AdmissionSessionId, DbType.Int32);
                    parameters.Add("DataSource", entity.DataSource, DbType.String);

                    parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32);
                    parameters.Add("@Query", 1, DbType.Int32);
                    res = await SqlMapper.ExecuteAsync(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return res;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(StudentModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 1;
                var query = "SP_InsertUpdateDelete_Student";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentID", entity.StudentID, DbType.Int32);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);
                    parameters.Add("ModifiedBy", entity.ModifiedBy, DbType.Int32);
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

        public async Task<List<StudentModel>> GetAllAsync()
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Student";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Query", 4, DbType.Int32);
                    var list = await SqlMapper.QueryAsync<StudentModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                   
                    return (List<StudentModel>)list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<StudentModel> GetByIdAsync(int StudentID)
        {
            try
            {
                var query = "SP_InsertUpdateDelete_Student";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentID", StudentID, DbType.Int32);
                    parameters.Add("@Query", 5, DbType.Int32);
                    var lst = await SqlMapper.QueryAsync<StudentModel>(connection, query, parameters, commandType: CommandType.StoredProcedure);
                    return lst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(StudentModel entity)
        {
            try
            {
                entity.IsRecordDeleted = 0;
                entity.IsActive = true;
                entity.DataSource = "New";
                var query = "SP_InsertUpdateDelete_Student";
                using (var connection = CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("StudentID", entity.StudentID, DbType.Int32);
                    parameters.Add("RollNo", entity.RollNo, DbType.String);
                    parameters.Add("EnrolmentNo", entity.EnrolmentNo, DbType.String);
                    parameters.Add("EntranceApplicationNo", entity.EntranceApplicationNo, DbType.String);
                    parameters.Add("FirstName", entity.FirstName, DbType.String);
                    parameters.Add("MiddleName", entity.MiddleName, DbType.String);
                    parameters.Add("LastName", entity.LastName, DbType.String);
                    parameters.Add("HindiName", entity.HindiName, DbType.String);
                    parameters.Add("FatherName", entity.FatherName, DbType.String);
                    parameters.Add("MotherName", entity.MotherName, DbType.String);
                    parameters.Add("DOB", entity.DOB, DbType.Date);
                    parameters.Add("Gender", entity.Gender, DbType.String);
                    parameters.Add("Mobile", entity.Mobile, DbType.String);
                    parameters.Add("ParentMobileNo", entity.ParentMobileNo, DbType.String);
                    parameters.Add("EmailId", entity.EmailId, DbType.String);
                    parameters.Add("CAddress", entity.CAddress, DbType.String);
                    parameters.Add("CStateId", entity.CStateId, DbType.Int32);
                    parameters.Add("CDistrictId", entity.CDistrictId, DbType.Int32);
                    parameters.Add("CTehsilId", entity.CTehsilId, DbType.Int32);
                    parameters.Add("CPinCode", entity.CPinCode, DbType.String);
                    parameters.Add("PAddress", entity.PAddress, DbType.String);
                    parameters.Add("PStateId", entity.PStateId, DbType.Int32);
                    parameters.Add("PDistrictId", entity.PDistrictId, DbType.Int32);
                    parameters.Add("PTehsilId", entity.PTehsilId, DbType.Int32);
                    parameters.Add("PPinCode", entity.PPinCode, DbType.String);
                    parameters.Add("IdProofType", entity.IdProofType, DbType.String);
                    parameters.Add("IdProofNo", entity.IdProofNo, DbType.String);
                    parameters.Add("Category", entity.Category, DbType.String);
                    parameters.Add("IsEWS", entity.IsEWS, DbType.Boolean);
                    parameters.Add("IsDisability", entity.IsDisability, DbType.Boolean);
                    parameters.Add("DisabilityType", entity.DisabilityType, DbType.String);
                    parameters.Add("Nationality", entity.Nationality, DbType.String);
                    parameters.Add("BloodGroup", entity.BloodGroup, DbType.String);
                    parameters.Add("IsVaccinated", entity.IsVaccinated, DbType.Boolean);
                    parameters.Add("ReligionId", entity.ReligionID, DbType.Int32);
                    parameters.Add("IsMinority", entity.IsMinority, DbType.Boolean);
                    parameters.Add("PhotographPath", entity.PhotographPath, DbType.String);
                    parameters.Add("SignaturePath", entity.SignaturePath, DbType.String);
                    parameters.Add("SeatType", entity.SeatType, DbType.String);
                    parameters.Add("IsActive", entity.IsActive, DbType.Boolean);
                    parameters.Add("IsRecordDeleted", entity.IsRecordDeleted, DbType.Int32);
                    parameters.Add("IPAddress", entity.IPAddress, DbType.String);

                    parameters.Add("IsLiteral", entity.IsLiteral, DbType.Boolean);
                    parameters.Add("ReservationCategory", entity.ReservationCategory, DbType.String);
                    parameters.Add("AdmissionSessionId", entity.AdmissionSessionId, DbType.Int32);
                    parameters.Add("DataSource", entity.DataSource, DbType.String);

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
