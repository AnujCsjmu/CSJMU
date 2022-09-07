using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.UserManagement.ParentMenu;
using CoreLayout.Services.UserManagement.SubMenu;
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

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SubMenuController : Controller
    {
        private readonly ILogger<SubMenuController> _logger;
        private readonly IParentMenuService _parentMenuService;
        private readonly ISubMenuService _subMenuService;
        private readonly ICommonService _commonService;
        private readonly IDataProtector _protector;
        public SubMenuController(ILogger<SubMenuController> logger, ICommonService commonService, IParentMenuService parentMenuService, ISubMenuService subMenuService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _commonService = commonService;
            _parentMenuService = parentMenuService;
            _subMenuService = subMenuService;
            _protector = provider.CreateProtector("SubMenu.SubMenuController");
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var subMenus = await _subMenuService.GetAllSubMenuAsync();
                foreach (var _subMenus in subMenus)
                {
                    var id = _subMenus.SubMenuId.ToString();
                    _subMenus.EncryptedId = _protector.Protect(id);
                }
                //end
                //start generate maxid for create button
                int maxsubmenuid = 0;
                foreach (var _subMenus in subMenus)
                {
                    maxsubmenuid = _subMenus.SubMenuId;
                }
                maxsubmenuid = maxsubmenuid + 1;
                ViewBag.MaxSubMenuId = _protector.Protect(maxsubmenuid.ToString());
                return View(subMenus);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _subMenuService.GetSubMenuByIdAsync(Convert.ToInt32(guid_id));
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
        //public void binddropdowns()
        //{
        //    var ParentMenuList = (from parentmenu in _parentMenuService.GetAllParentMenuAsync().Result
        //                          select new SelectListItem()
        //                          {
        //                              Text = parentmenu.ParentMenuName,
        //                              Value = parentmenu.ParentMenuId.ToString(),
        //                          }).ToList();
        //    ParentMenuList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });

        //    ViewBag.ParentMenuList = ParentMenuList;
        //}

        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(string id)
        {
            try
            {
                SubMenuModel subMenuModel = new SubMenuModel();
                subMenuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                var guid_id = _protector.Unprotect(id);

                return View(subMenuModel);
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
        public async Task<IActionResult> Create(SubMenuModel subMenuModel)
        {
            try
            {
                subMenuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                subMenuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                subMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var res = await _subMenuService.CreateSubMenuAsync(subMenuModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Sub Menu has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Sub Menu has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(subMenuModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _subMenuService.GetSubMenuByIdAsync(Convert.ToInt32(guid_id));
                data.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
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
        public async Task<IActionResult> Edit(int SubMenuId, SubMenuModel subMenuModel)
        {
            try
            {
                subMenuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                subMenuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                subMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                subMenuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                if (ModelState.IsValid)
                {
                    var value = await _subMenuService.GetSubMenuByIdAsync(SubMenuId);
                    if (await TryUpdateModelAsync<SubMenuModel>(value))
                    {
                        var res = await _subMenuService.UpdateSubMenuAsync(subMenuModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Sub Menu has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Sub Menu has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(subMenuModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _subMenuService.GetSubMenuByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _subMenuService.DeleteSubMenuAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "Sub Menu has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Sub Menu has not been deleted";
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
        public IActionResult VerifySubMenuName(string subMenuName)
        {
            var already = (from submenu in _subMenuService.GetAllSubMenuAsync().Result
                           where submenu.SubMenuName == subMenuName.Trim()
                           select new SelectListItem()
                           {
                               Text = submenu.SubMenuName,
                               Value = submenu.SubMenuId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{subMenuName} is already in use.");
            }

            return Json(true);

        }
    }
}
