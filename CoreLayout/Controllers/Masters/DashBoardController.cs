using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator,Institute")]
    public class DashBoardController : Controller
    {
       
        private readonly ILogger<DashBoardController> _logger;
        private readonly IDashboardService _dashboardService;
        public DashBoardController(ILogger<DashBoardController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<ActionResult> IndexAsync()
        {
            //var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
            {
                int userid = (int)HttpContext.Session.GetInt32("UserId");
                int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                if (roleid != 0 && userid != 0)
                {
                    //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);

                    //List<DashboardModel> alllevels = await _dashboardService.GetDashboardByRole(role);
                    List<DashboardModel> alllevels = await _dashboardService.GetDashboardByRoleAndUser(roleid, userid);

                    //List<DashboardModel> level1 = new List<DashboardModel>();
                    // List<DashboardModel> level2 = new List<DashboardModel>();
                    //List<DashboardModel> level3 = new List<DashboardModel>();

                    //foreach (DashboardModel dm in alllevels)
                    //{
                    //    if (dm.SubMenuName.Equals("*") && dm.MenuName.Equals("*"))
                    //    {
                    //        level1.Add(dm);
                    //    }
                    //    else if (dm.SubMenuName != "*" && dm.MenuName.Equals("*"))
                    //    {
                    //        level2.Add(dm);
                    //    }
                    //    else
                    //    {
                    //        level3.Add(dm);
                    //    }
                    //}

                    HttpContext.Session.SetString("AllLevelList", JsonConvert.SerializeObject(alllevels));
                    //HttpContext.Session.SetString("Level2List", JsonConvert.SerializeObject(level2));
                    //HttpContext.Session.SetString("Level3List", JsonConvert.SerializeObject(level3));
                    //ViewBag.AllLevelList = alllevels;
                   
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        //[HttpGet]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login", "Home");
        //}

    }

}
