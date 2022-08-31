using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.UserManagement
{
    [Authorize(Roles = "Administrator,Institute")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        public UserController(ILogger<UserController> logger, IRegistrationService registrationService, IRoleService roleService, CommonController commonController)
        {
            _roleService = roleService;
            _logger = logger;
            _registrationService = registrationService;
            _commonController = commonController;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            object data = null;
            int? RoleId =  HttpContext.Session.GetInt32("RoleId");
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (RoleId == 1)
            {
                 data = await _registrationService.GetAllRegistrationAsync();
            }
            else
            {

                data = (from registration in _registrationService.GetAllRegistrationAsync().Result
                               where registration.CreatedBy == UserId && registration.RoleId==RoleId
                               select registration).ToList();
            }

            return View(data);
        }
        //public void BindInstitute()
        //{
        //    var InstituteList = (from institute in _registrationService.GetAllInstituteAsync().Result
        //                         select new SelectListItem()
        //                         {
        //                             Text = institute.InstituteName,
        //                             Value = institute.InstituteId.ToString(),
        //                         }).ToList();

        //    InstituteList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    ViewBag.InstituteList = InstituteList;

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
        //    ViewBag.RoleList = RoleList;
        //}

        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create()
        {
            RegistrationModel registrationModel = new RegistrationModel();
            registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
            int? RoleId = HttpContext.Session.GetInt32("RoleId");
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (RoleId == 1)
            {
                registrationModel.RoleList = await _roleService.GetAllRoleAsync();
            }
            else
            {

                registrationModel.RoleList = (from role in _roleService.GetAllRoleAsync().Result
                        where role.RoleID== RoleId
                                              select role).ToList();
            }
         
            //BindInstitute();
            return View(registrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(RegistrationModel registrationModel)
        {
            try
            {
                registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
                registrationModel.RoleList = await _roleService.GetAllRoleAsync();

                int emailAlreadyExit = _commonController.emailAlreadyExits(registrationModel.EmailID);
                int mobileAlreadyExit = _commonController.mobileAlreadyExits(registrationModel.MobileNo);
                if (emailAlreadyExit == 0 && mobileAlreadyExit == 0)
                {
                   
                    registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    string salt = _commonController.CreateSalt();
                    string saltedHash = _commonController.ComputeSaltedHash(registrationModel.Password, salt);
                    registrationModel.Salt = salt;
                    registrationModel.SaltedHash = saltedHash;

                    if (ModelState.IsValid)
                    {
                        var result = await _registrationService.CreateRegistrationAsync(registrationModel);
                        if (result == 1)
                        {
                            TempData["success"] = "User has been created";
                        }
                        else
                        {
                            TempData["error"] = "User has not been created";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Some thing went wrong");
                        return View(registrationModel);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Some thing went wrong");
                    return View(registrationModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View(registrationModel);
            }
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var Data = await _registrationService.GetRegistrationByIdAsync(id);
            Data.InstituteList = await _registrationService.GetAllInstituteAsync();
            Data.RoleList = await _roleService.GetAllRoleAsync();
            if (Data == null)
            {
                return NotFound();
            }
            return View(Data);

            //if (HttpContext.Session.GetString("Name") != null)
            //{
            //    BindInstitute();
            //    return View(await _registrationService.GetRegistrationByIdAsync(id));
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Home");
            //}
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, RegistrationModel registrationModel)
        {
            try
            {
                registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
                registrationModel.RoleList = await _roleService.GetAllRoleAsync();
                registrationModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (registrationModel.IsPasswordChange == "1")
                {
                    string salt = _commonController.CreateSalt();
                    string saltedHash = _commonController.ComputeSaltedHash(registrationModel.Password, salt);
                    registrationModel.Salt = salt;
                    registrationModel.SaltedHash = saltedHash;
                }

                //if (ModelState.IsValid)
                //{
                var value = await _registrationService.GetRegistrationByIdAsync(id);
                registrationModel.UserRoleId = value.UserRoleId;
                if (value != null)
                {
                    var result = await _registrationService.UpdateRegistrationAsync(registrationModel);
                    if (result == 0)
                    {

                        TempData["error"] = "User has not been updated";
                        return View(registrationModel);
                    }
                    else
                    {
                        TempData["success"] = "User has been updated";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Record not found");
                }
                // }
                //else
                // {
                //     return View(registrationModel);
                // }


                //if (HttpContext.Session.GetString("Name") != null)
                //{
                //    registrationModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                //    registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                //    if (registrationModel.IsPasswordChange == "1")
                //    {
                //        string salt = _commonController.CreateSalt();
                //        string saltedHash = _commonController.ComputeSaltedHash(registrationModel.Password, salt);
                //        registrationModel.Salt = salt;
                //        registrationModel.SaltedHash = saltedHash;
                //    }


                //    //if (ModelState.IsValid)
                //    //{
                //    var value = await _registrationService.GetRegistrationByIdAsync(id);
                //    registrationModel.UserRoleId = value.UserRoleId;
                //    //if (await TryUpdateModelAsync<RegistrationModel>(value))
                //    //{
                //    var res = await _registrationService.UpdateRegistrationAsync(registrationModel);
                //    if (res.ToString().Equals("1"))
                //    {
                //        TempData["success"] = "User has been updated";
                //    }
                //    else
                //    {
                //        TempData["error"] = "User has not been updated";
                //    }
                //    return RedirectToAction(nameof(Index));
                //    //}
                //    //}
                //}
                //else
                //{
                //    return RedirectToAction("Login", "Home");
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            //BindInstitute();
            return View(registrationModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbRole = await _registrationService.GetRegistrationByIdAsync(id);
                if (dbRole != null)
                {
                    var result = await _registrationService.DeleteRegistrationAsync(dbRole);
                    if (result == 1)
                    {
                        TempData["error"] = "User has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "User has not been deleted";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Record not found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));

            //if (HttpContext.Session.GetString("Name") != null)
            //{
            //    try
            //    {
            //        var dbRole = await _registrationService.GetRegistrationByIdAsync(id);
            //        if (dbRole != null)
            //        {
            //            var res = await _registrationService.DeleteRegistrationAsync(dbRole);

            //            if (res.ToString().Equals("1"))
            //            {
            //                TempData["error"] = "User has been deleted";
            //            }
            //            else
            //            {
            //                TempData["error"] = "User has not been deleted";
            //            }
            //        }
            //        else
            //        {
            //            TempData["error"] = "Some thing went wrong!";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", ex.ToString());
            //    }

            //    return RedirectToAction(nameof(Index));
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Home");
            //}
        }

        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                //BindInstitute();
                return View(await _registrationService.GetRegistrationByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}
