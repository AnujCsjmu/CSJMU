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
        public MenuController(ILogger<MenuController> logger, IMenuService menuService, IRoleService roleService, ICommonService commonService,IDashboardService dashboardService, IParentMenuService parentMenuService, ISubMenuService subMenuService)
        {
            _logger = logger;
            _menuService = menuService;
            _roleService = roleService;
            _commonService = commonService;
            _dashboardService = dashboardService;
            _parentMenuService = parentMenuService;
            _subMenuService = subMenuService;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            //int userid = (int)HttpContext.Session.GetInt32("UserId");
            //int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            //if (roleid != 0 && userid != 0)
            //{
            //    _ = RefereshMenuAsync();
            //}
            return View(await _menuService.GetAllMenuAsync());
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
        //            if (dm.Level2.Equals("*") && dm.Level3.Equals("*"))
        //            {
        //                level1.Add(dm);
        //            }
        //            else if (dm.Level2 != "*" && dm.Level3.Equals("*"))
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
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _menuService.GetMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
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


            //var SubMenuList = (from submenu in _subMenuService.GetAllSubMenuAsync().Result
            //                 select new SelectListItem()
            //                 {
            //                     Text = submenu.SubMenuName,
            //                     Value = submenu.SubMenuId.ToString(),
            //                 }).ToList();

            //SubMenuList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = string.Empty
            //});

            ViewBag.ParentMenuList = ParentMenuList;
            //ViewBag.SubMenuList = SubMenuList;
        }

        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            binddropdowns();
            List<RoleModel> roleList = _roleService.GetAllRoleAsync().Result.ToList(); ;
            ViewBag.Listofrole = roleList.Select(l => l.RoleName).ToList();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(MenuModel menuModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                menuModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                menuModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                menuModel.UserRoleId = (int)HttpContext.Session.GetInt32("UserId");
                //countryModel.CreatedDate = System.DateTime.Now;


                if (ModelState.IsValid)
                {
                    var res = await _menuService.CreateMenuAsync(menuModel);
                    if (res.ToString().Equals("1"))
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
                List<RoleModel> roleList = _roleService.GetAllRoleAsync().Result.ToList(); ;
                ViewBag.Listofrole = roleList.Select(l => l.RoleName).ToList();
                return View(await _menuService.GetMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, MenuModel menuModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    menuModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    //countryModel.ModifiedDate = System.DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        var dbMenu = await _menuService.GetMenuByIdAsync(id);
                        if (await TryUpdateModelAsync<MenuModel>(dbMenu))
                        {
                            menuModel.MenuID = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _menuService.UpdateMenuAsync(dbMenu);
                            if (res.ToString().Equals("1"))
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
                    var dbMenu = await _menuService.GetMenuByIdAsync(id);
                    if (dbMenu != null)
                    {
                        var res = await _menuService.DeleteMenuAsync(dbMenu);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "Menu has been deleted";
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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}
