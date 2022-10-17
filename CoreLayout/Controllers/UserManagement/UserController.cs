using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.RoleToRoleMapping;
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
    [Authorize(Roles = "Administrator,Institute,Controller Of Examination,Institute")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private IHttpContextAccessor _httpContextAccessor;
        private static string errormsg = "";
        public UserController(ILogger<UserController> logger, IRegistrationService registrationService, IRoleService roleService, CommonController commonController, IHttpContextAccessor httpContextAccessor)
        {
            _roleService = roleService;
            _logger = logger;
            _registrationService = registrationService;
            _commonController = commonController;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        //[AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            object data = null;
            int? RoleId = HttpContext.Session.GetInt32("RoleId");
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (RoleId == 1)//1 is admin
            {
                data = await _registrationService.GetAllRegistrationAsync();
            }
            else if (RoleId == 3)//3 is coe
            {
                data = (from registration in _registrationService.GetAllRegistrationAsync().Result
                        where registration.CreatedBy == UserId && registration.RoleId!=19
                        select registration).ToList();//19 is paper setter
            }
            else
            {

                data = (from registration in _registrationService.GetAllRegistrationAsync().Result
                        where registration.CreatedBy == UserId /*&& registration.RoleId==RoleId*/
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
        //[AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create()
        {
            RegistrationModel registrationModel = new RegistrationModel();
            registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
            int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            if (RoleId == 1)
            {
                registrationModel.RoleList = await _roleService.GetAllRoleAsync();
            }
            else
            {
                registrationModel.RoleList = await _roleService.GetRoleToRoleMappingByRoleAsync(RoleId);
            }

            //BindInstitute();
            return View(registrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(RegistrationModel registrationModel)
        {
            try
            {
                registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
                // registrationModel.RoleList = await _roleService.GetAllRoleAsync();
                int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (RoleId == 1)
                {
                    registrationModel.RoleList = await _roleService.GetAllRoleAsync();
                }
                else
                {
                    registrationModel.RoleList = await _roleService.GetRoleToRoleMappingByRoleAsync(RoleId);
                }
                ///int emailAlreadyExit = _commonController.emailAlreadyExits(registrationModel.EmailID);
                //int mobileAlreadyExit = _commonController.mobileAlreadyExits(registrationModel.MobileNo);
                int loginidAlreadyExit = _commonController.loginidAlreadyExit(registrationModel.LoginID);
                //if (emailAlreadyExit == 0)
                //{
                    //if (mobileAlreadyExit == 0)
                    //{
                        if (loginidAlreadyExit == 0)
                        {
                            string salt = _commonController.CreateSalt();
                            string saltedHash = _commonController.ComputeSaltedHash(registrationModel.Password, salt);
                            registrationModel.Salt = salt;
                            registrationModel.SaltedHash = saltedHash;
                            registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                            registrationModel.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
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
                                ModelState.AddModelError("", "Model state is not valid");
                                return View(registrationModel);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "LoginId already exits");
                            return View(registrationModel);
                        }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "Email already exits");
                    //    return View(registrationModel);
                    //}
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Mobile already exits");
                //    return View(registrationModel);
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View(registrationModel);
            }
        }

        //[AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var Data = await _registrationService.GetRegistrationByIdAsync(id);
            Data.InstituteList = await _registrationService.GetAllInstituteAsync();
            //Data.RoleList = await _roleService.GetAllRoleAsync();
            if (RoleId == 1)
            {
                Data.RoleList = await _roleService.GetAllRoleAsync();
            }
            else
            {
                Data.RoleList = await _roleService.GetRoleToRoleMappingByRoleAsync(RoleId);
            }
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
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, RegistrationModel registrationModel)
        {
            try
            {
                registrationModel.InstituteList = await _registrationService.GetAllInstituteAsync();
                //registrationModel.RoleList = await _roleService.GetAllRoleAsync();
                int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (RoleId == 1)
                {
                    registrationModel.RoleList = await _roleService.GetAllRoleAsync();
                }
                else
                {
                    registrationModel.RoleList = await _roleService.GetRoleToRoleMappingByRoleAsync(RoleId);
                }
                registrationModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                registrationModel.ModifiedBy = (int)HttpContext.Session.GetInt32("UserId");
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

        //[AuthorizeContext(ViewAction.Details)]
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

        public async Task<IActionResult> ChangePassword()
        {
            int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var Data = await _registrationService.GetRegistrationByIdAsync(UserId);
            if (Data == null)
            {
                return NotFound();
            }
            return View(Data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(RegistrationModel registrationModel)
        {
            try
            {
                if (registrationModel.OldPassword != null)
                {
                    int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                    int UserId = (int)HttpContext.Session.GetInt32("UserId");
                    var Data = await _registrationService.GetRegistrationByIdAsync(UserId);
                    if (Data != null)
                    {
                        if (String.Compare(ComputeSaltedHash(registrationModel.OldPassword, Data.Salt), Data.SaltedHash) == 0)
                        {
                            string salt = _commonController.CreateSalt();
                            string saltedHash = _commonController.ComputeSaltedHash(registrationModel.Password, salt);
                            registrationModel.UserID = UserId;
                            registrationModel.Salt = salt;
                            registrationModel.SaltedHash = saltedHash;
                            registrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                            registrationModel.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                            registrationModel.OldSalt = Data.Salt;
                            registrationModel.OldSaltedHash = Data.SaltedHash;
                            registrationModel.RoleId = Data.RoleId;
                            registrationModel.UserName = Data.UserName;
                            if (ModelState.IsValid)
                            {
                                var result = await _registrationService.ChangePassword(registrationModel);
                                if (result == 1)
                                {
                                    TempData["success"] = "Password has been changed";
                                    //_httpContextAccessor.HttpContext.Session.Clear();
                                    ////Clear cookies
                                    //var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
                                    //foreach (var cookie in cookies)
                                    //{
                                    //    _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie.Key);
                                    //}
                                }
                                else
                                {
                                    TempData["error"] = "Password has not been changed";
                                }
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ModelState.AddModelError("", "Model State is not valid");
                                return View(registrationModel);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid old password!");
                            return View(registrationModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Data not found");
                        return View(registrationModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Old Password is not blank");
                    return View(registrationModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            //BindInstitute();
            return View(registrationModel);
        }

        public static string ComputeSaltedHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("salt");
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm hash = new SHA1Managed();

            List<byte> passwordWithSaltBytes = new List<byte>(passwordBytes);
            passwordWithSaltBytes.AddRange(saltBytes);

            byte[] hashBytes = hash.ComputeHash(passwordWithSaltBytes.ToArray());

            return Convert.ToBase64String(hashBytes);
        }
        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyMobile(string mobileNo)
        //{
        //    var already = (from data in _registrationService.GetAllRegistrationAsync().Result
        //                   where data.MobileNo == mobileNo.Trim()
        //                   select data).ToList();
        //    if (already.Count > 0)
        //    {
        //        return Json($"{mobileNo} is already in use.");
        //    }
        //    return Json(true);
        //}

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyEmail(string emailID)
        //{
        //    var already = (from data in _registrationService.GetAllRegistrationAsync().Result
        //                   where data.EmailID == emailID.Trim()
        //                   select data).ToList();
        //    if (already.Count > 0)
        //    {
        //        return Json($"{emailID} is already in use.");
        //    }
        //    return Json(true);
        //}
        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyLoginId(string loginID)
        //{
        //    var already = (from data in _registrationService.GetAllRegistrationAsync().Result
        //                   where data.LoginID == loginID.Trim()
        //                   select data).ToList();
        //    if (already.Count > 0)
        //    {
        //        return Json($"{loginID} is already in use.");
        //    }
        //    return Json(true);
        //}
    }
}
