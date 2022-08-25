using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AssignRoleController : Controller
    {
        private readonly ILogger<AssignRoleController> _logger;
        private readonly IAssignRoleService _assignRoleService;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;

        public AssignRoleController(ILogger<AssignRoleController> logger, IAssignRoleService assignRoleService, IRegistrationService registrationService, IRoleService roleService, CommonController commonController)
        {
            _logger = logger;
            _assignRoleService = assignRoleService;
            _registrationService = registrationService;
            _roleService = roleService;
            _commonController = commonController;
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _assignRoleService.GetAllRoleAssignAsync());
        }
        public void BindDropdown()
        {
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
            ViewBag.UserList = UserList;
            ViewBag.RoleList = RoleList;
        }

        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            BindDropdown();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(RegistrationRoleMapping registrationRoleMapping)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                int assignRoleAlreadyExit = _commonController.assignRoleAlreadyExits(registrationRoleMapping.RoleUserId, registrationRoleMapping.RoleId);
                if (assignRoleAlreadyExit == 0)
                {
                    registrationRoleMapping.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    registrationRoleMapping.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    registrationRoleMapping.IPAddress = HttpContext.Session.GetString("IPAddress");
                    if (ModelState.IsValid)
                    {
                        var res = await _assignRoleService.CreateRoleAssignAsync(registrationRoleMapping);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Role Assign has been saved";
                        }
                        else
                        {
                            TempData["error"] = "Role Assign has not been saved";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    return View(registrationRoleMapping);
                }
                else
                {
                    TempData["error"] = "ALready role assign to this user";
                    return RedirectToAction(nameof(Index));
                }
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
                BindDropdown();
                return View(await _assignRoleService.GetRoleAssignByIdAsync(id));
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
                BindDropdown();
                return View(await _assignRoleService.GetRoleAssignByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, RegistrationRoleMapping registrationRoleMapping)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    int assignRoleAlreadyExit = _commonController.assignRoleAlreadyExits(registrationRoleMapping.RoleUserId, registrationRoleMapping.RoleId);
                    if (assignRoleAlreadyExit == 0)
                    {
                        registrationRoleMapping.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                        registrationRoleMapping.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                        registrationRoleMapping.IPAddress = HttpContext.Session.GetString("IPAddress");
                        if (ModelState.IsValid)
                        {
                            var dbRole = await _assignRoleService.GetRoleAssignByIdAsync(id);
                            if (await TryUpdateModelAsync<RegistrationRoleMapping>(dbRole))
                            {
                                registrationRoleMapping.UserId = (int)HttpContext.Session.GetInt32("UserId");
                                var res = await _assignRoleService.UpdateRoleAssignAsync(registrationRoleMapping);
                                if (res.ToString().Equals("1"))
                                {
                                    TempData["success"] = "Role Assign has been updated";
                                }
                                else
                                {
                                    TempData["error"] = "Role Assign has not been updated";
                                }
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    else
                    {
                        TempData["error"] = "ALready role assign to this user";
                        return RedirectToAction(nameof(Index));
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
            return View(registrationRoleMapping);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _assignRoleService.GetRoleAssignByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _assignRoleService.DeleteRoleAssignAsync(dbRole);

                        if (res.Equals(1))
                        {
                            TempData["error"] = "Role Assign has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Role Assign has not been deleted";
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

