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
    public class CourseDetailsController : Controller
    {
        private readonly ILogger<CourseDetailsController> _logger;
        private readonly IDataProtector _protector;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly ICourseService _courseService;
        public CourseDetailsController(ILogger<CourseDetailsController> logger, IDataProtectionProvider provider, ICourseDetailsService courseDetailsService, ICourseService courseService)
        {
            _logger = logger;
            _courseDetailsService = courseDetailsService;
            _protector = provider.CreateProtector("CourseDetails.CourseDetailsController");
            _courseService = courseService;
        }



        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var coursedetails = await _courseDetailsService.GetAllCourseDetail();
                foreach (var _coursedetails in coursedetails)
                {
                    var stringId = _coursedetails.CourseDetailId.ToString();
                    _coursedetails.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxcoursedetailsid = 0;
                foreach (var _coursedetails in coursedetails)
                {
                    maxcoursedetailsid = _coursedetails.CourseDetailId;
                }
                maxcoursedetailsid = maxcoursedetailsid + 1;
                ViewBag.MaxCourseDetailsId = _protector.Protect(maxcoursedetailsid.ToString());
                //end
                return View(coursedetails);

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
                var data = await _courseDetailsService.GetCourseDetailById(Convert.ToInt32(guid_id));
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

        //public void BindCountry()
        //{
        //    var countryList = (from country in _countryService.GetAllCountry().Result
        //                       select new SelectListItem()
        //                       {
        //                           Text = country.CountryName,
        //                           Value = country.CountryId.ToString(),
        //                       }).ToList();

        //    countryList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    ViewBag.CountryList = countryList;//roleList.Select(l => l.CountryId).ToList();
        //}

        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                CourseDetailsModel courseDetailsModel = new CourseDetailsModel();
                courseDetailsModel.SessionList = await _courseDetailsService.GetAllSession();
                courseDetailsModel.CourseList = await _courseService.GetAllCourse();
                var guid_id = _protector.Unprotect(id);
                return View(courseDetailsModel);
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
        public async Task<IActionResult> Create(CourseDetailsModel courseDetailsModel)
        {
            courseDetailsModel.SessionList = await _courseDetailsService.GetAllSession();
            courseDetailsModel.CourseList = await _courseService.GetAllCourse();
            courseDetailsModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            courseDetailsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {

                var res = await _courseDetailsService.CreateCourseDetailAsync(courseDetailsModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Course Details has been saved";
                }
                else
                {
                    TempData["error"] = "Course Details has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View(courseDetailsModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _courseDetailsService.GetCourseDetailById(Convert.ToInt32(guid_id));
                data.SessionList = await _courseDetailsService.GetAllSession();
                data.CourseList = await _courseService.GetAllCourse();
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
        public async Task<IActionResult> Edit(int CourseDetailId, CourseDetailsModel courseDetailsModel)
        {
            try
            {
                courseDetailsModel.SessionList= await _courseDetailsService.GetAllSession();
                courseDetailsModel.CourseList = await _courseService.GetAllCourse();
                courseDetailsModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                courseDetailsModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _courseDetailsService.GetCourseDetailById(CourseDetailId);
                    if (await TryUpdateModelAsync<CourseDetailsModel>(value))
                    {
                        var res = await _courseDetailsService.UpdateCourseDetailAsync(courseDetailsModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Course Details has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Course Details has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(courseDetailsModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _courseDetailsService.GetCourseDetailById(Convert.ToInt32(guid_id));
                value.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (value != null)
                {
                    var res = await _courseDetailsService.DeleteCourseDetailAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Course Details has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Course Details has not been deleted";
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
        //public IActionResult VerifyName(string programName)
        //{

        //    var already = (from coursedetails in _courseDetailsService.GetAllCourseDetail().Result
        //                   where coursedetails.ProgramName == programName.Trim()
        //                   select new SelectListItem()
        //                   {
        //                       Text = coursedetails.ProgramName,
        //                       Value = coursedetails.ProgramId.ToString(),
        //                   }).ToList();

        //    if (already.Count > 0)
        //    {
        //        return Json($"{programName} is already in use.");
        //    }

        //    return Json(true);


        //}
    }
}
