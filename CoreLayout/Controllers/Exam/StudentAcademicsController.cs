using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Helper;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Exam.Student;
using CoreLayout.Services.Exam.StudentAcademicQPDetails;
using CoreLayout.Services.Exam.StudentAcademics;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.ExamCategory;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CoreLayout.Controllers.Exam
{
    [Authorize(Roles = "Administrator")]
    public class StudentAcademicsController : Controller
    {
        private readonly ILogger<StudentAcademicsController> _logger;
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
        public StudentAcademicsController(ILogger<StudentAcademicsController> logger, IDataProtectionProvider provider,
            IExamMasterService examMasterService, ICourseDetailsService courseDetailsService, IStudentAcademicsService studentAcademicsService,
            IStudentService studentService, IConfiguration configuration, ICourseService courseService,
            IBranchService branchService, ICourseBranchMappingService courseBranchMappingService,
            IInstituteService instituteService, IExamCourseMappingService examCourseMappingService, IExamCategoryService examCategoryService,
            IQPMasterService qPMasterService, IStudentAcademicQPDetailsService studentAcademicQPDetailsService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("StudentAcademics.StudentAcademicsController");
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

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.AcademicId;
                }
                id = id + 1;
                ViewBag.MaxStudentAcademicsId = _protector.Protect(id.ToString());
                //end
                var instituteList = (from s in _instituteService.AffiliationInstituteIntakeData().Result
                                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
                ViewBag.InstituteList = instituteList;

                return View("~/Views/Exam/StudentAcademics/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/StudentAcademics/Index.cshtml");
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


        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            StudentAcademicsModel studentAcademicsModel = new StudentAcademicsModel();
            try
            {
                var guid_id = _protector.Unprotect(id);
                var instituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
                ViewBag.InstituteList = instituteList;
                studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCategoryList = await _examCategoryService.GetExamCategoryAsync();

                return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
            }
        }



        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(StudentAcademicsModel studentAcademicsModel)
        {
            FileHelper fileHelper = new FileHelper();
            string ApprovalLetter = _configuration.GetSection("FilePaths:PreviousDocuments:ApprovalLetter").Value.ToString();
            try
            {
                studentAcademicsModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                studentAcademicsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                var instituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
                ViewBag.InstituteList = instituteList;
                studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCategoryList = await _examCategoryService.GetExamCategoryAsync();

                #region upload approval letter

                var supportedTypes = new[] { "pdf", "PDF" };

                if (studentAcademicsModel.FUApprovalLetter != null)
                {
                    var approvalLetterExt = Path.GetExtension(studentAcademicsModel.FUApprovalLetter.FileName).Substring(1);
                    if (supportedTypes.Contains(approvalLetterExt))
                    {
                        if (studentAcademicsModel.FUApprovalLetter.Length < 2100000)
                        {
                            studentAcademicsModel.ApprovalLetterPath = fileHelper.SaveFile(ApprovalLetter, "", studentAcademicsModel.FUApprovalLetter);
                        }
                        else
                        {
                            ModelState.AddModelError("", "approval letter size must be less than 2 mb");
                            return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "approval letter extension is invalid- accept only pdf");
                        return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                    }
                }
                #endregion
                if (ModelState.IsValid)
                {
                    var res = await _studentAcademicsService.CreateStudentAcademicsAsync(studentAcademicsModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "StudentAcademics has been saved";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        fileHelper.DeleteFileAnyException(ApprovalLetter, studentAcademicsModel.ApprovalLetterPath);
                        //TempData["error"] = "StudentAcademics has not been saved";
                        ModelState.AddModelError("", "StudentAcademics has not been saved");
                        return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                    }
                }
                else
                {
                    fileHelper.DeleteFileAnyException(ApprovalLetter, studentAcademicsModel.ApprovalLetterPath);
                    ModelState.AddModelError("", "Model state is not valid");
                    return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                }
            }
            catch (Exception ex)
            {
                fileHelper.DeleteFileAnyException(ApprovalLetter, studentAcademicsModel.ApprovalLetterPath);
                ModelState.AddModelError("", ex.ToString());
                return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
            }
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _studentAcademicsService.GetStudentAcademicsByIdAsync(Convert.ToInt32(guid_id));
                data.StudentList = (from s in await _studentService.GetAllStudentAsync()
                                    where s.RollNo == data.RollNo
                                    select s).Distinct().ToList();
                var instituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
                ViewBag.InstituteList = instituteList;
                data.CourseList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                   where s.InstituteID == data.InstituteID
                                   select s).Distinct().ToList();
                data.SubjectList = (from s in await _instituteService.All_AffiliationInstituteIntakeData()
                                    where s.CourseId == data.CourseId && s.InstituteID == data.InstituteID
                                    select s).Distinct().ToList();
                data.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                data.ExamCenterList = await _instituteService.GetAllInstitute();
                data.AcademicSessionList = await _courseDetailsService.GetAllSession();
                data.ExamList = await _examMasterService.GetAllExamMasterAsync();
                data.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
                data.ExamCategoryList = await _examCategoryService.GetExamCategoryAsync();
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/StudentAcademics/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/StudentAcademics/Edit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int AcademicId, StudentAcademicsModel studentAcademicsModel)
        {
            try
            {
                studentAcademicsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                studentAcademicsModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                var value = await _studentAcademicsService.GetStudentAcademicsByIdAsync(AcademicId);
                studentAcademicsModel.StudentList = (from s in await _studentService.GetAllStudentAsync()
                                                     where s.RollNo == value.RollNo
                                                     select s).Distinct().ToList();
                var instituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
                ViewBag.InstituteList = instituteList;
                studentAcademicsModel.CourseList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                                    where s.InstituteID == value.InstituteID
                                                    select s).Distinct().ToList();
                studentAcademicsModel.SubjectList = (from s in await _instituteService.All_AffiliationInstituteIntakeData()
                                                     where s.CourseId == value.CourseId && s.InstituteID == value.InstituteID
                                                     select s).Distinct().ToList();
                studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCategoryList = await _examCategoryService.GetExamCategoryAsync();

                #region upload approval letter
                string ApprovalLetter = _configuration.GetSection("FilePaths:PreviousDocuments:ApprovalLetter").Value.ToString();
                var supportedTypes = new[] { "pdf", "PDF" };
                FileHelper fileHelper = new FileHelper();
                if (studentAcademicsModel.FUApprovalLetter != null)
                {
                    var approvalLetterExt = Path.GetExtension(studentAcademicsModel.FUApprovalLetter.FileName).Substring(1);
                    if (supportedTypes.Contains(approvalLetterExt))
                    {
                        if (studentAcademicsModel.FUApprovalLetter.Length < 2100000)
                        {
                            studentAcademicsModel.ApprovalLetterPath = fileHelper.SaveFile(ApprovalLetter, "", studentAcademicsModel.FUApprovalLetter);
                        }
                        else
                        {
                            ModelState.AddModelError("", "approval letter size must be less than 2 mb");
                            return View("~/Views/Exam/StudentAcademics/Edit.cshtml", studentAcademicsModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "approval letter extension is invalid- accept only pdf");
                        return View("~/Views/Exam/StudentAcademics/Edit.cshtml", studentAcademicsModel);
                    }
                }
                else
                {

                    studentAcademicsModel.ApprovalLetterPath = value.ApprovalLetterPath;
                }
                #endregion
                if (ModelState.IsValid)
                {

                    if (await TryUpdateModelAsync<StudentAcademicsModel>(value))
                    {
                        var res = await _studentAcademicsService.UpdateStudentAcademicsAsync(studentAcademicsModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "StudentAcademics has been updated";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // TempData["error"] = "Student has not been updated";
                            ModelState.AddModelError("", "StudentAcademics has not been updated");
                            return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Model state is not valid");
                    return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/StudentAcademics/Edit.cshtml", studentAcademicsModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _studentAcademicsService.GetStudentAcademicsByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _studentAcademicsService.DeleteStudentAcademicsAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "StudentAcademics has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "StudentAcademics has not been deleted";
                    }
                }
                else
                {
                    TempData["error"] = "Some thing went wrong!";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetCourse(int InstituteId)
        {
            var CourseList = (from instituteintake in await _instituteService.AffiliationInstituteIntakeData()
                              where instituteintake.InstituteID == InstituteId
                              select new SelectListItem()
                              {
                                  Text = instituteintake.CourseName,
                                  Value = instituteintake.CourseId.ToString(),
                              }).Distinct().ToList();

            CourseList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(CourseList);
        }
        public async Task<JsonResult> GetBranch(int CourseId, int inst)
        {
            var BranchList = (from instituteintake in await _instituteService.All_AffiliationInstituteIntakeData()
                              where instituteintake.CourseId == CourseId && instituteintake.InstituteID == inst
                              select new SelectListItem()
                              {
                                  Text = instituteintake.BranchName,
                                  Value = instituteintake.BranchId.ToString(),
                              }).Distinct().ToList();

            BranchList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(BranchList);
        }
        public async Task<JsonResult> GetStudent(string rollno)
        {
            var studentlist = (from data in await _studentService.GetAllStudentAsync()
                               where data.RollNo == rollno.Trim()
                               select new SelectListItem()
                               {
                                   Text = data.FullName,
                                   Value = data.StudentID.ToString(),
                               }).ToList();

            return Json(studentlist);
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
        public async Task<ActionResult> PartialViewForAddSubject(string id, int? courseid, int? subjectid, int? semyearid, int? syllabussessionid, int? examid)
        {
            StudentAcademicQPDetailsModel studentAcademicQPDetailsModel = new StudentAcademicQPDetailsModel();
            try
            {
                if (id != null && courseid != null && subjectid != null && semyearid != null && syllabussessionid != null && examid != null)
                {
                    //ViewBag.type = 1;
                    var guid_id = _protector.Unprotect(id);

                    //var data = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(courseid, subjectid, semyearid);
                    studentAcademicQPDetailsModel.QPList = await _qPMasterService.GetAllQPByFilter((int)courseid, (int)subjectid, (int)semyearid, (int)syllabussessionid);

                    studentAcademicQPDetailsModel.CourseList = (from s in await _courseService.GetAllCourse()
                                                                where s.CourseID == courseid
                                                                select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SubjectList = (from s in await _branchService.GetAllBranch()
                                                                 where s.BranchID == subjectid
                                                                 select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SemYearId = (int)semyearid;
                    studentAcademicQPDetailsModel.SyllabusSessionList = (from s in await _courseDetailsService.GetAllSession()
                                                                         where s.SessionId == syllabussessionid
                                                                         select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.AcademicId = Convert.ToInt32(guid_id);
                    studentAcademicQPDetailsModel.ExamList = (from s in await _examMasterService.GetAllExamMasterAsync()
                                                              where s.ExamId == examid
                                                              select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData((int)courseid, (int)subjectid, (int)semyearid, (int)syllabussessionid, (int)examid);
                }
                else
                {
                    ModelState.AddModelError("", "Some thing went wrong!");
                    return View("~/Views/Exam/StudentAcademics/Index.cshtml", studentAcademicQPDetailsModel);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            //return PartialView("_AddSubject", data);
            return PartialView("~/Views/Exam/StudentAcademics/_AddSubject.cshtml", studentAcademicQPDetailsModel);

        }
        //[HttpGet]
        //public Task<IActionResult> AddSubject()
        //{
        //    StudentAcademicQPDetailsModel  studentAcademicQPDetailsModel = new StudentAcademicQPDetailsModel();
        //    try
        //    {
        //        ////var guid_id = _protector.Unprotect(id);
        //        //var instituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
        //        //                     select new { s.InstituteID, s.InstituteName }).Distinct().ToList();
        //        //ViewBag.InstituteList = instituteList;
        //        //studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
        //        //studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
        //        //studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
        //        //studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
        //        //studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
        //        //studentAcademicsModel.ExamCategoryList = await _examCategoryService.GetExamCategoryAsync();

        //        //return PartialView("~/Views/Exam/StudentAcademics/_AddSubject.cshtml", studentAcademicQPDetailsModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.ToString());
        //        //return PartialView("~/Views/Exam/StudentAcademics/_AddSubject.cshtml", studentAcademicQPDetailsModel);
        //    }
        //    return View(studentAcademicQPDetailsModel);
        //}

        [HttpPost]
        public async Task<IActionResult> AddSubject(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            try
            {
                if (studentAcademicQPDetailsModel.AcademicId != 0 && studentAcademicQPDetailsModel.CourseId != 0 && studentAcademicQPDetailsModel.SubjectId != 0 && studentAcademicQPDetailsModel.SemYearId != 0 && studentAcademicQPDetailsModel.SyllabusSessionId != 0 && studentAcademicQPDetailsModel.ExamId != 0)
                {
                    studentAcademicQPDetailsModel.QPList = await _qPMasterService.GetAllQPByFilter(studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId);

                    studentAcademicQPDetailsModel.CourseList = (from s in await _courseService.GetAllCourse()
                                                                where s.CourseID == studentAcademicQPDetailsModel.CourseId
                                                                select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SubjectList = (from s in await _branchService.GetAllBranch()
                                                                 where s.BranchID == studentAcademicQPDetailsModel.SubjectId
                                                                 select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SemYearId = studentAcademicQPDetailsModel.SemYearId;
                    studentAcademicQPDetailsModel.SyllabusSessionList = (from s in await _courseDetailsService.GetAllSession()
                                                                         where s.SessionId == studentAcademicQPDetailsModel.SyllabusSessionId
                                                                         select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.AcademicId = studentAcademicQPDetailsModel.AcademicId;
                    studentAcademicQPDetailsModel.ExamList = (from s in await _examMasterService.GetAllExamMasterAsync()
                                                              where s.ExamId == studentAcademicQPDetailsModel.ExamId
                                                              select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId, studentAcademicQPDetailsModel.ExamId);
                    studentAcademicQPDetailsModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    studentAcademicQPDetailsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    if (ModelState.IsValid)
                    {
                        var res = await _studentAcademicQPDetailsService.CreateStudentAcademicsQPDetailsAsync(studentAcademicQPDetailsModel);
                        if (res.Equals(1))
                        {
                            //TempData["success"] = "Data has been saved";
                            ModelState.AddModelError("", "Data has been saved");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Data has not been saved");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Model state is not valid");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return PartialView("~/Views/Exam/StudentAcademics/_AddSubject.cshtml", studentAcademicQPDetailsModel);
        }
        //[AcceptVerbs("GET", "POST")]
        //public async Task<IActionResult> VerifyRollNoAsync(string rollNo)
        //{

        //    var studentlist = (from data in await _studentService.GetAllStudentAsync()
        //                   where data.RollNo == rollNo.Trim()
        //                   select new SelectListItem()
        //                   {
        //                       Text = data.FullName,
        //                       Value = data.StudentID.ToString(),
        //                   }).ToList();

        //    if (studentlist.Count <= 0)
        //    {
        //        return Json($"{rollNo} is not found.");
        //    }
        //    ViewBag.Studentlist = studentlist;
        //    //return Json(studentlist);
        //    return View(studentlist);


        //}
        //    public async Task<JsonResult> GetStudent(int Instituteid,int courseid,int subjectid)
        //    {
        //        var StudentList = (from student in await _studentAcademicsService.GetAllStudentAcademicsAsync()
        //                           where student.InstituteID == Instituteid && student.CourseId == courseid && student.SubjectId == subjectid
        //                           select new SelectListItem()
        //                           {
        //                               Text = student.FullName,
        //                               Value = student.StudentID.ToString(),
        //                           }).ToList();

        //        StudentList.Insert(0, new SelectListItem()
        //        {
        //            Text = "----Select----",
        //            Value = string.Empty
        //        });
        //        return Json(StudentList);
        //    }
    }
}
