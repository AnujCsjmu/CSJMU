using CoreLayout.Models.PCP;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PSP
{
    public class PCPRegistrationController : Controller
    {
        private readonly ILogger<PCPRegistrationController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        private readonly IQPMasterService _qPMasterService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;
        public PCPRegistrationController(ILogger<PCPRegistrationController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, CommonController commonController, IQPMasterService qPMasterService, ICourseService courseService, IBranchService branchService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _commonController = commonController;
            _qPMasterService = qPMasterService;
            _courseService = courseService;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            pCPRegistrationModel.QPCodeList =await _qPMasterService.GetAllQPMaster();
            pCPRegistrationModel.CourseList = await _courseService.GetAllCourse();
            pCPRegistrationModel.BranchList = await _branchService.GetAllBranch();
            //return View(pSPRegistrationModel);
            return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(PCPRegistrationModel pCPRegistrationModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                string salt = CreateSalt();
                string saltedHash = ComputeSaltedHash(pCPRegistrationModel.Password, salt);
                pCPRegistrationModel.Salt = salt;
                pCPRegistrationModel.SaltedHash = saltedHash;
                pCPRegistrationModel.IPAddress = ipAddress;
                //registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPRegistrationModel.QPCodeList = await _qPMasterService.GetAllQPMaster();
                pCPRegistrationModel.CourseList = await _courseService.GetAllCourse();
                pCPRegistrationModel.BranchList = await _branchService.GetAllBranch();
                int emailAlreadyExit = _commonController.emailAlreadyExitsPSP(pCPRegistrationModel.EmailID);
                int mobileAlreadyExit = _commonController.mobileAlreadyExitsPSP(pCPRegistrationModel.MobileNo);
                if (emailAlreadyExit == 0 && mobileAlreadyExit == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var res = await _pCPRegistrationService.CreatePCPRegistrationAsync(pCPRegistrationModel);
                        if (res.Equals(1))
                        {
                            //success
                            errormsg = "Successfully Saved.";
                            return View("~/Views/Home/Login.cshtml");
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
                    errormsg = "Mobile or Email already exits!";
                    //return View(pSPRegistrationModel);
                    return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                return RedirectToAction("Registration", "PSPRegistration");
            }
            //return RedirectToAction(nameof(Registration));
            return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
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
