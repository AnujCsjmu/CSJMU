using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
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
    [Authorize(Roles = "Administrator,Institute,Controller Of Examination,Paper Setter,QPAssign,Paper Printing")]
    public class DashBoardController : Controller
    {

        private readonly ILogger<DashBoardController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        public DashBoardController(ILogger<DashBoardController> logger, IDashboardService dashboardService, IPCPRegistrationService pCPRegistrationService, IPCPUploadPaperService pCPUploadPaperService, IPCPSendPaperService pCPSendPaperService, IPCPAssignedQPService pCPAssignedQPService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _pCPSendPaperService = pCPSendPaperService;
        }
        [HttpGet]
        //[AuthorizeContext(ViewAction.View)]
        public async Task<ActionResult> IndexAsync()
        {
            //var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
            {
                int userid = (int)HttpContext.Session.GetInt32("UserId");
                int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                if (roleid != 0 && userid != 0)
                {
                    #region pcp deshboard report
                    //start for report


                    var registercount = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                             //where reg.IsApproved == null
                                         select reg).ToList();
                    var approvecount = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                        where reg.IsApproved != null
                                        select reg).ToList();
                    var reject = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                  where reg.IsRecordDeleted == 1
                                  select reg).ToList();
                    var qpallotted = (from reg in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                      select reg).ToList();
                    var paperupload = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                                       select reg).ToList();
                    var sendpaper = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                                     select reg).ToList();

                    ViewBag.RegistrationCount = registercount.Count.ToString();
                    ViewBag.ApprovedCount = approvecount.Count.ToString();
                    ViewBag.RejectCount = reject.Count.ToString();
                    ViewBag.QPAllotmentCount = qpallotted.Count.ToString();
                    ViewBag.PaperUploadCount = paperupload.Count.ToString();
                    ViewBag.SendToAgencyCount = sendpaper.Count.ToString();

                    #endregion
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
