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
    [Authorize(Roles = "Administrator,Institute,Controller Of Examination,Paper Setter")]
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
                    List<DashboardModel> alllevels = await _dashboardService.GetDashboardByRoleAndUser(roleid, userid);
                    HttpContext.Session.SetString("AllLevelList", JsonConvert.SerializeObject(alllevels));
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
