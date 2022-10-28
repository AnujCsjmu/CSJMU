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
            //var data = new object();
            var data = (dynamic)null;
            int? RoleId = HttpContext.Session.GetInt32("RoleId");
            int? UserId = HttpContext.Session.GetInt32("UserId");
            //start encrypt id for update,delete & details
            try
            {
                if (RoleId == 1)//1 is admin
                {
                    data = await _assignMenuByUserService.GetAllMenuAssignByUserAsync();
                }
                else if (RoleId == 4)//4 is institute
                {
                    data = (from menuassignedbyuser in _assignMenuByUserService.GetAllMenuAssignByUserAsync().Result
                            where menuassignedbyuser.CreatedBy == UserId
                            select menuassignedbyuser).ToList();
                }

                //end
                foreach (var s in data)
                {
                    string stringId = s.MenuPermissionId.ToString();
                    s.EncryptedId = _protector.Protect(stringId);
                }
                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.MenuPermissionId;
                }
                id = id + 1;
                ViewBag.MaxMenuPermissionByUser = _protector.Protect(id.ToString());
                //end
                int result = RefereshMenuAsync();
                if (result == 1)
                {
                    return View(data);
                }
                else
                {
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(data);
        }
        public int RefereshMenuAsync()
        {
            int result = 0;
            var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {
                List<DashboardModel> alllevels = _dashboardService.GetDashboardByRoleAndUser(roleid, userid).Result;
                HttpContext.Session.SetString("AllLevelList", JsonConvert.SerializeObject(alllevels));
                result = 1;
            }
            return result;
        }
        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(string id)
        {
            try
            {
                AssignMenuByUserModel assignMenuByUserModel = new AssignMenuByUserModel();
                int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (RoleId == 1)//1 is admin
                {
                    assignMenuByUserModel.MenuList = await _menuService.GetAllMenuAsync();
                    assignMenuByUserModel.UserList = await _registrationService.GetAllRegistrationAsync();
                }
                else if (RoleId == 4)//4 is institute
                {
                    assignMenuByUserModel.MenuList =await _menuService.GetMenuByRoleAndUserForInstitute(RoleId, UserId);
                    assignMenuByUserModel.UserList = (from s in await _registrationService.GetAllRegistrationAsync()
                                                     where s.CreatedBy == UserId
                                                     select s).ToList();
                }
                var guid_id = _protector.Unprotect(id);

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
                            TempData["success"] = "MenuAssignByUser has been saved";
                        }
                        else
                        {
                            TempData["error"] = "MenuAssignByUser has not been saved";
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
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(Convert.ToInt32(guid_id));
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
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(Convert.ToInt32(guid_id));
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
                            TempData["success"] = "MenuAssignByUser has been updated";
                        }
                        else
                        {
                            TempData["error"] = "MenuAssignByUser has not been updated";
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _assignMenuByUserService.GetMenuAssignByUserByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _assignMenuByUserService.DeleteMenuAssignByUserAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "MenuAssignByUser has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "MenuAssignByUser has not been deleted";
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
