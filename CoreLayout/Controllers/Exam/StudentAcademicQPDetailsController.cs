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
    public class StudentAcademicQPDetailsController : Controller
    {
        private readonly ILogger<StudentAcademicQPDetailsController> _logger;
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
        public StudentAcademicQPDetailsController(ILogger<StudentAcademicQPDetailsController> logger, IDataProtectionProvider provider,
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
        public async Task<IActionResult> CreateAsync(string id, int? courseid, int? subjectid, int? semyearid, int? syllabussessionid, int? examid)
        {
            StudentAcademicQPDetailsModel studentAcademicQPDetailsModel = new StudentAcademicQPDetailsModel();
            if (id != null && courseid != null && subjectid != null  && semyearid != null && syllabussessionid != null && examid != null)
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
                studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(studentAcademicQPDetailsModel.AcademicId,(int)courseid, (int)subjectid, (int)semyearid, (int)syllabussessionid, (int)examid);
                if (studentAcademicQPDetailsModel.DataList != null)
                {
                    foreach (var _data in studentAcademicQPDetailsModel.DataList)
                    {
                        var stringId = _data.StudentAcademicQPId.ToString();
                        _data.EncryptedId = _protector.Protect(stringId);
                    }
                }
            }
            return View("~/Views/Exam/StudentAcademicQPDetails/Create.cshtml", studentAcademicQPDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            string counter = string.Empty;
            studentAcademicQPDetailsModel.QPList = await _qPMasterService.GetAllQPByFilter(studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId);
            //ViewBag.QPListForInsert = (from s in await _qPMasterService.GetAllQPByFilter(studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId)
            //                                        select s).Distinct().ToList();

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

            #region check qp already exit in selected course,subject,semyear & session
            studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(studentAcademicQPDetailsModel.AcademicId, studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId, studentAcademicQPDetailsModel.ExamId);
            if (studentAcademicQPDetailsModel.DataList != null && studentAcademicQPDetailsModel.QPListForInsert !=null)
            {
               
                foreach (var _data in studentAcademicQPDetailsModel.DataList)
                {
                    foreach(int _data1 in studentAcademicQPDetailsModel.QPListForInsert)
                    {
                        if (_data.QPId == _data1)
                        {
                            counter += _data.QPCode+",";
                        }
                    }
                  
                }
                if(counter!="")
                {
                    ModelState.AddModelError("", "QP Code "+ counter +" already exit.Please unselect this qp code and try again");
                    return View("~/Views/Exam/StudentAcademicQPDetails/Create.cshtml", studentAcademicQPDetailsModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select atleast one qp code");
                return View("~/Views/Exam/StudentAcademicQPDetails/Create.cshtml", studentAcademicQPDetailsModel);
            }
            #endregion

            studentAcademicQPDetailsModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            studentAcademicQPDetailsModel.IPAddress = HttpContext.Session.GetString("IPAddress");

            if (ModelState.IsValid)
            {
                var res =await _studentAcademicQPDetailsService.CreateStudentAcademicsQPDetailsAsync(studentAcademicQPDetailsModel);
                if (res.Equals(1))
                {
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
            //bind data after insert
            studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(studentAcademicQPDetailsModel.AcademicId,studentAcademicQPDetailsModel.CourseId, studentAcademicQPDetailsModel.SubjectId, studentAcademicQPDetailsModel.SemYearId, studentAcademicQPDetailsModel.SyllabusSessionId, studentAcademicQPDetailsModel.ExamId);
            if (studentAcademicQPDetailsModel.DataList != null)
            {
                foreach (var _data in studentAcademicQPDetailsModel.DataList)
                {
                    var stringId = _data.StudentAcademicQPId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
            }
            return View("~/Views/Exam/StudentAcademicQPDetails/Create.cshtml", studentAcademicQPDetailsModel);
        }
        public async Task<IActionResult> Delete(string id, StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _studentAcademicQPDetailsService.GetStudentAcademicsQPDetailsByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    studentAcademicQPDetailsModel.QPList = await _qPMasterService.GetAllQPByFilter(value.CourseId, value.SubjectId, value.SemYearId, value.SyllabusSessionId);

                    studentAcademicQPDetailsModel.CourseList = (from s in await _courseService.GetAllCourse()
                                                                where s.CourseID == value.CourseId
                                                                select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SubjectList = (from s in await _branchService.GetAllBranch()
                                                                 where s.BranchID == value.SubjectId
                                                                 select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.SemYearId = value.SemYearId;
                    studentAcademicQPDetailsModel.SyllabusSessionList = (from s in await _courseDetailsService.GetAllSession()
                                                                         where s.SessionId == value.SyllabusSessionId
                                                                         select s).Distinct().ToList();
                    studentAcademicQPDetailsModel.AcademicId = value.AcademicId;
                    studentAcademicQPDetailsModel.ExamList = (from s in await _examMasterService.GetAllExamMasterAsync()
                                                              where s.ExamId == value.ExamId
                                                              select s).Distinct().ToList();

                    if (value != null)
                    {
                        var res = await _studentAcademicQPDetailsService.DeleteStudentAcademicsQPDetailsAsync(value);
                        if (res.Equals(1))
                        {
                            //TempData["success"] = "Data has been deleted";
                            ModelState.AddModelError("", "Data has been deleted");
                        }
                        else
                        {
                            //TempData["error"] = "StudentAcademics has not been deleted";
                            ModelState.AddModelError("", "Data has not been deleted");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Some thing went wrong!";
                    }
                    //bind data after delete
                    studentAcademicQPDetailsModel.DataList = await _studentAcademicQPDetailsService.GetFilterStudentAcademicsQPData(value.AcademicId,value.CourseId, value.SubjectId, value.SemYearId, value.SyllabusSessionId, value.ExamId);
                    if (studentAcademicQPDetailsModel.DataList != null)
                    {
                        foreach (var _data in studentAcademicQPDetailsModel.DataList)
                        {
                            var stringId = _data.StudentAcademicQPId.ToString();
                            _data.EncryptedId = _protector.Protect(stringId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return View("~/Views/Exam/StudentAcademicQPDetails/Create.cshtml", studentAcademicQPDetailsModel);
        }
    }
}
