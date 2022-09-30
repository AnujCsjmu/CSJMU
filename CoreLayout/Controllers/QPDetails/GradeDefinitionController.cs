using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.QPDetails;
using CoreLayout.Services.QPDetails.GradeDefinition;
using CoreLayout.Services.QPDetails.QPType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.QPDetails
{
    [Authorize(Roles = "Administrator")]
    public class GradeDefinitionController : Controller
    {
        private readonly ILogger<GradeDefinitionController> _logger;
        private readonly IDataProtector _protector;
        private readonly IGradeDefinitionService _gradeDefinitionService;
        public GradeDefinitionController(ILogger<GradeDefinitionController> logger, IDataProtectionProvider provider, IGradeDefinitionService gradeDefinitionService)
        {
            _logger = logger;
            _gradeDefinitionService = gradeDefinitionService;
            _protector = provider.CreateProtector("GradeDefinition.GradeDefinitionController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _gradeDefinitionService.GetAllGradeDefinition();
                foreach (var _data in data)
                {
                    var stringId = _data.GradeId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.GradeId;
                }
                id = id + 1;
                ViewBag.MaxGradeId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/QPDetails/GradeDefinition/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/GradeDefinition/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _gradeDefinitionService.GetGradeDefinitionById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/QPDetails/GradeDefinition/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public IActionResult Create(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                return View("~/Views/QPDetails/GradeDefinition/Create.cshtml");
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
        public async Task<IActionResult> Create(GradeDefinitionModel gradeDefinitionModel)
        {
            gradeDefinitionModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            gradeDefinitionModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {

                var res = await _gradeDefinitionService.CreateGradeDefinitionAsync(gradeDefinitionModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "GradeDefinition has been saved";
                }
                else
                {
                    TempData["error"] = "GradeDefinition has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View("~/Views/QPDetails/GradeDefinition/Create.cshtml", gradeDefinitionModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _gradeDefinitionService.GetGradeDefinitionById(Convert.ToInt32(guid_id));
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/QPDetails/GradeDefinition/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/GradeDefinition/Edit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int GradeId, GradeDefinitionModel gradeDefinitionModel)
        {
            try
            {
                gradeDefinitionModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                gradeDefinitionModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _gradeDefinitionService.GetGradeDefinitionById(GradeId);
                    if (await TryUpdateModelAsync<GradeDefinitionModel>(value))
                    {
                        var res = await _gradeDefinitionService.UpdateGradeDefinitionAsync(gradeDefinitionModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "GradeDefinition has been updated";
                        }
                        else
                        {
                            TempData["error"] = "GradeDefinition has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/QPDetails/GradeDefinition/Edit.cshtml", gradeDefinitionModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _gradeDefinitionService.GetGradeDefinitionById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _gradeDefinitionService.DeleteGradeDefinitionAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "GradeDefinition has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "GradeDefinition has not been deleted";
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
        //public IActionResult VerifyName(string qPTypeName)
        //{

        //    var already = (from data in _qPTypeService.GetAllQPType().Result
        //                   where data.QPTypeName == qPTypeName.Trim()
        //                   select new SelectListItem()
        //                   {
        //                       Text = data.QPTypeName,
        //                       Value = data.QPTypeId.ToString(),
        //                   }).ToList();

        //    if (already.Count > 0)
        //    {
        //        return Json($"{qPTypeName} is already in use.");
        //    }

        //    return Json(true);


        //}
    }
}
