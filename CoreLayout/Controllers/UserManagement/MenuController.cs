using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.UserManagement.Menu;
using CoreLayout.Services.UserManagement.ParentMenu;
using CoreLayout.Services.UserManagement.SubMenu;
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
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;
        private readonly IParentMenuService _parentMenuService;
        private readonly ISubMenuService _subMenuService;
        private readonly IDataProtector _protector;
        public MenuController(ILogger<MenuController> logger, IMenuService menuService, IRoleService roleService, ICommonService commonService, IDashboardService dashboardService, IParentMenuService parentMenuService, ISubMenuService subMenuService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _menuService = menuService;
            _roleService = roleService;
            _commonService = commonService;
            _dashboardService = dashboardService;
            _parentMenuService = parentMenuService;
            _subMenuService = subMenuService;
            _protector = provider.CreateProtector("Menu.MenuController");
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var menu = await _menuService.GetAllMenuAsync();
               // _ = RefereshMenuAsync();
                foreach (var _menu in menu)
                {
                    var id = _menu.MenuID.ToString();
                    _menu.EncryptedId = _protector.Protect(id);
                }
                //end
                //start generate maxid for create button
                int maxmenuid = 0;
                foreach (var _menu in menu)
                {
                    maxmenuid = _menu.MenuID;
                }
                maxmenuid = maxmenuid + 1;
                ViewBag.MaxMenuId = _protector.Protect(maxmenuid.ToString());
                return View(menu);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }
        //public async Task<ActionResult> RefereshMenuAsync()
        //{
        //    var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
        //    int userid = (int)HttpContext.Session.GetInt32("UserId");
        //    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
        //    if (roleid != 0 && userid != 0)
        //    {
        //        //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);
        //        //IDashboardService _dashboardService1 = _dashboardService;
        //        List<DashboardModel> alllevels = await _dashboardService.GetDashboardByRoleAndUser(roleid, userid);

        //        List<DashboardModel> level1 = new List<DashboardModel>();
        //        List<DashboardModel> level2 = new List<DashboardModel>();
        //        List<DashboardModel> level3 = new List<DashboardModel>();

        //        foreach (DashboardModel dm in alllevels)
        //        {
        //            if (dm.SubMenuName.Equals("*") && dm.MenuName.Equals("*"))
        //            {
        //                level1.Add(dm);
        //            }
        //            else if (dm.SubMenuName != "*" && dm.MenuName.Equals("*"))
        //            {
        //                level2.Add(dm);
        //            }
        //            else
        //            {
        //                level3.Add(dm);
        //            }
        //        }

        //        HttpContext.Session.SetString("Level1List", JsonConvert.SerializeObject(level1));
        //        HttpContext.Session.SetString("Level2List", JsonConvert.SerializeObject(level2));
        //        HttpContext.Session.SetString("Level3List", JsonConvert.SerializeObject(level3));

        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _menuService.GetMenuByIdAsync(Convert.ToInt32(guid_id));
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

        public JsonResult GetSubMenu(int ParentMenuId)
        {
            var SubMenuList = (from submenu in _subMenuService.GetAllSubMenuAsync().Result
                               where submenu.ParentMenuId == ParentMenuId
                               select new SelectListItem()
                               {
                                   Text = submenu.SubMenuName,
                                   Value = submenu.SubMenuId.ToString(),
                               }).ToList();
            SubMenuList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(SubMenuList);
        }
        //public void binddropdowns()
        //{
        //    var ParentMenuList = (from parentmenu in _parentMenuService.GetAllParentMenuAsync().Result
        //                       select new SelectListItem()
        //                       {
        //                           Text = parentmenu.ParentMenuName,
        //                           Value = parentmenu.ParentMenuId.ToString(),
        //                       }).ToList();

        //    ParentMenuList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });


        //    //var SubMenuList = (from submenu in _subMenuService.GetAllSubMenuAsync().Result
        //    //                 select new SelectListItem()
        //    //                 {
        //    //                     Text = submenu.SubMenuName,
        //    //                     Value = submenu.SubMenuId.ToString(),
        //    //                 }).ToList();

        //    //SubMenuList.Insert(0, new SelectListItem()
        //    //{
        //    //    Text = "----Select----",
        //    //    Value = string.Empty
        //    //});

        //    ViewBag.ParentMenuList = ParentMenuList;
        //    //ViewBag.SubMenuList = SubMenuList;
        //}

        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(string id)
        {
            try
            {
                MenuModel menuModel = new MenuModel();
                menuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                menuModel.SubMenuList = await _subMenuService.GetAllSubMenuAsync();
                var guid_id = _protector.Unprotect(id);

                return View(menuModel);
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
        public async Task<IActionResult> Create(MenuModel menuModel)
        {
            try
            {
                menuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                menuModel.SubMenuList = await _subMenuService.GetAllSubMenuAsync();
                menuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                menuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                menuModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                menuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var res = await _menuService.CreateMenuAsync(menuModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Menu has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Menu has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(menuModel);
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
                var data = await _menuService.GetMenuByIdAsync(Convert.ToInt32(guid_id));
                data.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                data.SubMenuList = await _subMenuService.GetAllSubMenuAsync();
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
        public async Task<IActionResult> Edit(int MenuId, MenuModel menuModel)
        {

            try
            {
                menuModel.ParentMenuList = await _parentMenuService.GetAllParentMenuAsync();
                menuModel.SubMenuList = await _subMenuService.GetAllSubMenuAsync();
                menuModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                menuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                menuModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");

                if (ModelState.IsValid)
                {
                    var value = await _menuService.GetMenuByIdAsync(MenuId);
                    if (await TryUpdateModelAsync<MenuModel>(value))
                    {
                        var res = await _menuService.UpdateMenuAsync(menuModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Menu has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Menu has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
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
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _menuService.GetMenuByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _menuService.DeleteMenuAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["success"] = "Menu has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Menu has not been deleted";
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
        public IActionResult VerifyMenuName(string MenuName)
        {
            if (MenuName != "*")
            {
                var already = (from menu in _menuService.GetAllMenuAsync().Result
                               where menu.MenuName == MenuName.Trim()
                               select new SelectListItem()
                               {
                                   Text = menu.MenuName,
                                   Value = menu.MenuID.ToString(),
                               }).ToList();

                if (already.Count > 0)
                {
                    return Json($"{MenuName} is already in use.");
                }

                return Json(true);
            }
            else
            {
                return Json(false);
            }

        }
    }
}
