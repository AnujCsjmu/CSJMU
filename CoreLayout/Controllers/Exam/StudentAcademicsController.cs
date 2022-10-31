﻿using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Helper;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Exam.Student;
using CoreLayout.Services.Exam.StudentAcademics;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
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
        public StudentAcademicsController(ILogger<StudentAcademicsController> logger, IDataProtectionProvider provider, 
            IExamMasterService examMasterService, ICourseDetailsService courseDetailsService, IStudentAcademicsService studentAcademicsService,
            IStudentService studentService, IConfiguration configuration, ICourseService courseService, 
            IBranchService branchService, ICourseBranchMappingService courseBranchMappingService, 
            IInstituteService instituteService, IExamCourseMappingService examCourseMappingService)
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
                return View("~/Views/Exam/StudentAcademics/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/StudentAcademics/Index.cshtml");
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
            try
            {
                var guid_id = _protector.Unprotect(id);
                StudentAcademicsModel studentAcademicsModel = new StudentAcademicsModel();
                studentAcademicsModel.StudentList = await _studentService.GetAllStudentAsync();
                studentAcademicsModel.InstituteList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.CourseList = await _courseService.GetAllCourse();
                studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();

                return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(StudentAcademicsModel studentAcademicsModel)
        {
            try
            {
                studentAcademicsModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                studentAcademicsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
               
   
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

                        //TempData["error"] = "StudentAcademics has not been saved";
                        ModelState.AddModelError("", "StudentAcademics has not been saved");
                        return View("~/Views/Exam/StudentAcademics/Create.cshtml", studentAcademicsModel);
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
                data.StudentList = await _studentService.GetAllStudentAsync();
                data.InstituteList = await _instituteService.GetAllInstitute();
                data.CourseList = await _courseService.GetAllCourse();
                data.SubjectList = await _branchService.GetAllBranch();
                data.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                data.ExamCenterList = await _instituteService.GetAllInstitute();
                data.AcademicSessionList = await _courseDetailsService.GetAllSession();
                data.ExamList = await _examMasterService.GetAllExamMasterAsync();
                data.PreviousSessionIdList = await _courseDetailsService.GetAllSession();
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
                studentAcademicsModel.StudentList = await _studentService.GetAllStudentAsync();
                studentAcademicsModel.InstituteList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.CourseList = await _courseService.GetAllCourse();
                studentAcademicsModel.SubjectList = await _branchService.GetAllBranch();
                studentAcademicsModel.SyllabusSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamCenterList = await _instituteService.GetAllInstitute();
                studentAcademicsModel.AcademicSessionList = await _courseDetailsService.GetAllSession();
                studentAcademicsModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                studentAcademicsModel.PreviousSessionIdList = await _courseDetailsService.GetAllSession();

                if (ModelState.IsValid)
                {
                    var value = await _studentAcademicsService.GetStudentAcademicsByIdAsync(AcademicId);
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
        public async Task<JsonResult> GetBranch(int CourseId)
        {
            var BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                              where coursbranchemapping.CourseId == CourseId
                              select new SelectListItem()
                              {
                                  Text = coursbranchemapping.BranchName,
                                  Value = coursbranchemapping.BranchId.ToString(),
                              }).ToList();

            BranchList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(BranchList);
        }
    }
}