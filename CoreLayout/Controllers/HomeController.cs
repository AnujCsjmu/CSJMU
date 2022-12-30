using AspNetCore.ReCaptcha;
using CoreLayout.Models.Common;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _userService;
        private readonly IRegistrationService _registrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        public HomeController(ILogger<HomeController> logger, ILoginService userService, IRegistrationService registrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, CommonController commonController)
        {
            _logger = logger;
            _userService = userService;
            _registrationService = registrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _commonController = commonController;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnURL)
        {
            _logger.LogInformation("HomeController.Index method called!!!");
            try
            {
                HttpContext.Session.Clear();
                if (errormsg != "")
                {
                    ViewBag.errormsg = errormsg;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Exception through...");
            }

            return View();
        }
        //[ValidateReCaptcha]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (loginModel.LoginID != null && loginModel.Password != null)
                    {
                        var abc = loginModel.LoginID;
                        bool exists = abc.IndexOf("~", StringComparison.CurrentCultureIgnoreCase) > -1;
                        if (exists == false)
                        {
                            var getuser = _userService.GetUserDetailByLoginId(loginModel.LoginID);
                            if (getuser.Result != null)
                            {
                                if (String.Compare(ComputeSaltedHash(loginModel.Password, getuser.Result.Salt), getuser.Result.SaltedHash) == 0)
                                {
                                    if (getuser.Result.IsUserActive == 1 && getuser.Result.IsRoleActive == 1)
                                    {
                                        ClaimsIdentity identity = null;
                                        bool isAuthenticated = false;

                                        //Create the identity for the user  
                                        identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, getuser.Result.LoginID), new Claim(ClaimTypes.Role, getuser.Result.RoleName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                                        isAuthenticated = true;
                                        if (isAuthenticated)
                                        {
                                            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                                            var principal = new ClaimsPrincipal(identity);
                                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                            HttpContext.Session.SetString("Name", getuser.Result.UserName);
                                            HttpContext.Session.SetInt32("Id", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("UserId", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("RoleId", getuser.Result.RoleId);
                                            HttpContext.Session.SetInt32("SessionInstituteId", getuser.Result.InstituteId);
                                            HttpContext.Session.SetString("SessionInstituteName", getuser.Result.InstituteName);
                                            HttpContext.Session.SetString("SessionInstituteCode", getuser.Result.InstituteCode);
                                            HttpContext.Session.SetString("IPAddress", ipAddress);
                                            return RedirectToAction("Index", "Dashboard");
                                        }
                                    }
                                    else
                                    {
                                        //errormsg = "User is not active!";
                                        ModelState.AddModelError("", "User is not active!");
                                        //return View();
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Invalid password!");
                                    // errormsg = "Invalid password!!";
                                    //return View();
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid UserName or Password");
                                //return View();
                            }
                        }
                        else
                        {
                            //collage login with admin id and password
                            var loginid = loginModel.LoginID;
                            var stringAfterChar = loginid.Split('~').Last();
                            string stringBeforeChar = loginid.Split('~').First();

                            var getuser = _userService.GetUserDetailByLoginId(stringAfterChar);
                            var getadminuser = _userService.GetUserDetailByLoginId(stringBeforeChar);
                            if (getuser.Result != null && getadminuser.Result !=null)
                            {
                                if (String.Compare(ComputeSaltedHash(loginModel.Password, getadminuser.Result.Salt), getadminuser.Result.SaltedHash) == 0)
                                {
                                    if (getuser.Result.IsUserActive == 1 && getuser.Result.IsRoleActive == 1 && getadminuser.Result.IsUserActive == 1 && getadminuser.Result.IsRoleActive == 1)
                                    {
                                        ClaimsIdentity identity = null;
                                        bool isAuthenticated = false;

                                        //Create the identity for the user  
                                        identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, getuser.Result.LoginID), new Claim(ClaimTypes.Role, getuser.Result.RoleName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                                        isAuthenticated = true;
                                        if (isAuthenticated)
                                        {
                                            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                                            var principal = new ClaimsPrincipal(identity);
                                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                            HttpContext.Session.SetString("Name", getuser.Result.UserName);
                                            HttpContext.Session.SetInt32("Id", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("UserId", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("RoleId", getuser.Result.RoleId);
                                            HttpContext.Session.SetInt32("SessionInstituteId", getuser.Result.InstituteId);
                                            HttpContext.Session.SetString("SessionInstituteName", getuser.Result.InstituteName);
                                            HttpContext.Session.SetString("SessionInstituteCode", getuser.Result.InstituteCode);
                                            HttpContext.Session.SetString("IPAddress", ipAddress);
                                            return RedirectToAction("Index", "Dashboard");
                                        }
                                    }
                                    else
                                    {
                                        //errormsg = "User is not active!";
                                        ModelState.AddModelError("", "User is not active!");
                                        //return View();
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Invalid password!");
                                    // errormsg = "Invalid password!!";
                                    //return View();
                                }
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter UserName or Password");
                        //return View();
                    }
                }
                else
                {
                    //errormsg = "Model state is invalid!";
                    ModelState.AddModelError("", "Invalid captcha!");
                    // return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                //errormsg = ex.StackTrace.ToString();
                //return RedirectToAction("Login", "Home");
                ModelState.AddModelError("", ex.ToString());
            }
            //errormsg = "some thing went wrong!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            RegistrationModel registrationModel = new RegistrationModel();
            registrationModel.RoleList = (from role in await _roleService.GetAllRoleAsync()
                                          where role.RoleID == 19
                                          select role).ToList();
            return View(registrationModel);
        }

        public static string CreateSalt()
        {
            // Define salt sizes
            int minSaltSize = 4;
            int maxSaltSize = 8;
            byte[] saltBytes;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            saltBytes = new byte[saltSize];
            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel registrationModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                string salt = CreateSalt();
                string saltedHash = ComputeSaltedHash(registrationModel.Password, salt);
                registrationModel.Salt = salt;
                registrationModel.SaltedHash = saltedHash;
                registrationModel.IPAddress = ipAddress;
                //registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");

                int emailAlreadyExit = _commonController.emailAlreadyExits(registrationModel.EmailID);
                int mobileAlreadyExit = _commonController.mobileAlreadyExits(registrationModel.MobileNo);
                if (emailAlreadyExit == 0 && mobileAlreadyExit == 0)
                {
                    if (ModelState.IsValid)
                    {
                        //registrationModel.Id = (int)HttpContext.Session.GetInt32("Id");
                        var res = await _registrationService.CreateRegistrationAsync(registrationModel);
                        if (res.Equals(1))
                        {
                            //success
                            errormsg = "Successfully Saved.";
                        }
                        else
                        {
                            //falure
                            errormsg = "Data Not Saved!";
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Some thing went wrong!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mobile or Email already exits!");
                    return View(registrationModel);
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                return RedirectToAction("Registration", "Home");
            }
            return RedirectToAction(nameof(Registration));
        }
        public IActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetAsync(RegistrationModel registrationModel)
        {
            int insertSMSEmail = 0;
            try
            {
                var data = await _registrationService.ForgetPassword(registrationModel.EmailID, registrationModel.MobileNo, registrationModel.LoginID);

                if (data == null)
                {
                    ModelState.AddModelError("", "No Data found!");
                    return View(registrationModel);
                }
                else if (data != null)
                {
                    string salt = CreateSalt();
                    string saltedHash = ComputeSaltedHash(registrationModel.Password, salt);
                    registrationModel.Salt = salt;
                    registrationModel.SaltedHash = saltedHash;
                    registrationModel.RoleId = data.RoleId;
                    registrationModel.MobileNo = data.MobileNo;
                    registrationModel.EmailID = data.EmailID;
                    registrationModel.UserID = data.UserID;
                    registrationModel.UserName = data.UserName;
                    registrationModel.CreatedBy = data.CreatedBy;
                    registrationModel.Remarks = "Forget Password";
                    registrationModel.IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    bool sendmessage = _commonController.SendRegistraionMeassage(registrationModel.UserName, registrationModel.MobileNo, registrationModel.LoginID, registrationModel.Password);
                    bool sendemail = _commonController.SendRegistraionMail(registrationModel.UserName, registrationModel.EmailID, registrationModel.LoginID, registrationModel.Password);
                    registrationModel.MobileReminder = sendmessage.ToString();
                    registrationModel.EmailReminder = sendemail.ToString();
                    var res = await _registrationService.InsertEmailSMSHistory(registrationModel);
                    if (res.Equals(1))
                    {
                        ModelState.AddModelError("", "Password send your's registered mobile no and email id");
                        return View(registrationModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email and SMS not send!");
                        return View(registrationModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Multiple record exits!");
                    return View(registrationModel);
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                return View(registrationModel);
            }
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _httpContextAccessor.HttpContext.Session.Clear();
            //Clear cookies
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            foreach (var cookie in cookies)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie.Key);
            }
            var myCookies = Request.Cookies.Keys;
            foreach (string cookie in myCookies)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie, new CookieOptions()
                {
                    Domain = "www.google.com"
                });
            }
            return RedirectToAction("Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UnAuthorized()
        {
            return View();
        }

        public IActionResult ExceptionLog()
        {
            return View();
        }
        public IActionResult WRNLogin()
        {
            return RedirectToAction("Login", "WRNRegistration");
        }

    }
}
