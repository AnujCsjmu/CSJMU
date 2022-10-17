using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignMenuByRole;
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
    public class AssignMenuByRoleController : Controller
    {
        private readonly ILogger<AssignMenuByRoleController> _logger;
        private readonly IAssignMenuByRoleService _assignMenuByRoleService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly ICommonService _commonService;
        private readonly IDashboardService _dashboardService;
        private readonly IDataProtector _protector;
        private readonly IRegistrationService _registrationService;
        public AssignMenuByRoleController(ILogger<AssignMenuByRoleController> logger, IAssignMenuByRoleService assignMenuByRoleService, IRoleService roleService, IMenuService menuService, ICommonService commonService, IDashboardService dashboardService, IDataProtectionProvider provider, IRegistrationService registrationService)
        {
            _logger = logger;
            _assignMenuByRoleService = assignMenuByRoleService;
            _roleService = roleService;
            _menuService = menuService;
            _commonService = commonService;
            _dashboardService = dashboardService;
            _registrationService = registrationService;
            _protector = provider.CreateProtector("AssignMenuByRole.AssignMenuByRoleController");
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _assignMenuByRoleService.GetAllMenuAssignByRoleAsync();
                foreach (var _data in data)
                {
                    var stringId = _data.MenuPermissionId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.MenuPermissionId;
                }
                id = id + 1;
                ViewBag.MaxMenuPermissionId = _protector.Protect(id.ToString());
                //end
               int result=RefereshMenuAsync();
                if(result==1)
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
          
            return RedirectToAction(nameof(Index));
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

        
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(string id)
        {
            try
            {
                AssignMenuByRoleModel assignMenuByRoleModel = new AssignMenuByRoleModel();
                assignMenuByRoleModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByRoleModel.RoleList = await _roleService.GetAllRoleAsync();
                var guid_id = _protector.Unprotect(id);

                return View(assignMenuByRoleModel);
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
        public async Task<IActionResult> Create(AssignMenuByRoleModel assignMenuByRoleModel)
        {
            try
            {
                assignMenuByRoleModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByRoleModel.RoleList = await _roleService.GetAllRoleAsync();
                assignMenuByRoleModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                //assignMenuByRoleModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                assignMenuByRoleModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var alreadyExit = await _assignMenuByRoleService.AlreadyExitAsync(assignMenuByRoleModel.MenuId, assignMenuByRoleModel.RoleId);
                    if (alreadyExit.Count == 0)
                    {
                        var res = await _assignMenuByRoleService.CreateMenuAssignByRoleAsync(assignMenuByRoleModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "MenuAssignByRole has been saved";
                        }
                        else
                        {
                            TempData["error"] = "MenuAssignByRole has not been saved";
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
                var data = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(Convert.ToInt32(guid_id));
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
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(Convert.ToInt32(guid_id));
                data.MenuList = await _menuService.GetAllMenuAsync();
                data.RoleList = await _roleService.GetAllRoleAsync();
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
        public async Task<IActionResult> Edit(int MenuPermissionId, AssignMenuByRoleModel assignMenuByRoleModel)
        {
            try
            {
                assignMenuByRoleModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByRoleModel.RoleList = await _roleService.GetAllRoleAsync();
                assignMenuByRoleModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
               //assignMenuByRoleModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                //assignMenuByRoleModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                assignMenuByRoleModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                if (ModelState.IsValid)
                {
                    var value = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(MenuPermissionId);
                    if (await TryUpdateModelAsync<AssignMenuByRoleModel>(value))
                    {
                        var res = await _assignMenuByRoleService.UpdateMenuAssignByRoleAsync(assignMenuByRoleModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "MenuAssignByRole has been updated";
                        }
                        else
                        {
                            TempData["error"] = "MenuAssignByRole has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _assignMenuByRoleService.DeleteMenuAssignByRoleAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "MenuAssignByRole has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "MenuAssignByRole has not been deleted";
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

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyAssignMenuByRole(int menuid, int roleid)
        //{
            
        //        var already = (from assignmenubyrole in _assignMenuByRoleService.GetAllMenuAssignByRoleAsync().Result
        //                       where assignmenubyrole.MenuId == menuid && assignmenubyrole.RoleId==roleid
        //                       select new SelectListItem()
        //                       {
        //                           Text = assignmenubyrole.RoleName,
        //                           Value = assignmenubyrole.RoleId.ToString(),
        //                       }).ToList();

        //        if (already.Count > 0)
        //        {
        //            return Json($"{menuid} is already assign to user {roleid}.");
        //        }

        //        return Json(true);
           

        //}
    }
}
