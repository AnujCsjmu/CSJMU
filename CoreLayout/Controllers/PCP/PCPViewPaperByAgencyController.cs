using CoreLayout.Models.PCP;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Paper Printing")]
    public class PCPViewPaperByAgencyController : Controller
    {
        private readonly ILogger<PCPViewPaperByAgencyController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload
        private readonly CommonController _commonController;
        public IConfiguration _configuration;
        [Obsolete]
        public PCPViewPaperByAgencyController(ILogger<PCPViewPaperByAgencyController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPSendPaperService pCPSendPaperService, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment, CommonController commonController, IConfiguration configuration)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPViewPaperByAgency.PCPViewPaperByAgencyController");
            _pCPSendPaperService = pCPSendPaperService;
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
            _commonController = commonController;
            _configuration = configuration;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                int? id = HttpContext.Session.GetInt32("UserId");
                var data = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                            where reg.AgencyId == id
                            select reg).ToList();

                //start encrypt id for update, delete & details
                List<string> pcslist = new List<string>();
                foreach (var _data in data)
                {
                    var stringId = _data.SendPaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);

                    //for decrypt pwd
                    if (_data.PaperPassword != null)
                    {
                        _data.DecryptPassword = _commonController.Decrypt(_data.PaperPassword);
                        pcslist.Add(_data.DecryptPassword);
                    }
                }
                ViewBag.EncryptPwdList = pcslist;
                //end
                return View("~/Views/PCP/PCPViewPaperByAgency/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPViewPaperByAgency/Index.cshtml");
        }


        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Index(PCPSendPaperModel pCPSendPaperModel)
        {
            string sendpaperid = Request.Form["sendpaperid"];
            if (sendpaperid != "")
            {
                pCPSendPaperModel.sendpaperids = sendpaperid;
                //var data1 = await _pCPSendPaperService.GetPCPSendPaperById(Convert.ToInt32(sendpaperid));
                //if (data1 != null)
               // {
                    var res = await _pCPSendPaperService.FinalSubmitAsync(pCPSendPaperModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Data has been accepted";
                    }
                    else
                    {
                        TempData["error"] = "Data has not been not accepted";
                    }
                //}
               // else
               // {
               ///     ModelState.AddModelError("", "Selected data is not available");
               //     TempData["error"] = "Selected data is not available";
               // }
            }
            else
            {
                ModelState.AddModelError("", "Select at least one checkbox");
                TempData["error"] = "Select at least one checkbox";
            }

            int? id = HttpContext.Session.GetInt32("UserId");
            var data = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                        where reg.AgencyId == id
                        select reg).ToList();

            //start encrypt id for update, delete & details
            List<string> pcslist = new List<string>();
            foreach (var _data in data)
            {
                var stringId = _data.SendPaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);

                //for decrypt pwd
                if (_data.PaperPassword != null)
                {
                    _data.DecryptPassword = _commonController.Decrypt(_data.PaperPassword);
                    pcslist.Add(_data.DecryptPassword);
                }
            }
            ViewBag.EncryptPwdList = pcslist;
            return View("~/Views/PCP/PCPViewPaperByAgency/Index.cshtml", data);
        }

        [Obsolete]
        public async Task<IActionResult> DownloadAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                //var data = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id)); // get paer details for download
                var data = await _pCPSendPaperService.GetPCPSendPaperById(Convert.ToInt32(guid_id));//get peper open time and static ip
                var data1 = await _pCPSendPaperService.GetServerDateTime();//get server datetime

                string getIPAddres = HttpContext.Session.GetString("IPAddress");
                if (data ==null && data1==null)
                {
                    return NotFound();
                }
                else if (data1.ServerDateTime <= data.PaperOpenTime)
                {
                    TempData["error"] = "Paper open time is not started";
                    return RedirectToAction(nameof(Index));
                }
                else if (getIPAddres != data.StaticIPAddress)
                {
                    TempData["error"] = "Static IP is not matched";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //insert download record
                    PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
                    pCPUploadPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    pCPUploadPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    pCPUploadPaperModel.PaperId = data.PaperId;
                    pCPUploadPaperModel.PaperPath = data.PaperPath;
                    pCPUploadPaperModel.DownloadStatus = "Download";
                    var res = await _pCPUploadPaperService.InsertDownloadLogAsync(pCPUploadPaperModel);
                    //end
                    if (res.Equals(1))
                    {
                        //TempData["success"] = "Paper has been downloaded";

                        #region file download
                        // string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
                        string uploadsFolder = _configuration.GetSection("FilePaths:PreviousDocuments:UploadPaper").Value.ToString();
                        var path = System.IO.Path.Combine(uploadsFolder, data.PaperPath);
                        //string dycriptpassword = _commonController.Decrypt(data.PaperPassword);
                        string ReportURL = path;
                        byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                        return File(FileBytes, "application/pdf");
                        #endregion

                    }
                    else
                    {
                        //TempData["error"] = "Paper has not been downloaded";
                    }

                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
            }
            return await IndexAsync();
        }
    }
}
