using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Circular;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator,Institute,Controller Of Examination,Paper Setter,Assistant Registrar,Examination AO,Paper Printing")]
    public class DashBoardController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<DashBoardController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        private readonly ICircularService _circularService;
        public IConfiguration _configuration;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public DashBoardController(ILogger<DashBoardController> logger, IDashboardService dashboardService, IPCPRegistrationService pCPRegistrationService, IPCPUploadPaperService pCPUploadPaperService, IPCPSendPaperService pCPSendPaperService, IPCPAssignedQPService pCPAssignedQPService, ICircularService circularService, IHostingEnvironment environment, IDataProtectionProvider provider, IConfiguration configuration)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _pCPSendPaperService = pCPSendPaperService;
            _circularService = circularService;
            hostingEnvironment = environment;
            _configuration = configuration;
            _protector = provider.CreateProtector("DashBoard.DashBoardController");
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

                    if (roleid == 3)
                    {
                        var registercount = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                                 //where reg.IsApproved == null
                                             select reg).ToList();
                        var approvecount = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                            where reg.IsApproved != null
                                            select reg).ToList();
                        var reject = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                      where reg.IsRecordDeleted == 1
                                      select reg).ToList();
                        var qpnotassigned = (from reg in await _pCPRegistrationService.GetReportQPAndPaperWise()
                                        where reg.QPCode is null
                                            select reg).ToList();
                        var qpallotted = (from reg in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                          select reg).ToList();
                        var paperupload = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                                           where reg.FinalSubmit !=null
                                           select reg).ToList();
                        var sendpaper = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                                         select reg).ToList();
                        var acceptPaper = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                                         where reg.AcceptedStatus!=null
                                         select reg).ToList();

                        ViewBag.RegistrationCount = registercount.Count.ToString();
                        ViewBag.ApprovedCount = approvecount.Count.ToString();
                        ViewBag.RejectCount = reject.Count.ToString();
                        ViewBag.QPNotAssigned = qpnotassigned.Count.ToString();
                        ViewBag.QPAllotmentCount = qpallotted.Count.ToString();
                        ViewBag.PaperUploadCount = paperupload.Count.ToString();
                        ViewBag.SendToAgencyCount =sendpaper.Count.ToString();
                        ViewBag.AcceptPaper = acceptPaper.Count.ToString();
                    }

                    #endregion

                    #region view circular for collage
                    if (roleid == 4) 
                    {
                        int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
                        if (SessionInstituteId != 0)
                        {
                            var circular = await _circularService.GetAllCircularByCollageId(SessionInstituteId);
                            foreach (var _data in circular)
                            {
                                var stringId = _data.CircularId.ToString();
                                _data.EncryptedId = _protector.Protect(stringId);
                            }
                            ViewBag.Circular = circular;
                        }
                    }
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

        [Obsolete]
        public async Task<IActionResult> Download(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _circularService.GetCircularById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    var path = string.Empty;
                    var ext = string.Empty;
                   
                        string CircularDocument = _configuration.GetSection("FilePaths:PreviousDocuments:Circular").Value.ToString();
                        path = Path.Combine(CircularDocument, data.CircularPath);
                        ext = Path.GetExtension(data.CircularPath).Substring(1);
                    
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    if (ext == "png" || ext == "PNG")
                    {
                        return File(FileBytes, "image/png");
                    }
                    else if (ext == "jpg" || ext == "JPG")
                    {
                        return File(FileBytes, "image/jpg");
                    }
                    else if (ext == "jpeg" || ext == "JPEG")
                    {
                        return File(FileBytes, "image/jpeg");
                    }
                    else
                    {
                        return File(FileBytes, "application/pdf");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

    }

}
