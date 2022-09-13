using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        public RoleController(ILogger<RoleController> logger, IRoleService roleMService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _roleService = roleMService;
            _protector = provider.CreateProtector("Role.RoleController");
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var data = await _roleService.GetAllRoleAsync();
                // _ = RefereshMenuAsync();
                foreach (var _data in data)
                {
                    var id = _data.RoleID.ToString();
                    _data.EncryptedId = _protector.Protect(id);
                }
                //end
                //start generate maxid for create button
                int maxid = 0;
                foreach (var _data in data)
                {
                    maxid = _data.RoleID;
                }
                maxid = maxid + 1;
                ViewBag.MaxRoleId = _protector.Protect(maxid.ToString());
                return View(data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _roleService.GetRoleByIdAsync(Convert.ToInt32(guid_id));
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


        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                return View();
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
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            try
            {
               
                    roleModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    roleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    //countryModel.CreatedDate = System.DateTime.Now;


                    if (ModelState.IsValid)
                    {
                        var res = await _roleService.CreateRoleAsync(roleModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Role has been saved";
                        }
                        else
                        {
                            TempData["error"] = "Role has not been saved";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    return View(roleModel);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _roleService.GetRoleByIdAsync(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Edit(int Roleid, RoleModel roleModel)
        {
            try
            {
               
                    roleModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _roleService.GetRoleByIdAsync(Roleid);
                        if (await TryUpdateModelAsync<RoleModel>(dbRole))
                        {
                            roleModel.UserId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _roleService.UpdateRoleAsync(roleModel);
                            if (res.Equals(1))
                            {
                                TempData["success"] = "Role has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Role has not been updated";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(roleModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
                try
                {
                var guid_id = _protector.Unprotect(id);
                var dbRole = await _roleService.GetRoleByIdAsync(Convert.ToInt32(guid_id));
                    if (dbRole != null)
                    {
                        var res = await _roleService.DeleteRoleAsync(dbRole);

                        if (res.Equals(1))
                        {
                            TempData["error"] = "Role has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Role has not been deleted";
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
