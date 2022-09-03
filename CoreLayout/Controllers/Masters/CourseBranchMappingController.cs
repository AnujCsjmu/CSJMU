using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
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

namespace CoreLayout.Controllers.Masters
{
    [Authorize(Roles = "Administrator")]
    public class CourseBranchMappingController : Controller
    {
        private readonly ILogger<CourseBranchMappingController> _logger;
        private readonly IDataProtector _protector;
        private readonly IBranchService _branchService;
        private readonly ICourseService _courseService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        public CourseBranchMappingController(ILogger<CourseBranchMappingController> logger, IDataProtectionProvider provider,
            IBranchService branchService, ICourseService courseService, ICourseBranchMappingService courseBranchMappingService)
        {
            _logger = logger;
            _branchService = branchService;
            _protector = provider.CreateProtector("CourseBranchMapping.CourseBranchMappingController");
            _courseService = courseService;
            _courseBranchMappingService = courseBranchMappingService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _courseBranchMappingService.GetAllCourseBranchMapping();
                foreach (var _data in data)
                {
                    var stringId = _data.CBId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxid = 0;
                foreach (var _data in data)
                {
                    maxid = _data.CBId;
                }
                maxid = maxid + 1;
                ViewBag.CourseBranchMappingId = _protector.Protect(maxid.ToString());
                //end
                return View(data);

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
                var data = await _courseBranchMappingService.GetCourseBranchMappingById(Convert.ToInt32(guid_id));
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
                CourseBranchMappingModel data = new CourseBranchMappingModel();
                data.CourseList = await _courseService.GetAllCourse();
                data.BranchList= await _branchService.GetAllBranch();
                if(data==null)
                {
                    return NotFound();
                }
                var guid_id = _protector.Unprotect(id);
                return View(data);
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
        public async Task<IActionResult> Create(CourseBranchMappingModel courseBranchMappingModel)
        {
            courseBranchMappingModel.CourseList = await _courseService.GetAllCourse();
            courseBranchMappingModel.BranchList = await _branchService.GetAllBranch();
            courseBranchMappingModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            courseBranchMappingModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
            courseBranchMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
           
            if (ModelState.IsValid)
            {
                var alreadyExit = _courseBranchMappingService.alreadyExit(courseBranchMappingModel.CourseId, courseBranchMappingModel.BranchId);
                if (alreadyExit.Result.Count != 1)
                {
                    var res = await _courseBranchMappingService.CreateCourseBranchMappingAsync(courseBranchMappingModel);

                    if (res.Equals(1))
                    {
                        TempData["success"] = "CourseBranchMapping has been saved";
                    }
                    else
                    {
                        TempData["error"] = "CourseBranchMapping has not been saved";
                    }
                }
                else
                {
                    TempData["error"] = "data already exit";
                }    
                return RedirectToAction(nameof(Index));

            }
            return View(courseBranchMappingModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _courseBranchMappingService.GetCourseBranchMappingById(Convert.ToInt32(guid_id));
                data.CourseList = await _courseService.GetAllCourse();
                data.BranchList = await _branchService.GetAllBranch();
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
        public async Task<IActionResult> Edit(int CBId, CourseBranchMappingModel courseBranchMappingModel)
        {
            try
            {
                courseBranchMappingModel.CourseList = await _courseService.GetAllCourse();
                courseBranchMappingModel.BranchList = await _branchService.GetAllBranch();
                courseBranchMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                courseBranchMappingModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                courseBranchMappingModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _courseBranchMappingService.GetCourseBranchMappingById(CBId);
                    if (await TryUpdateModelAsync<CourseBranchMappingModel>(value))
                    {
                        var res = await _courseBranchMappingService.UpdateCourseBranchMappingAsync(courseBranchMappingModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "CourseBranchMapping has been updated";
                        }
                        else
                        {
                            TempData["error"] = "CourseBranchMapping has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(courseBranchMappingModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _courseBranchMappingService.GetCourseBranchMappingById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _courseBranchMappingService.DeleteCourseBranchMappingAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "CourseBranchMapping has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "CourseBranchMapping has not been deleted";
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
    }
}
