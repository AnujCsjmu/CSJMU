using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.Exam
{

    [Authorize(Roles = "Administrator")]
    public class ExamCourseMappingController : Controller
    {
        private readonly ILogger<ExamCourseMappingController> _logger;
        private readonly IDataProtector _protector;
        private readonly IExamMasterService _examMasterService;
        //private readonly ICourseDetailsService _courseDetailsService;
        private readonly IExamCourseMappingService _examCourseMappingService;
        private readonly ICourseService _courseService;
        public ExamCourseMappingController(ILogger<ExamCourseMappingController> logger, IDataProtectionProvider provider, IExamMasterService examMasterService, IExamCourseMappingService examCourseMappingService, ICourseService courseService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("ExamCourseMapping.ExamCourseMappingController");
           // _courseDetailsService = courseDetailsService;
            _examCourseMappingService = examCourseMappingService;
            _courseService = courseService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _examCourseMappingService.GetAllExamCourseMappingAsync();
                foreach (var _data in data)
                {
                    var stringId = _data.ECId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.ECId;
                }
                id = id + 1;
                ViewBag.MaxECId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/Exam/ExamCourseMapping/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamCourseMapping/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _examCourseMappingService.GetExamCourseMappingByIdAsync(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/ExamCourseMapping/Details.cshtml", data);

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
                ExamCourseMappingModel examCourseMappingModel  = new ExamCourseMappingModel();
                examCourseMappingModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                examCourseMappingModel.CourseList = await _courseService.GetAllCourse();
                return View("~/Views/Exam/ExamCourseMapping/Create.cshtml", examCourseMappingModel);
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
        public async Task<IActionResult> Create(ExamCourseMappingModel examCourseMappingModel)
        {
            examCourseMappingModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            examCourseMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            examCourseMappingModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
            examCourseMappingModel.CourseList = await _courseService.GetAllCourse();
            if (ModelState.IsValid)
            {

                var res = await _examCourseMappingService.CreateExamCourseMappingAsync(examCourseMappingModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "ExamCourseMapping has been saved";
                }
                else
                {
                    TempData["error"] = "ExamCourseMapping has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View("~/Views/Exam/ExamCourseMapping/Create.cshtml", examCourseMappingModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _examCourseMappingService.GetExamCourseMappingByIdAsync(Convert.ToInt32(guid_id));
                data.ExamList = await _examMasterService.GetAllExamMasterAsync();
                data.CourseList = await _courseService.GetAllCourse();
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/ExamCourseMapping/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamCourseMapping/Edit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int ECId, ExamCourseMappingModel examCourseMappingModel)
        {
            try
            {
                examCourseMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                examCourseMappingModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                examCourseMappingModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                examCourseMappingModel.CourseList = await _courseService.GetAllCourse();
                if (ModelState.IsValid)
                {
                    var value = await _examCourseMappingService.GetExamCourseMappingByIdAsync(ECId);
                    if (await TryUpdateModelAsync<ExamCourseMappingModel>(value))
                    {
                        var res = await _examCourseMappingService.UpdateExamCourseMappingAsync(examCourseMappingModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "ExamCourseMapping has been updated";
                        }
                        else
                        {
                            TempData["error"] = "ExamCourseMapping has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamCourseMapping/Edit.cshtml", examCourseMappingModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _examCourseMappingService.GetExamCourseMappingByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _examCourseMappingService.DeleteExamCourseMappingAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "ExamCourseMapping has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "ExamCourseMapping has not been deleted";
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


        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyName(string examName)
        //{

        //    var already = (from data in _examMasterService.GetAllExamMasterAsync().Result
        //                   where data.ExamName == examName.Trim()
        //                   select data).ToList();

        //    if (already.Count > 0)
        //    {
        //        return Json($"{examName} is already in use.");
        //    }

        //    return Json(true);


        //}
    }
}
