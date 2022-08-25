using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignMenuByUser;
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
    public class AssignMenuByUserController : Controller
    {
        private readonly ILogger<AssignMenuByUserController> _logger;
        private readonly IAssignMenuByUserService _assignMenuByUserService;
        private readonly IRegistrationService _registrationService;
        private readonly IMenuService _menuService;
        private readonly ICommonService _commonService;
        private readonly IDashboardService _dashboardService;

        public AssignMenuByUserController(ILogger<AssignMenuByUserController> logger, IAssignMenuByUserService assignMenuByUserService, IRegistrationService registrationService, IMenuService menuService, ICommonService commonService, IDashboardService dashboardService)
        {
            _logger = logger;
            _assignMenuByUserService = assignMenuByUserService;
            _registrationService = registrationService;
            _menuService = menuService;
            _commonService = commonService;
            _dashboardService = dashboardService;
        }
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
            return View(await _assignMenuByUserService.GetAllMenuAssignByUserAsync());
        }
        public async Task<ActionResult> RefereshMenuAsync()
        {
            var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {
                //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);
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
        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
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

            var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                            select new SelectListItem()
                            {
                                Text = user.UserName,
                                Value = user.UserID.ToString(),
                            }).ToList();

            UserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.MenuList = MenuList;
            ViewBag.UserList = UserList;
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(AssignMenuByUserModel assignMenuByUserModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                assignMenuByUserModel.CreatedBy = HttpContext.Session.GetInt32("UserId");

                if (ModelState.IsValid)
                {
                    var alreadyExit = await _assignMenuByUserService.AlreadyExitAsync(assignMenuByUserModel.MenuId, assignMenuByUserModel.UserId);
                    if (alreadyExit.Count == 0)
                    {
                        var res = await _assignMenuByUserService.CreateMenuAssignByUserAsync(assignMenuByUserModel);
                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "Menu Assign By User has been saved";
                        }
                        else
                        {
                            TempData["error"] = "Menu Assign By User has not been saved";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Data already exit!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(assignMenuByUserModel);
            }
            else
            {
                TempData["error"] = "Some thing went wrong!";
                return RedirectToAction(nameof(Index));

            }
        }
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
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

                var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                                select new SelectListItem()
                                {
                                    Text = user.UserName,
                                    Value = user.UserID.ToString(),
                                }).ToList();

                UserList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.MenuList = MenuList;
                ViewBag.UserList = UserList;
                return View(await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id));
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

                var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                                select new SelectListItem()
                                {
                                    Text = user.UserName,
                                    Value = user.UserID.ToString(),
                                }).ToList();

                UserList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.MenuList = MenuList;
                ViewBag.UserList = UserList;
                return View(await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, AssignMenuByUserModel assignMenuByUserModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    assignMenuByUserModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    //assignMenuByRoleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id);
                        if (await TryUpdateModelAsync<AssignMenuByUserModel>(dbRole))
                        {
                            assignMenuByUserModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                            var res = await _assignMenuByUserService.UpdateMenuAssignByUserAsync(assignMenuByUserModel);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Menu Assign By User has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Menu Assign By User has not been updated";
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
            return View(assignMenuByUserModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _assignMenuByUserService.DeleteMenuAssignByUserAsync(dbRole);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "Menu Assign By User has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Menu Assign By User has not been deleted";
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
