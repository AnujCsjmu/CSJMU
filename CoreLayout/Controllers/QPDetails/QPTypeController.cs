using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.QPDetails;
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
    public class QPTypeController : Controller
    {
        private readonly ILogger<QPTypeController> _logger;
        private readonly IDataProtector _protector;
        private readonly IQPTypeService _qPTypeService;
        public QPTypeController(ILogger<QPTypeController> logger, IDataProtectionProvider provider, IQPTypeService qPTypeService)
        {
            _logger = logger;
            _qPTypeService = qPTypeService;
            _protector = provider.CreateProtector("QPType.QPTypeController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _qPTypeService.GetAllQPType();
                foreach (var _data in data)
                {
                    var stringId = _data.QPTypeId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.QPTypeId;
                }
                id = id + 1;
                ViewBag.MaxQPTypeId = _protector.Protect(id.ToString());
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
                var data = await _qPTypeService.GetQPTypeById(Convert.ToInt32(guid_id));
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
        public IActionResult Create(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                return View();
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
        public async Task<IActionResult> Create(QPTypeModel qPTypeModel)
        {
            qPTypeModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            qPTypeModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {

                var res = await _qPTypeService.CreateQPTypeAsync(qPTypeModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "QPType has been saved";
                }
                else
                {
                    TempData["error"] = "QPType has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View(qPTypeModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _qPTypeService.GetQPTypeById(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Edit(int QPTypeId, QPTypeModel qPTypeModel)
        {
            try
            {
                qPTypeModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                qPTypeModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                qPTypeModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _qPTypeService.GetQPTypeById(QPTypeId);
                    if (await TryUpdateModelAsync<QPTypeModel>(value))
                    {
                        var res = await _qPTypeService.UpdateQPTypeAsync(qPTypeModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "QPType has been updated";
                        }
                        else
                        {
                            TempData["error"] = "QPType has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(qPTypeModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _qPTypeService.GetQPTypeById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _qPTypeService.DeleteQPTypeAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "QPType has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "QPType has not been deleted";
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
        public IActionResult VerifyName(string qPTypeName)
        {

            var already = (from data in _qPTypeService.GetAllQPType().Result
                           where data.QPTypeName == qPTypeName.Trim()
                           select new SelectListItem()
                           {
                               Text = data.QPTypeName,
                               Value = data.QPTypeId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{qPTypeName} is already in use.");
            }

            return Json(true);


        }
    }
}
