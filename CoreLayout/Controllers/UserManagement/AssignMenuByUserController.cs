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
    [Authorize(Roles = "Administrator,Institute")]
    public class AssignMenuByUserController : Controller
    {
        private readonly ILogger<AssignMenuByUserController> _logger;
        private readonly IAssignMenuByUserService _assignMenuByUserService;
        private readonly IRegistrationService _registrationService;
        private readonly IMenuService _menuService;
        private readonly ICommonService _commonService;
        private readonly IDashboardService _dashboardService;
        private readonly IDataProtector _protector;
        public AssignMenuByUserController(ILogger<AssignMenuByUserController> logger, IAssignMenuByUserService assignMenuByUserService, IRegistrationService registrationService, IMenuService menuService, ICommonService commonService, IDashboardService dashboardService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _assignMenuByUserService = assignMenuByUserService;
            _registrationService = registrationService;
            _menuService = menuService;
            _commonService = commonService;
            _dashboardService = dashboardService;
            _protector = provider.CreateProtector("AssignMenuByUser.AssignMenuByUserController");
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            
            try
            {
                object data = null;
                int userid = (int)HttpContext.Session.GetInt32("UserId");
                int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                if (roleid != 0 && userid != 0)
                {
                    //_ = _commonService.GetDashboardByRoleAndUser(roleid, userid);
                    _ = RefereshMenuAsync();
                }
                if (roleid == 1)
                {
                    data = await _assignMenuByUserService.GetAllMenuAssignByUserAsync();
                }
                else
                {

                    data = (from menuassignedByUser in _assignMenuByUserService.GetAllMenuAssignByUserAsync().Result
                            where menuassignedByUser.UserId == userid
                            select menuassignedByUser).ToList();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
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
                    if (dm.SubMenuName.Equals("*") && dm.MenuName.Equals("*"))
                    {
                        level1.Add(dm);
                    }
                    else if (dm.SubMenuName != "*" && dm.MenuName.Equals("*"))
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
        public async Task<ActionResult> CreateAsync(int id)
        {
            try
            {
                int? RoleId = HttpContext.Session.GetInt32("RoleId");
                int? UserId = HttpContext.Session.GetInt32("UserId");
                AssignMenuByUserModel assignMenuByUserModel = new AssignMenuByUserModel();
                if (RoleId == 1)
                {
                    assignMenuByUserModel.MenuList = await _menuService.GetAllMenuAsync();
                    assignMenuByUserModel.UserList = await _registrationService.GetAllRegistrationAsync();
                }
                else
                {

                    assignMenuByUserModel.MenuList = (from menu in _menuService.GetAllMenuAsync().Result
                            where menu.UserId == UserId
                            select menu).ToList();

                    assignMenuByUserModel.UserList = (from registration in _registrationService.GetAllRegistrationAsync().Result
                                                      where registration.RoleId == RoleId
                                                      select registration).ToList();

                }

                //var guid_id = _protector.Unprotect(id);

                return View(assignMenuByUserModel);
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
        public async Task<IActionResult> Create(AssignMenuByUserModel assignMenuByUserModel)
        {
            try
            {
                assignMenuByUserModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByUserModel.UserList = await _registrationService.GetAllRegistrationAsync();
                assignMenuByUserModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                //assignMenuByUserModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                assignMenuByUserModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var alreadyExit = await _assignMenuByUserService.AlreadyExitAsync(assignMenuByUserModel.MenuId, assignMenuByUserModel.UserId);
                    if (alreadyExit.Count == 0)
                    {
                        var res = await _assignMenuByUserService.CreateMenuAssignByUserAsync(assignMenuByUserModel);
                        if (res.Equals(1))
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id);
                //data.EncryptedId = id;
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
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id);
                data.MenuList = await _menuService.GetAllMenuAsync();
                data.UserList = await _registrationService.GetAllRegistrationAsync();
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
        public async Task<IActionResult> Edit(int MenuPermissionId, AssignMenuByUserModel assignMenuByUserModel)
        {
            try
            {
                assignMenuByUserModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByUserModel.UserList = await _registrationService.GetAllRegistrationAsync();
                assignMenuByUserModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                //assignMenuByRoleModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                //assignMenuByUserModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                assignMenuByUserModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                if (ModelState.IsValid)
                {
                    var value = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(MenuPermissionId);
                    if (await TryUpdateModelAsync<AssignMenuByUserModel>(value))
                    {
                        var res = await _assignMenuByUserService.UpdateMenuAssignByUserAsync(assignMenuByUserModel);
                        if (res.Equals(1))
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
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var value = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(id);
                if (value != null)
                {
                    var res = await _assignMenuByUserService.DeleteMenuAssignByUserAsync(value);

                    if (res.Equals(1))
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
    }
}
