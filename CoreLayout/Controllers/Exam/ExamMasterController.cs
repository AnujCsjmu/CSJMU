using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Branch;
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
    public class ExamMasterController : Controller
    {
        private readonly ILogger<ExamMasterController> _logger;
        private readonly IDataProtector _protector;
        private readonly IExamMasterService _examMasterService;
        private readonly ICourseDetailsService _courseDetailsService;
        public ExamMasterController(ILogger<ExamMasterController> logger, IDataProtectionProvider provider, IExamMasterService examMasterService, ICourseDetailsService courseDetailsService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("ExamMaster.ExamMasterController");
            _courseDetailsService = courseDetailsService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _examMasterService.GetAllExamMasterAsync();
                foreach (var _data in data)
                {
                    var stringId = _data.ExamId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.ExamId;
                }
                id = id + 1;
                ViewBag.MaxExamId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/Exam/ExamMaster/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamMaster/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _examMasterService.GetExamMasterByIdAsync(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/ExamMaster/Details.cshtml",data);

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
                ExamMasterModel examMasterModel = new ExamMasterModel();
                examMasterModel.SessionList = await _courseDetailsService.GetAllSession();//sllabus
                return View("~/Views/Exam/ExamMaster/Create.cshtml", examMasterModel);
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
        public async Task<IActionResult> Create(ExamMasterModel examMasterModel)
        {
            examMasterModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            examMasterModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            examMasterModel.SessionList = await _courseDetailsService.GetAllSession();//sllabus
            if (ModelState.IsValid)
            {

                var res = await _examMasterService.CreateExamMasterAsync(examMasterModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Exam has been saved";
                }
                else
                {
                    TempData["error"] = "Exam has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View("~/Views/Exam/ExamMaster/Create.cshtml", examMasterModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _examMasterService.GetExamMasterByIdAsync(Convert.ToInt32(guid_id));
                data.SessionList = await _courseDetailsService.GetAllSession();//sllabus
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/ExamMaster/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamMaster/Edit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int ExamId, ExamMasterModel examMasterModel)
        {
            try
            {
                examMasterModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                examMasterModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                examMasterModel.SessionList = await _courseDetailsService.GetAllSession();//sllabus
                if (ModelState.IsValid)
                {
                    var value = await _examMasterService.GetExamMasterByIdAsync(ExamId);
                    if (await TryUpdateModelAsync<ExamMasterModel>(value))
                    {
                        var res = await _examMasterService.UpdateExamMasterAsync(examMasterModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Exam has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Exam has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/ExamMaster/Edit.cshtml", examMasterModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _examMasterService.GetExamMasterByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _examMasterService.DeleteExamCourseMappingAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Exam has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Exam has not been deleted";
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
        public IActionResult VerifyName(string examName)
        {

            var already = (from data in _examMasterService.GetAllExamMasterAsync().Result
                           where data.ExamName == examName.Trim()
                          select data).ToList();

            if (already.Count > 0)
            {
                return Json($"{examName} is already in use.");
            }

            return Json(true);


        }
    }
}
