using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.QPDetails;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.Faculty;
using CoreLayout.Services.QPDetails.GradeDefinition;
using CoreLayout.Services.QPDetails.QPMaster;
using CoreLayout.Services.QPDetails.QPType;
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

namespace CoreLayout.Controllers.QPDetails
{
    [Authorize(Roles = "Administrator")]
    public class QPMasterController : Controller
    {
        private readonly ILogger<QPMasterController> _logger;
        private readonly IDataProtector _protector;
        private readonly IQPTypeService _qPTypeService;
        private readonly IQPMasterService _qPMasterService;
        private readonly IFacultyService _facultyService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IGradeDefinitionService _gradeDefinitionService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        public QPMasterController(ILogger<QPMasterController> logger, IDataProtectionProvider provider,
            IQPTypeService qPTypeService, IQPMasterService qPMasterService, IFacultyService facultyService,
            ICourseService courseService, IBranchService branchService, ICourseDetailsService courseDetailsService,
            IGradeDefinitionService gradeDefinitionService, ICourseBranchMappingService courseBranchMappingService)
        {
            _logger = logger;
            _qPTypeService = qPTypeService;
            _protector = provider.CreateProtector("QPMaster.QPMasterController");
            _qPMasterService = qPMasterService;
            _facultyService = facultyService;
            _courseService = courseService;
            _branchService = branchService;
            _courseDetailsService = courseDetailsService;
            _gradeDefinitionService = gradeDefinitionService;
            _courseBranchMappingService = courseBranchMappingService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _qPMasterService.GetAllQPMaster();
                foreach (var _data in data)
                {
                    var stringId = _data.QPId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.QPId;
                }
                id = id + 1;
                ViewBag.MaxQPMasterId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/QPDetails/QPMaster/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/QPMaster/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _qPMasterService.GetQPMasterById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/QPDetails/QPMaster/Details.cshtml", data);

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
                QPMasterModel qPMasterModel = new QPMasterModel();
                qPMasterModel.QPTypeList = await _qPTypeService.GetAllQPType();//
                qPMasterModel.FacultyList = await _facultyService.GetAllFaculty();
                qPMasterModel.CourseList = await _courseService.GetAllCourse();
                //qPMasterModel.BranchList = await _branchService.GetAllBranch();//subject
                qPMasterModel.SessionList = await _courseDetailsService.GetAllSession();//sllabus
                qPMasterModel.GradeList = await _gradeDefinitionService.GetAllGradeDefinition();//
                var guid_id = _protector.Unprotect(id);
                return View("~/Views/QPDetails/QPMaster/Create.cshtml", qPMasterModel);
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
        public async Task<IActionResult> CreateAsync(QPMasterModel qPMasterModel)
        {
            qPMasterModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            qPMasterModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            qPMasterModel.QPTypeList = await _qPTypeService.GetAllQPType();//
            qPMasterModel.FacultyList = await _facultyService.GetAllFaculty();
            qPMasterModel.CourseList = await _courseService.GetAllCourse();
            qPMasterModel.BranchList = await _branchService.GetAllBranch();//subject
            qPMasterModel.SessionList = await _courseDetailsService.GetAllSession();//course
            qPMasterModel.GradeList = await _gradeDefinitionService.GetAllGradeDefinition();//course
            if (ModelState.IsValid)
            {
                var res = await _qPMasterService.CreateQPMasterAsync(qPMasterModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "QPMaster has been saved";
                }
                else
                {
                    TempData["error"] = "QPMaster has not been saved";
                }
                return RedirectToAction("Create");
            }
            return View("~/Views/QPDetails/QPMaster/Create.cshtml", qPMasterModel);
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _qPMasterService.GetQPMasterById(Convert.ToInt32(guid_id));
                data.QPTypeList = await _qPTypeService.GetAllQPType();//
                data.FacultyList = await _facultyService.GetAllFaculty();
                data.CourseList = await _courseService.GetAllCourse();
                data.BranchList = await _branchService.GetAllBranch();//subject
                data.SessionList = await _courseDetailsService.GetAllSession();//syllabus
                data.GradeList = await _gradeDefinitionService.GetAllGradeDefinition();

                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/QPDetails/QPMaster/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/QPMaster/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int QPId, QPMasterModel qPMasterModel)
        {
            try
            {
                qPMasterModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                qPMasterModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                qPMasterModel.QPTypeList = await _qPTypeService.GetAllQPType();//
                qPMasterModel.FacultyList = await _facultyService.GetAllFaculty();
                qPMasterModel.CourseList = await _courseService.GetAllCourse();
                qPMasterModel.BranchList = await _branchService.GetAllBranch();//subject
                qPMasterModel.SessionList = await _courseDetailsService.GetAllSession();//syllabus
                qPMasterModel.GradeList = await _gradeDefinitionService.GetAllGradeDefinition();
                if (ModelState.IsValid)
                {
                    var value = await _qPMasterService.GetQPMasterById(QPId);
                    if (await TryUpdateModelAsync<QPMasterModel>(value))
                    {
                        var res = await _qPMasterService.UpdateQPMasterAsync(qPMasterModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "QPMaster has been updated";
                        }
                        else
                        {
                            TempData["error"] = "QPMaster has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/QPMaster/Edit.cshtml", qPMasterModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _qPMasterService.GetQPMasterById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _qPMasterService.DeleteQPMasterAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "QPMaster has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "QPMaster has not been deleted";
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
        public IActionResult VerifyName(string qPCode)
        {

            var already = (from data in _qPMasterService.GetAllQPMaster().Result
                           where data.QPCode == qPCode.Trim()
                           select new SelectListItem()
                           {
                               Text = data.QPCode,
                               Value = data.QPId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{qPCode} is already in use.");
            }
            return Json(true);
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

