using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using CoreLayout.Models.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using CoreLayout.Filters;
using CoreLayout.Enum;
using CoreLayout.Services.Masters.Program;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.Course;

namespace CoreLayout.Controllers.Masters
{
    [Authorize(Roles = "Administrator")]
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IDataProtector _protector;
        private readonly ICourseService _courseService;
        private readonly IProgramService _programService;
        public CourseController(ILogger<CourseController> logger, IDataProtectionProvider provider, ICourseService courseService, IProgramService programService)
        {
            _logger = logger;
            _courseService = courseService;
            _protector = provider.CreateProtector("Course.CourseController");
            _programService = programService;
        }



        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var course = await _courseService.GetAllCourse();
                foreach (var _course in course)
                {
                    var stringId = _course.CourseID.ToString();
                    _course.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxcourseid = 0;
                foreach (var _course in course)
                {
                    maxcourseid = _course.CourseID;
                }
                maxcourseid = maxcourseid + 1;
                ViewBag.MaxCourseId = _protector.Protect(maxcourseid.ToString());
                //end
                return View(course);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _courseService.GetCourseById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);

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
                CourseModel courseModel = new CourseModel();
                courseModel.ProgramList = await _programService.GetAllProgram();
                courseModel.CourseTypeList = await _courseService.GetAllCourseType();
                var guid_id = _protector.Unprotect(id);
                return View(courseModel);
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
        public async Task<IActionResult> Create(CourseModel courseModel)
        {
            courseModel.ProgramList = await _programService.GetAllProgram();
            courseModel.CourseTypeList = await _courseService.GetAllCourseType();
            courseModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            courseModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
            courseModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {

                var res = await _courseService.CreateCourseAsync(courseModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Course has been saved";
                }
                else
                {
                    TempData["error"] = "Course has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View(courseModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _courseService.GetCourseById(Convert.ToInt32(guid_id));
                data.ProgramList = await _programService.GetAllProgram();
                data.CourseTypeList = await _courseService.GetAllCourseType();
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int CourseId, CourseModel courseModel)
        {
            try
            {
                courseModel.ProgramList = await _programService.GetAllProgram();
                courseModel.CourseTypeList = await _courseService.GetAllCourseType();
                courseModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                courseModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                courseModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _courseService.GetCourseById(CourseId);
                    if (await TryUpdateModelAsync<CourseModel>(value))
                    {
                        var res = await _courseService.UpdateCourseAsync(courseModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Course has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Course has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(courseModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _courseService.GetCourseById(Convert.ToInt32(guid_id));
                value.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                value.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (value != null)
                {
                    var res = await _courseService.DeleteCourseAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Course has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Course has not been deleted";
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


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string courseCode)
        {

            var already = (from course in _courseService.GetAllCourse().Result
                           where course.CourseCode == courseCode.Trim()
                           select new SelectListItem()
                           {
                               Text = course.CourseCode,
                               Value = course.CourseID.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{courseCode} is already in use.");
            }

            return Json(true);


        }
    }
}
