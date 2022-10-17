using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignRole;
using CoreLayout.Services.UserManagement.RoleToRoleMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.UserManagement
{
    [Authorize(Roles = "Administrator")]
    public class RoleToRoleMappingController : Controller
    {
        private readonly ILogger<RoleToRoleMappingController> _logger;
        private readonly IAssignRoleService _assignRoleService;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private readonly IRoleToRoleMappingService _roleToRoleMappingService;
        private readonly IDataProtector _protector;
        public RoleToRoleMappingController(ILogger<RoleToRoleMappingController> logger, IAssignRoleService assignRoleService, IRegistrationService registrationService, IRoleService roleService, CommonController commonController, IRoleToRoleMappingService roleToRoleMappingService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _assignRoleService = assignRoleService;
            _registrationService = registrationService;
            _roleService = roleService;
            _commonController = commonController;
            _roleToRoleMappingService = roleToRoleMappingService;
            _protector = provider.CreateProtector("RoleToRoleMapping.RoleToRoleMappingController");
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                
                List<RoleToRoleMappingModel> list2 = new List<RoleToRoleMappingModel>();
                

                var fromRoleList = (from reg in await _roleToRoleMappingService.GetAllRoleToRoleMappingAsync()
                            select new { reg.FromRoleId, reg.CreatedBy, reg.FromRoleName }).Distinct().ToList();
                foreach (var _data in fromRoleList)
                {
                    List<RoleToRoleMappingModel> list1 = new List<RoleToRoleMappingModel>();
                    RoleToRoleMappingModel roleToRoleMappingModel = new RoleToRoleMappingModel();
                    var toRoleList = (from reg1 in await _roleToRoleMappingService.GetAllRoleToRoleMappingAsync()
                                 where reg1.FromRoleId == _data.FromRoleId
                                 select reg1).Distinct().ToList();
                    foreach (var _data1 in toRoleList)
                    {
                        
                        if (_data.FromRoleId == _data1.FromRoleId)
                        {
                            roleToRoleMappingModel.RoleMappingId = _data1.RoleMappingId;
                            roleToRoleMappingModel.FromRoleId = _data1.FromRoleId;
                            roleToRoleMappingModel.CreatedBy = _data1.CreatedBy;
                            roleToRoleMappingModel.FromRoleName = _data1.FromRoleName;
                            roleToRoleMappingModel.ToRoleId = _data1.ToRoleId;
                            roleToRoleMappingModel.ToRoleName = _data1.ToRoleName;
                            roleToRoleMappingModel.CreatedDate = _data1.CreatedDate;

                            list1.Add(roleToRoleMappingModel);
                        }
                    }

                    roleToRoleMappingModel.ToRolelListForGrid = toRoleList;
                    list2.Add(roleToRoleMappingModel);
                }
                //ViewBag.list = roleToRoleMappingModel;

                //start encrypt id for update, delete & details
                //var menu = await _roleToRoleMappingService.GetAllRoleToRoleMappingAsync();
                //_ = RefereshMenuAsync();
                foreach (var _menu in await _roleToRoleMappingService.GetAllRoleToRoleMappingAsync())
                {
                    var ids = _menu.RoleMappingId.ToString();
                    _menu.EncryptedId = _protector.Protect(ids);
                }
                //end
                //start generate maxid for create button
                int id = 0;
                foreach (var _menu in await _roleToRoleMappingService.GetAllRoleToRoleMappingAsync())
                {
                    id = _menu.RoleMappingId;
                }
                id = id + 1;
                ViewBag.MaxRoleMappingId = _protector.Protect(id.ToString());

                return View(list2);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(string id)
        {
            try
            {
                RoleToRoleMappingModel roleToRoleMappingModel = new RoleToRoleMappingModel();
                roleToRoleMappingModel.FromRoleList = await _roleService.GetAllRoleAsync();
                ViewBag.ToRoleList = await _roleService.GetAllRoleAsync();
                var guid_id = _protector.Unprotect(id);

                return View(roleToRoleMappingModel);
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
        public async Task<IActionResult> Create(RoleToRoleMappingModel roleToRoleMappingModel)
        {
            try
            {
                roleToRoleMappingModel.FromRoleList = await _roleService.GetAllRoleAsync();
                ViewBag.ToRoleList = await _roleService.GetAllRoleAsync();
                roleToRoleMappingModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                roleToRoleMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
                {
                    var res = await _roleToRoleMappingService.CreateRoleToRoleMappingAsync(roleToRoleMappingModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "RoleToRoleMapping has been saved";
                    }
                    else
                    {
                        TempData["error"] = "RoleToRoleMapping has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(roleToRoleMappingModel);
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
                var data = await _roleToRoleMappingService.GetRoleToRoleMappingByIdAsync(Convert.ToInt32(guid_id));
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
                var data = await _roleToRoleMappingService.GetRoleToRoleMappingByIdAsync(Convert.ToInt32(guid_id));
                data.FromRoleList = await _roleService.GetAllRoleAsync();
                ViewBag.ToRoleList = await _roleService.GetAllRoleAsync();
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
        public async Task<IActionResult> Edit(int RoleMappingId, RoleToRoleMappingModel roleToRoleMappingModel)
        {

            try
            {
                roleToRoleMappingModel.FromRoleList = await _roleService.GetAllRoleAsync();
                ViewBag.ToRoleList = await _roleService.GetAllRoleAsync();
                roleToRoleMappingModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                roleToRoleMappingModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");

                if (ModelState.IsValid)
                {
                    var value = await _roleToRoleMappingService.GetRoleToRoleMappingByIdAsync(RoleMappingId);
                    if (await TryUpdateModelAsync<RoleToRoleMappingModel>(value))
                    {
                        var res = await _roleToRoleMappingService.UpdateRoleToRoleMappingAsync(roleToRoleMappingModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "RoleToRoleMapping has been updated";
                        }
                        else
                        {
                            TempData["error"] = "RoleToRoleMapping has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(roleToRoleMappingModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _roleToRoleMappingService.GetRoleToRoleMappingByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _roleToRoleMappingService.DeleteRoleToRoleMappingAsync(value);

                    if (res.Equals(1))
                    {
                        TempData["success"] = "RoleToRoleMapping has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "RoleToRoleMapping has not been deleted";
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
