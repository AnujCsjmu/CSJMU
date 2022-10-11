using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.PCP;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendReminder;
using CoreLayout.Services.PCP.PCPUploadPaper;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignRole;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Services.UserManagement.ParentMenu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{

    public class CommonController : Controller
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;
        private readonly IRegistrationService _registrationService;
        private readonly IAssignRoleService _assignRoleService;
        private readonly IParentMenuService _parentMenuService;
        private readonly IButtonPermissionService _buttonPermissionService;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IMailService _mailService;
        private readonly IPCPSendReminderService _pCPSendReminderService;
        private IHttpContextAccessor _httpContextAccessor;
        public CommonController(ILogger<CommonController> logger, IDashboardService dashboardService, ICommonService commonService, IRegistrationService registrationService, IAssignRoleService assignRoleService, IParentMenuService parentMenuService, IButtonPermissionService buttonPermissionService, IPCPRegistrationService pCPRegistrationService, IPCPUploadPaperService pCPUploadPaperService, IMailService mailService, IPCPSendReminderService pCPSendReminderService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _commonService = commonService;
            _registrationService = registrationService;
            _assignRoleService = assignRoleService;
            _parentMenuService = parentMenuService;
            _buttonPermissionService = buttonPermissionService;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _mailService = mailService;
            _pCPSendReminderService = pCPSendReminderService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ActionResult> RefereshMenuAsync()
        {
            var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {

                //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);
                IDashboardService _dashboardService1 = _dashboardService;
                List<DashboardModel> alllevels = await _dashboardService1.GetDashboardByRoleAndUser(roleid, userid);

                List<DashboardModel> level1 = new List<DashboardModel>();
                List<DashboardModel> level2 = new List<DashboardModel>();
                List<DashboardModel> level3 = new List<DashboardModel>();

                foreach (DashboardModel dm in alllevels)
                {
                    if (dm.SubMenuName.Equals("*") && dm.MenuName.Equals("*"))
                    {
                        level1.Add(dm);
                    }
                    else if (dm.SubMenuName != "*" && dm.MenuName.Equals("*"))
                    {
                        level2.Add(dm);
                    }
                    else
                    {
                        level3.Add(dm);
                    }
                }

                HttpContext.Session.SetString("Level1List", JsonConvert.SerializeObject(level1));
                HttpContext.Session.SetString("Level2List", JsonConvert.SerializeObject(level2));
                HttpContext.Session.SetString("Level3List", JsonConvert.SerializeObject(level3));

                return RedirectToAction("DashBoard", "Index"); ;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //public int CheckPagePermission(int roleid, int userid, AuthorizationFilterContext context)
        //{
        //    int result = 0;
        //    try
        //    {
        //        //enterurl = enterurl.Remove(0, 23);//for local host
        //        var controllerName = context.ActionDescriptor.RouteValues["controller"];//browser controller
        //        var actionName = context.ActionDescriptor.RouteValues["action"];//browser action
        //        var enterurl = string.Empty;
        //        enterurl= controllerName + "/" + actionName;


        //        var dbUrl = string.Empty;
        //        var getPagePermission = _commonService.GetDashboardByRoleAndUser(roleid, userid).Result;
        //        foreach (var _getPagePermission in getPagePermission)
        //        {
        //            var Controller = _getPagePermission.Controller;//get controller from db
        //            var Action = _getPagePermission.Action;//get action from db
        //            dbUrl = Controller + "/" + Action;
        //            if (dbUrl == enterurl)
        //            {
        //                result = 1;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = 2;
        //    }
        //    return result;
        //}
        public int emailAlreadyExits(string EmailID)
        {
            int result = 0;
            try
            {
                var already = (from user in _registrationService.GetAllRegistrationAsync().Result
                               where user.EmailID == EmailID
                               select new SelectListItem()
                               {
                                   Text = user.EmailID,
                                   Value = user.UserID.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
        public int emailAlreadyExitsPSP(string EmailID)
        {
            int result = 0;
            try
            {
                var already = (from user in _pCPRegistrationService.GetAllPCPRegistration().Result
                               where user.EmailID == EmailID
                               select new SelectListItem()
                               {
                                   Text = user.EmailID,
                                   Value = user.PCPRegID.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
        public int mobileAlreadyExits(string MobileNo)
        {
            int result = 0;
            try
            {
                var already = (from user in _registrationService.GetAllRegistrationAsync().Result
                               where user.MobileNo == MobileNo
                               select new SelectListItem()
                               {
                                   Text = user.MobileNo,
                                   Value = user.UserID.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
        public int mobileAlreadyExitsPSP(string MobileNo)
        {
            int result = 0;
            try
            {
                var already = (from user in _pCPRegistrationService.GetAllPCPRegistration().Result
                               where user.MobileNo == MobileNo
                               select new SelectListItem()
                               {
                                   Text = user.MobileNo,
                                   Value = user.PCPRegID.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
        public int assignRoleAlreadyExits(int userid, int roleid)
        {
            int result = 0;
            try
            {
                var already = (from userRole in _assignRoleService.GetAllRoleAssignAsync().Result
                               where userRole.RoleUserId == userid && userRole.RoleId == roleid
                               select new SelectListItem()
                               {
                                   Text = userRole.RoleId.ToString(),
                                   Value = userRole.UserRoleId.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }

        public int AlreadyExitsButtonPermission(int buttonid, int userid, int roleid, string controller, string action)
        {
            int result = 0;
            try
            {
                var already = _buttonPermissionService.CheckAllButtonActionPermissionAsync(buttonid, userid, roleid, controller, action);
                if (already.Result.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
        public int AlreadyExitsButtonPermissionNew(int buttonid, int userid, int roleid, int menuid)
        {
            int result = 0;
            try
            {
                var already = _buttonPermissionService.AlreadyExit(buttonid, userid, roleid, menuid);
                if (already.Result.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }

        public int getMaxSortNumber()
        {
            //List<ParentMenuModel> emp = new List<ParentMenuModel>();
            int result = 0;
            try
            {
                var emp = (from maxSort in _parentMenuService.GetAllParentMenuAsync().Result
                           select new SelectListItem()
                           {
                               Text = maxSort.UserName,
                               Value = maxSort.ParentMenuId.ToString(),
                           }).ToList();

            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }

        public string CreateSalt()
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
        public string ComputeSaltedHash(string password, string salt)
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

        public bool SendSMSWithTemplateId(string _MobileNo, string _Message, string _TemplateId)
        {
            bool rtr = true;
            try
            {

                //rtr = false;
                string result = apicall("https://vsms.minavo.in/api/singlesms.php?auth_key=0a321346-b4d5-4b82-a2ae-ba884f00e3a2"
                                                                    + "&mobilenumber="
                                                                    + _MobileNo + "&message="
                                                                    + _Message + ".&sid=CSJMUK&mtype=N&template_id="
                                                                    + _TemplateId);


                //string result = apicall("http://sms.fennecfoxsolutions.com/sms_api/sendsms.php?username=kanpur&password=kanpur@123"
                //                                                    + "&mobile="
                //                                                    + _MobileNo + "&message="
                //                                                    + _Message + "&sendername=CSJMUK&templateid="
                //                                                    + _TemplateId);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return rtr;
        }

        /// <summary>
        /// To send BULK SMS
        /// Devbrat Upadhyay
        /// 01/12/2018
        /// </summary>
        /// <param name="List<string> Mobile No"></param>
        /// <param name="_Message"></param>
        /// <returns></returns>
        public static bool SendBulkSMS(List<string> _mobileCollection, string _Message)
        {
            bool rtr = true;
            try
            {
                //add api
                rtr = false;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return rtr;
        }


        public string apicall(string url)
        {
            SMSModel sMSModel = new SMSModel();
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
                StreamReader sr = new StreamReader(httpres.GetResponseStream());
                string results = sr.ReadToEnd();
                sr.Close();

                sMSModel.APIResponse = "Response";
                sMSModel.APIURL = url;
                sMSModel.ServiceType = results;
                var result = _commonService.CreateSMSLogs(sMSModel);
                //DataRepository.Provider.ExecuteDataSet("cusp_SMSLogs_LogActivity", "Response", url, results);
                return results;
            }
            catch (Exception e)
            {
                sMSModel.APIResponse = "Response-Error";
                sMSModel.APIURL = url;
                sMSModel.ServiceType = e.Message;
                var result = _commonService.CreateSMSLogs(sMSModel);
                //DataRepository.Provider.ExecuteDataSet("cusp_SMSLogs_LogActivity", "Response-Error", url, e.Message);
                return "0";
            }
        }

        public string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public async Task<ActionResult> SendAutoEmailAsync()
        {
            int failedCount = 0;
            int successCount = 0;
            int finaldata = 0;
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            try
            {
                var data = (from upload in await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload()
                            where upload.PaperPath is null
                            select upload).ToList();
                foreach (var _data in data)
                {
                    var data1 = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(_data.PCPRegID));
                    pCPRegistrationModel.PCPRegID = data1.PCPRegID;
                    pCPRegistrationModel.UserId = data1.UserId;
                    pCPRegistrationModel.UserName = data1.UserName;
                    pCPRegistrationModel.MobileNo = data1.MobileNo;
                    pCPRegistrationModel.EmailID = data1.EmailID;
                    pCPRegistrationModel.IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    pCPRegistrationModel.CreatedBy = _data.CreatedBy;
                    pCPRegistrationModel.QPId = data1.QPId;
                    pCPRegistrationModel.AssignedQPId = data1.AssignedQPId;
                    #region Send email and sms
                    //send message
                    bool messageResult = false;
                    messageResult = SendRegistraionMeassage(pCPRegistrationModel.UserName, pCPRegistrationModel.MobileNo, pCPRegistrationModel.LoginID, pCPRegistrationModel.QPCode);
                    if (messageResult == false)
                    {
                        pCPRegistrationModel.IsMobileReminder = "N";
                    }
                    else
                    {
                        pCPRegistrationModel.IsMobileReminder = messageResult.ToString();
                    }

                    //send mail
                    bool emailResult = false;
                    emailResult = SendRegistraionMail(pCPRegistrationModel.UserName, pCPRegistrationModel.MobileNo, pCPRegistrationModel.LoginID, pCPRegistrationModel.Password);
                    if (emailResult == false)
                    {
                        pCPRegistrationModel.IsEmailReminder = "N";
                    }
                    else
                    {
                        pCPRegistrationModel.IsEmailReminder = emailResult.ToString();
                    }
                    #endregion
                     finaldata = await _pCPSendReminderService.CreateReminderAsync(pCPRegistrationModel);
                    if (finaldata.Equals(1))
                    {
                        successCount = successCount+1;

                    }
                    else
                    {
                        failedCount = failedCount+1;
                    }
                }
                //pCPRegistrationModel.failedCount = failedCount.ToString();
                //pCPRegistrationModel.successCount = successCount.ToString();
                ViewBag.successCount = successCount;
                ViewBag.failedCount = failedCount;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Common/SendAutoEmail.cshtml", pCPRegistrationModel);
        }

        public bool SendRegistraionMeassage(string username, string mobile, string loginid, string qpid)
        {
            bool result = false;
            string message = "Dear " + username + ", Attendance for Subject Codes: " + mobile + " is not entered today. Please specify reasons at " + loginid + " " + qpid + ". CSJMU, Kanpur";

            result = SendSMSWithTemplateId(mobile, message, "1607100000000228609");
           
            return result;
        }

        public bool SendRegistraionMail(string username, string email, string loginid, string password)
        {
            MailRequest request = new MailRequest();

            request.Subject = "Gentel Reminder!!";
            request.Body = "<br/> This is reminder mail" +
                   "<br/> Please upload paper as soon as possible.";
            //request.Attachments = "";
            request.ToEmail = email;
            return _mailService.SendEmailAsync(request);
        }
        //public int InsertEmailSmsRecordHistory(RegistrationModel registrationModel)
        //{
        //    int result = 0;
        //    var data = _registrationService.InsertEmailSMSHistory(registrationModel);
        //   if(data.Equals(1))
        //    {
        //        result = 1;
        //    }
        //    return result;
        //}

    }
}
