using CoreLayout.Models.PSP;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PSP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PSP
{
    public class PSPRegistrationController : Controller
    {
        private readonly ILogger<PSPRegistrationController> _logger;
        private readonly IPSPRegistrationService _pSPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        public PSPRegistrationController(ILogger<PSPRegistrationController> logger, IPSPRegistrationService pSPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, CommonController commonController)
        {
            _logger = logger;
            _pSPRegistrationService = pSPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _commonController = commonController;
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            PSPRegistrationModel pSPRegistrationModel = new PSPRegistrationModel();
            pSPRegistrationModel.RoleList = (from role in await _roleService.GetAllRoleAsync()
                                          where role.RoleID == 19
                                          select role).ToList();
            //return View(pSPRegistrationModel);
            return View("~/Views/PSP/PSPRegistration/Registration.cshtml", pSPRegistrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(PSPRegistrationModel pSPRegistrationModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                string salt = CreateSalt();
                string saltedHash = ComputeSaltedHash(pSPRegistrationModel.Password, salt);
                pSPRegistrationModel.Salt = salt;
                pSPRegistrationModel.SaltedHash = saltedHash;
                pSPRegistrationModel.IPAddress = ipAddress;
                //registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pSPRegistrationModel.RoleList = (from role in await _roleService.GetAllRoleAsync()
                                                 where role.RoleID == 19
                                                 select role).ToList();
                int emailAlreadyExit = _commonController.emailAlreadyExitsPSP(pSPRegistrationModel.EmailID);
                int mobileAlreadyExit = _commonController.mobileAlreadyExitsPSP(pSPRegistrationModel.MobileNo);
                if (emailAlreadyExit == 0 && mobileAlreadyExit == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var res = await _pSPRegistrationService.CreatePSPRegistrationAsync(pSPRegistrationModel);
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
                        //ModelState.AddModelError("", "Some thing went wrong!");
                        errormsg = "Some thing went wrong!";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mobile or Email already exits!");
                    //errormsg = "Mobile or Email already exits!";
                    //return View(pSPRegistrationModel);
                    return View("~/Views/PSP/PSPRegistration/Registration.cshtml", pSPRegistrationModel);
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                return RedirectToAction("Registration", "PSPRegistration");
            }
            //return RedirectToAction(nameof(Registration));
            return View("~/Views/PSP/PSPRegistration/Registration.cshtml", pSPRegistrationModel);
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
    }
}
