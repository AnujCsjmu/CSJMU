using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignRole;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Services.UserManagement.ParentMenu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public CommonController(ILogger<CommonController> logger, IDashboardService dashboardService, ICommonService commonService, IRegistrationService registrationService, IAssignRoleService assignRoleService, IParentMenuService parentMenuService, IButtonPermissionService buttonPermissionService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _commonService = commonService;
            _registrationService = registrationService;
            _assignRoleService = assignRoleService;
            _parentMenuService = parentMenuService;
            _buttonPermissionService = buttonPermissionService;
        }
        public async  Task<ActionResult> RefereshMenuAsync()
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
                if (already.Count>0)
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
        public int assignRoleAlreadyExits(int userid,int roleid)
        {
            int result = 0;
            try
            {
                var already = (from userRole in _assignRoleService.GetAllRoleAssignAsync().Result
                               where userRole.RoleUserId == userid && userRole.RoleId==roleid
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

        public int AlreadyExitsButtonPermission( int buttonid, int userid, int roleid, string controller, string action)
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
    }
}
