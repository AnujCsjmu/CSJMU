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
    [Authorize(Roles = "Administrator")]
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
                int userid = (int)HttpContext.Session.GetInt32("UserId");
                int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                if (roleid != 0 && userid != 0)
                {
                    //_ = _commonService.GetDashboardByRoleAndUser(roleid, userid);
                    _ = RefereshMenuAsync();
                }
                return View(await _assignMenuByRoleService.GetAllMenuAssignByRoleAsync());
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
               // IDashboardService _dashboardService1 = _dashboardService;
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

        //public void BindDropDown()
        //{

        //    var MenuList = (from menu in _menuService.GetAllMenuAsync().Result
        //                    select new SelectListItem()
        //                    {
        //                        Text = menu.ParentMenuName + "-" + menu.SubMenuName + "-" + menu.MenuName,
        //                        Value = menu.MenuID.ToString(),
        //                    }).ToList();

        //    MenuList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });

        //    var RoleList = (from role in _roleService.GetAllRoleAsync().Result
        //                    select new SelectListItem()
        //                    {
        //                        Text = role.RoleName,
        //                        Value = role.RoleID.ToString(),
        //                    }).ToList();

        //    RoleList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    ViewBag.MenuList = MenuList;
        //    ViewBag.RoleList = RoleList;
        //}
        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(int id)
        {
            try
            {
                AssignMenuByRoleModel assignMenuByRoleModel = new AssignMenuByRoleModel();
                assignMenuByRoleModel.MenuList = await _menuService.GetAllMenuAsync();
                assignMenuByRoleModel.RoleList = await _roleService.GetAllRoleAsync();
                //var guid_id = _protector.Unprotect(id);

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
                var data = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id);
               // data.EncryptedId = id;
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var data = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id);
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
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var value = await _assignMenuByRoleService.GetMenuAssignByRoleByIdAsync(id);
                if (value != null)
                {
                    var res = await _assignMenuByRoleService.DeleteMenuAssignByRoleAsync(value);

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
