using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Exam;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Exam.Student;
using CoreLayout.Services.Exam.StudentAcademicQPDetails;
using CoreLayout.Services.Exam.StudentAcademics;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.ExamCategory;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CoreLayout.Controllers.Exam
{
    [Authorize(Roles = "Administrator")]
    public class StudentAcademicsApprovalController : Controller
    {
        private readonly ILogger<StudentAcademicsApprovalController> _logger;
        private readonly IDataProtector _protector;
        private readonly IExamMasterService _examMasterService;
        private readonly IExamCourseMappingService _examCourseMappingService;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IStudentService _studentService;
        private readonly IStudentAcademicsService _studentAcademicsService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        public readonly IConfiguration _configuration;
        private readonly IInstituteService _instituteService;
        private readonly IExamCategoryService _examCategoryService;
        private readonly IQPMasterService _qPMasterService;
        private readonly IStudentAcademicQPDetailsService _studentAcademicQPDetailsService;

        public StudentAcademicsApprovalController(ILogger<StudentAcademicsApprovalController> logger, IDataProtectionProvider provider,
            IExamMasterService examMasterService, ICourseDetailsService courseDetailsService, IStudentAcademicsService studentAcademicsService,
            IStudentService studentService, IConfiguration configuration, ICourseService courseService,
            IBranchService branchService, ICourseBranchMappingService courseBranchMappingService,
            IInstituteService instituteService, IExamCourseMappingService examCourseMappingService, IExamCategoryService examCategoryService,
            IQPMasterService qPMasterService, IStudentAcademicQPDetailsService studentAcademicQPDetailsService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("StudentAcademicsApproval.StudentAcademicsApprovalController");
            _courseDetailsService = courseDetailsService;
            _studentService = studentService;
            _studentAcademicsService = studentAcademicsService;
            _courseService = courseService;
            _branchService = branchService;
            _configuration = configuration;
            _courseBranchMappingService = courseBranchMappingService;
            _instituteService = instituteService;
            _examCourseMappingService = examCourseMappingService;
            _examCategoryService = examCategoryService;
            _qPMasterService = qPMasterService;
            _studentAcademicQPDetailsService = studentAcademicQPDetailsService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _studentAcademicsService.GetAllStudentAcademicsAsync();
                foreach (var _data in data)
                {
                    var stringId = _data.AcademicId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end



                return View("~/Views/Exam/StudentAcademicsApproval/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/StudentAcademicsApproval/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(int? hdnInstituteID, int? hdnCourseId, int? hdnSubjectId, int? hdnSemYearId)
        {
            //var data = (dynamic)null;
            //object data = null;

            var data = await _studentAcademicsService.GetFilterStudentAcademicsData(hdnInstituteID, hdnCourseId, hdnSubjectId, hdnSemYearId);
            if (data != null)
            {
                foreach (var _data in data)
                {
                    var stringId = _data.AcademicId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.AcademicId;
                }
                id = id + 1;
                ViewBag.MaxStudentAcademicsId = _protector.Protect(id.ToString());
            }


            //end
            var instituteList = (from s in _instituteService.AffiliationInstituteIntakeData().Result
                                 select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
            ViewBag.InstituteList = instituteList;
            return View("~/Views/Exam/StudentAcademics/Index.cshtml", data);
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _studentAcademicsService.GetStudentAcademicsByIdAsync(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/StudentAcademics/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        public JsonResult EditCustomer(int InstituteId)
        {
            //int id = Convert.ToInt32(cust.AcademicId);
            StudentAcademicsModel customer = new StudentAcademicsModel();
            customer.AcademicId = InstituteId;
            //customer.Name = GetCustomers(id).FirstOrDefault().Name;
            //customer.Country = GetCustomers(id).FirstOrDefault().Country;
            //customer.Customers = GetCustomers(null);
            return Json(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int? AcademicId, StudentAcademicsModel studentAcademicsModel)
        {
            try
            {
                studentAcademicsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                studentAcademicsModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                studentAcademicsModel.ApprovedBy = HttpContext.Session.GetInt32("UserId");
                if (studentAcademicsModel.ApprovedStatus != "" && AcademicId != null)
                {
                    var value = await _studentAcademicsService.GetStudentAcademicsByIdAsync((int)AcademicId);
                    //if (await TryUpdateModelAsync<StudentAcademicsModel>(value))
                    //{
                        var res = await _studentAcademicsService.InsertUpdateApprovalAsync(studentAcademicsModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Data has been updated";
                            ModelState.AddModelError("", "Data has been updated");
                        }
                        else
                        {
                            TempData["error"] = "Student has not been updated";
                            ModelState.AddModelError("", "Data has not been updated");
                        }
                    //}
                }
                else
                {
                    ModelState.AddModelError("", "Some thing went wrong !");
                    TempData["error"] = "Model state is not valid";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [Obsolete]
        public async Task<IActionResult> Download(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _studentAcademicsService.GetStudentAcademicsByIdAsync(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    var path = string.Empty;
                    var ext = string.Empty;
                    if (data != null)
                    {
                        string ApprovalLetter = _configuration.GetSection("FilePaths:PreviousDocuments:ApprovalLetter").Value.ToString();
                        path = Path.Combine(ApprovalLetter, data.ApprovalLetterPath);
                        ext = Path.GetExtension(data.ApprovalLetterPath).Substring(1);
                    }

                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    if (ext == "png" || ext == "PNG")
                    {
                        return File(FileBytes, "image/png");
                    }
                    else if (ext == "jpg" || ext == "JPG")
                    {
                        return File(FileBytes, "image/jpg");
                    }
                    else if (ext == "jpeg" || ext == "JPEG")
                    {
                        return File(FileBytes, "image/jpeg");
                    }
                    else
                    {
                        return File(FileBytes, "application/pdf");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return await Index();
        }
    }
}
