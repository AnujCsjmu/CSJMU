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
            var parentMenus = await _parentMenuService.GetAllParentMenuAsync();
            foreach (var _parentMenus in parentMenus)
            {
                var id = _parentMenus.ParentMenuId.ToString();
                _parentMenus.EncryptedId = _protector.Protect(id);
            }
            return View(parentMenus);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var guid_id = _protector.Unprotect(id);
                var result = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(ParentMenuModel parentMenuModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                parentMenuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                parentMenuModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                if (ModelState.IsValid)
                {
                    //get max sort order number
                    //int getmaxShortNumber=_commonController.assignRoleAlreadyExits()

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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var guid_id = _protector.Unprotect(id);
                var parentMenu = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));

                return View(parentMenu);
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id, ParentMenuModel parentMenuModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    parentMenuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                    parentMenuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    if (ModelState.IsValid)
                    {
                        var guid_id = _protector.Unprotect(id);
                        var dbMenu = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
                        if (await TryUpdateModelAsync<ParentMenuModel>(dbMenu))
                        {
                            parentMenuModel.ParentMenuId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _parentMenuService.UpdateParentMenuAsync(dbMenu);
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
                else
                {
                    return RedirectToAction("Login", "Home");

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

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var guid_id = _protector.Unprotect(id);
                    var dbMenu = await _parentMenuService.GetParentMenuByIdAsync(Convert.ToInt32(guid_id));
                    if (dbMenu != null)
                    {
                        var res = await _parentMenuService.DeleteParentMenuAsync(dbMenu);

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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}
