using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.UserManagement.ParentMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ParentMenuController : Controller
    {
        private readonly ILogger<ParentMenuController> _logger;
        private readonly IParentMenuService _parentMenuService;
        private readonly ICommonService _commonService;
        private readonly CommonController _commonController;
        private readonly IDataProtector _protector;
        public ParentMenuController(ILogger<ParentMenuController> logger, ICommonService commonService, IParentMenuService parentMenuService, CommonController commonController, IDataProtectionProvider provider)
        {
            _logger = logger;
            _commonService = commonService;
            _parentMenuService = parentMenuService;
            _commonController = commonController;
            _protector = provider.CreateProtector("ParentMenu.ParentMenuController");
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var parentMenus =await _parentMenuService.GetAllParentMenuAsync();
                foreach (var _parentMenus in parentMenus)
                {
                    var id = _parentMenus.ParentMenuId.ToString();
                    _parentMenus.EncryptedId = _protector.Protect(id);
                }
                //end
                //start generate maxid for create button
                int maxparentmenuid = 0;
                foreach (var _parentMenus in parentMenus)
                {
                    maxparentmenuid = _parentMenus.ParentMenuId;
                }
                maxparentmenuid = maxparentmenuid + 1;
                ViewBag.MaxParentMenuId = _protector.Protect(maxparentmenuid.ToString());
                return View(parentMenus);
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
                var data = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
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
        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create(string id)
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
        public async Task<IActionResult> Create(ParentMenuModel parentMenuModel)
        {
            try
            {
                parentMenuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                parentMenuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var res = await _parentMenuService.CreateParentMenuAsync(parentMenuModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Parent Menu has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Parent Menu has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(parentMenuModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Edit(int ParentMenuId, ParentMenuModel parentMenuModel)
        {
            try
            {
                parentMenuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                parentMenuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var value = await _parentMenuService.GetParentMenuByIdAsync(ParentMenuId);
                    if (await TryUpdateModelAsync<ParentMenuModel>(value))
                    {
                        var res = await _parentMenuService.UpdateParentMenuAsync(parentMenuModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Parent Menu has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Parent Menu has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(parentMenuModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _parentMenuService.DeleteParentMenuAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "Parent Menu has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Parent Menu has not been deleted";
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
        public IActionResult VerifyParentMenuName(string parentMenuName)
        {
            var already = (from parentmenu in _parentMenuService.GetAllParentMenuAsync().Result
                           where parentmenu.ParentMenuName == parentMenuName.Trim()
                           select new SelectListItem()
                           {
                               Text = parentmenu.ParentMenuName,
                               Value = parentmenu.ParentMenuId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{parentMenuName} is already in use.");
            }

            return Json(true);

        }
    }
}
