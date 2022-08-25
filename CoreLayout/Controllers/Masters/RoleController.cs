using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Role;
using Microsoft.AspNetCore.Authorization;
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
        public RoleController(ILogger<RoleController> logger, IRoleService roleMService)
        {
            _logger = logger;
            _roleService = roleMService;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _roleService.GetAllRoleAsync());
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _roleService.GetRoleByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }


        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                roleModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                roleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                //countryModel.CreatedDate = System.DateTime.Now;


                if (ModelState.IsValid)
                {
                    var res = await _roleService.CreateRoleAsync(roleModel);
                    if (res.ToString().Equals("1"))
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
            else
            {
                TempData["error"] = "Some thing went wrong!";
                return RedirectToAction(nameof(Index));

            }
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _roleService.GetRoleByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, RoleModel roleModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    roleModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    roleModel.RoleID = id;
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _roleService.GetRoleByIdAsync(id);
                        if (await TryUpdateModelAsync<RoleModel>(dbRole))
                        {
                            roleModel.UserId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _roleService.UpdateRoleAsync(roleModel);
                            if (res.ToString().Equals("1"))
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
                else
                {
                    return RedirectToAction("Login", "Home");

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
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _roleService.GetRoleByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _roleService.DeleteRoleAsync(dbRole);

                        if (res.ToString().Equals("1"))
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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}
