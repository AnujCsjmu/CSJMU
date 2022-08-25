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
using System.Linq;
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
                    if (dm.Level2.Equals("*") && dm.Level3.Equals("*"))
                    {
                        level1.Add(dm);
                    }
                    else if (dm.Level2 != "*" && dm.Level3.Equals("*"))
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
    }
}
