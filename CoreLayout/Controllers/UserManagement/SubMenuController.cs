using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.UserManagement.ParentMenu;
using CoreLayout.Services.UserManagement.SubMenu;
using Microsoft.AspNetCore.Authorization;
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
        public SubMenuController(ILogger<SubMenuController> logger, ICommonService commonService, IParentMenuService parentMenuService, ISubMenuService subMenuService)
        {
            _logger = logger;
            _commonService = commonService;
            _parentMenuService = parentMenuService;
            _subMenuService = subMenuService;
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _subMenuService.GetAllSubMenuAsync());
        }

        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _subMenuService.GetSubMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        public void binddropdowns()
        {
            var ParentMenuList = (from parentmenu in _parentMenuService.GetAllParentMenuAsync().Result
                                  select new SelectListItem()
                                  {
                                      Text = parentmenu.ParentMenuName,
                                      Value = parentmenu.ParentMenuId.ToString(),
                                  }).ToList();
            ParentMenuList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.ParentMenuList = ParentMenuList;
        }
        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            binddropdowns();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(SubMenuModel subMenuModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                subMenuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                //parentMenuModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                subMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                //countryModel.CreatedDate = System.DateTime.Now;


                if (ModelState.IsValid)
                {
                    var res = await _subMenuService.CreateSubMenuAsync(subMenuModel);
                    if (res.ToString().Equals("1"))
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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                binddropdowns();
                return View(await _subMenuService.GetSubMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, SubMenuModel menuModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    menuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    //countryModel.ModifiedDate = System.DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        var dbMenu = await _subMenuService.GetSubMenuByIdAsync(id);
                        if (await TryUpdateModelAsync<SubMenuModel>(dbMenu))
                        {
                            menuModel.SubMenuId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _subMenuService.UpdateSubMenuAsync(dbMenu);
                            if (res.ToString().Equals("1"))
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
                else
                {
                    return RedirectToAction("Login", "Home");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(menuModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbMenu = await _subMenuService.GetSubMenuByIdAsync(id);
                    if (dbMenu != null)
                    {
                        var res = await _subMenuService.DeleteSubMenuAsync(dbMenu);

                        if (res.ToString().Equals("1"))
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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}
