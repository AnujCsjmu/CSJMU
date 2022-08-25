using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.UserManagement.AssignMenuByRole;
using CoreLayout.Services.UserManagement.Menu;
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
    public class AssignMenuByRoleController : Controller
    {
        private readonly ILogger<AssignMenuByRoleController> _logger;
        private readonly IAssignMenuByRoleService _assignMenuByRoleService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly ICommonService _commonService;
        private readonly IDashboardService _dashboardService;
        public AssignMenuByRoleController(ILogger<AssignMenuByRoleController> logger, IAssignMenuByRoleService assignMenuByRoleService, IRoleService roleService, IMenuService menuService, ICommonService commonService, IDashboardService dashboardService)
        {
            _logger = logger;
            _assignMenuByRoleService = assignMenuByRoleService;
            _roleService = roleService;
            _menuService = menuService;
            _commonService = commonService;
            _dashboardService = dashboardService;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {
                //_ = _commonService.GetDashboardByRoleAndUser(roleid, userid);
                _ = RefereshMenuAsync();
            }
            return View(await _assignMenuByRoleService.GetAllMenuAssignByRoleAsync());
        }
        public async Task<ActionResult> RefereshMenuAsync()
        {
            var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {
                //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);
               // IDashboardService _dashboardService1 = _dashboardService;
                List<DashboardModel> alllevels = await _dashboardService.GetDashboardByRoleAndUser(roleid, userid);

                List<DashboardModel> level1 = new List<DashboardModel>();
                List<DashboardModel> level2 = new List<DashboardModel>();
                List<DashboardModel> level3 = new List<DashboardModel>();

                foreach (DashboardModel dm in alllevels)
                {
                    if (dm.Level2.Equals("*") && dm.Level3.Equals("*"))
                    {
                        level1.Add(dm);
                    }
                    else if (dm.Level2 != "*" && dm.Level3.Equals("*"))
                    {
                        level2.Add(dm);
                    }
                    else
                    {
                        level3.Add(dm);
                    }
                }

                HttpContext.Session.SetString("Level1List", JsonConvert.SerializeObject(level1));
                HttpContext.Session.SetString("Level2List", JsonConvert.SerializeObject(level2));
                HttpContext.Session.SetString("Level3List", JsonConvert.SerializeObject(level3));

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public void BindDropDown()
        {

            var MenuList = (from menu in _menuService.GetAllMenuAsync().Result
                            select new SelectListItem()
                            {
                                Text = menu.Level1 + "-" + menu.Level2 + "-" + menu.Level3,
                                Value = menu.MenuID.ToString(),
                            }).ToList();

            MenuList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleID.ToString(),
                            }).ToList();

            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.MenuList = MenuList;
            ViewBag.RoleList = RoleList;
        }
        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            BindDropDown();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(AssignMenuByRoleModel assignMenuByRoleModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                assignMenuByRoleModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                

                if (ModelState.IsValid)
                {
                    var alreadyExit = await _assignMenuByRoleService.AlreadyExitAsync(assignMenuByRoleModel.MenuId, assignMenuByRoleModel.RoleId);
                    if (alreadyExit.Count == 0)
                    {
                        var res = await _assignMenuByRoleService.CreateMenuAssignByRoleAsync(assignMenuByRoleModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Menu Assign By Role has been saved";
                        }
                        else
                        {
                            TempData["error"] = "Menu Assign By Role has not been saved";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Data already exit!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(assignMenuByRoleModel);
            }
            else
            {
                TempData["error"] = "Some thing went wrong!";
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                BindDropDown();
                return View(await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {

                BindDropDown();
                return View(await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, AssignMenuByRoleModel assignMenuByRoleModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    assignMenuByRoleModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    //assignMenuByRoleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id);
                        if (await TryUpdateModelAsync<AssignMenuByRoleModel>(dbRole))
                        {
                            var res = await _assignMenuByRoleService.UpdateMenuAssignByRoleAsync(assignMenuByRoleModel);
                            if (res.Equals(1))
                            {
                                TempData["success"] = "Menu Assign By Role has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Menu Assign By Role has not been updated";
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
            return View(assignMenuByRoleModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _assignMenuByRoleService.DeleteMenuAssignByRoleAsync(dbRole);

                        if (res.Equals(1))
                        {
                            TempData["error"] = "Menu Assign By Role has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Menu Assign By Role has not been deleted";
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
