using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Common;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPApproval;
using CoreLayout.Services.PCP.PCPRegistration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class PCPApprovalController : Controller
    {
        private readonly ILogger<PCPApprovalController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        private readonly IPCPApprovalService _pCPApprovalService;
        private readonly CommonController _commonController;
        private readonly IMailService _mailService;
        public PCPApprovalController(ILogger<PCPApprovalController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IDataProtectionProvider provider, IPCPApprovalService pCPApprovalService, CommonController commonController, IMailService mailService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _protector = provider.CreateProtector("PCPApproval.PCPApprovalController");
            _pCPApprovalService = pCPApprovalService;
            _commonController = commonController;
            _mailService = mailService;
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                            where reg.IsApproved == null
                            select reg).ToList();
                //var data = await _pCPRegistrationService.GetAllPCPRegistration();

                foreach (var _data in data)
                {
                    var stringId = _data.PCPRegID.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.PCPRegID;
                }
                id = id + 1;
                ViewBag.MaxUserIdId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/PCP/PCPApproval/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPApproval/Index.cshtml");

        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));
                string filePath = "~/PCPPhoto/" + data.UploadFileName;
                data.UploadFileName = filePath;
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPApproval/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _pCPRegistrationService.DeletePCPRegistrationAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "User has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "User has not been deleted";
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
        

        public async Task<IActionResult> GetJsonData(string uid)
        {
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            int result = 0;
            string msg = string.Empty;
            if (uid != null)
            {
               
                List<string> list = new List<string>();

                String[] array = uid.Split(",");
                for (int i = 0; i < array.Length; i++)
                {
                    //list.Add(array[i]);
                    var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(array[i]));
                    pCPRegistrationModel.PCPRegID = Convert.ToInt32(array[i]);
                    pCPRegistrationModel.UserName = data.UserName;
                    pCPRegistrationModel.MobileNo = data.MobileNo;
                    pCPRegistrationModel.EmailID = data.EmailID;

                    //generate loginid
                    string randomno = Generate5digitRandomNuber();
                    string loginid = "PCP" + randomno;
                    pCPRegistrationModel.LoginID = loginid;

                    //generate password
                    string salt = CreateSalt();
                    string randompwd = CreateRandomPassword();
                    pCPRegistrationModel.Password = randompwd;
                    string saltedHash = ComputeSaltedHash(randompwd, salt);
                    pCPRegistrationModel.Salt = salt;
                    pCPRegistrationModel.SaltedHash = saltedHash;

                    pCPRegistrationModel.IsUserActive = data.IsUserActive;
                    pCPRegistrationModel.RefID = data.RefID;
                    pCPRegistrationModel.RefType = data.RefType;
                    pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    //pCPRegistrationModel.InstituteId = data.InstituteId;
                    pCPRegistrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    pCPRegistrationModel.RoleId = 19;
                    pCPRegistrationModel.IsApproved = 1;
                    pCPRegistrationModel.IsApprovedBy = HttpContext.Session.GetInt32("UserId");

                    //send message
                    bool res = SendRegistraionMeassage(pCPRegistrationModel.UserName, pCPRegistrationModel.MobileNo, pCPRegistrationModel.LoginID, pCPRegistrationModel.Password);
                    if (res == true)
                    {
                        pCPRegistrationModel.IsMobileReminder = "Y";
                    }
                    else
                    {
                        pCPRegistrationModel.IsMobileReminder = "N";
                    }
                    //send mail
                    bool res1 = SendRegistraionMail(pCPRegistrationModel.UserName, pCPRegistrationModel.EmailID, pCPRegistrationModel.LoginID, pCPRegistrationModel.Password);
                    if (res1 == true)
                    {
                        pCPRegistrationModel.IsEmailReminder = "Y";
                    }
                    else
                    {
                        pCPRegistrationModel.IsEmailReminder = "N";
                    }

                    result = await _pCPApprovalService.CreatePCPApprovalAsync(pCPRegistrationModel);
                    if (result.Equals(1))
                    {
                        TempData["success"] = "User has been approved successfully";
                    }
                    else
                    {
                        TempData["error"] = "User has been not approved successfully";
                    }
                }
               
            }
            //return View("~/Views/PCP/PCPApproval/Index.cshtml", pCPRegistrationModel);
            return RedirectToAction(nameof(Index));
        }

        public JsonResult SendReminder(string uid)
        {
            return Json(uid);
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
        public string Generate5digitRandomNuber()
        {
            string number = "";
            Random random = new Random();

            int n = random.Next(0, 100000);
            number += n.ToString("D5");
            return number;
        }
        private string CreateRandomPassword(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public bool SendRegistraionMeassage(string username, string mobile, string loginid, string password)
        {
            bool result = false;
            string message = "Dear " + username + ", Attendance for Subject Codes: " + mobile + " is not entered today. Please specify reasons at " + loginid + " " + password + ". CSJMU, Kanpur";

            result = _commonController.SendSMSWithTemplateId(mobile, message, "1607100000000228609");
            return result;
        }

        public bool SendRegistraionMail(string username, string email, string loginid, string password)
        {
            bool result = false;
            MailRequest request = new MailRequest();

            request.Subject = "Paper setter Registration!!";
            request.Body = "<br/> Your registration completed succesfully." +
                   "<br/> This registration is for paper upload." +
                   "<br/><br/> Your loginid is " + loginid + " and password is " + password + "</a>";
            //request.Attachments = "";
            request.ToEmail = email;
            _mailService.SendEmailAsync(request);
            return result;
        }

       
    }
}



