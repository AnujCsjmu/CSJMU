using CoreLayout.Models.PCP;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Controller Of Examination")]
    public class PCPQPAndPaperReportController : Controller
    {
        private readonly ILogger<PCPQPAndPaperReportController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        private readonly IDataProtector _protector;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PCPQPAndPaperReportController(ILogger<PCPQPAndPaperReportController> logger, IPCPRegistrationService pCPRegistrationService, IPCPUploadPaperService pCPUploadPaperService, IPCPSendPaperService pCPSendPaperService, IPCPAssignedQPService pCPAssignedQPService,IDataProtectionProvider provider, IHostingEnvironment environment)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _pCPSendPaperService = pCPSendPaperService;
            _pCPAssignedQPService = pCPAssignedQPService;
            hostingEnvironment = environment;
            _protector = provider.CreateProtector("PCPQPAndPaperReport.PCPQPAndPaperReportController");
        }
        public async Task<IActionResult> IndexAsync()
        {
            PCPSendPaperModel pCPSendPaperModel = new PCPSendPaperModel();
            List<PCPSendPaperModel> list = new List<PCPSendPaperModel>();
            List<List<PCPSendPaperModel>> list2 = new List<List<PCPSendPaperModel>>();
            var data = await _pCPRegistrationService.GetReportQPAndPaperWise();
            foreach (var _data in data)
            {
                var data1 = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                             where reg.PaperId == _data.PaperId && reg.QPId == _data.QPId
                             select reg).ToList();
                //foreach (var _data1 in data1)
                //{
                //    pCPSendPaperModel.AgencyId = _data1.AgencyId;
                //    pCPSendPaperModel.PaperId = _data1.PaperId;
                //    pCPSendPaperModel.QPId = _data1.QPId;
                //    list.Add(pCPSendPaperModel);
                //}
                list2.Add(data1);
            }

            ViewBag.list = list2;
            //start encrypt
            foreach (var _data in data)
            {
                var stringId = _data.PaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);
            }
            //end

            //start for report
            var registercount = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                        //where reg.IsApproved == null
                        select reg).ToList();
            var approvecount = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                 where reg.IsApproved != null
                                 select reg).ToList();
            var reject = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                 where reg.IsRecordDeleted == 1
                                 select reg).ToList();
            var qpallotted = (from reg in (await _pCPAssignedQPService.GetAllPCPAssignedQP())
                          select reg).ToList();
            var paperupload = (from reg in (await _pCPUploadPaperService.GetAllPCPUploadPaper())
                               where reg.FinalSubmit!=null
                              select reg).ToList();
            var sendpaper = (from reg in (await _pCPSendPaperService.GetAllPCPSendPaper())
                               select reg).ToList();
            var AcceptPaper = (from reg in (await _pCPSendPaperService.GetAllPCPSendPaper())
                               where reg.AcceptedStatus!=null
                             select reg).ToList();

            ViewBag.RegistrationCount = registercount.Count.ToString();
            ViewBag.ApprovedCount = approvecount.Count.ToString();
            ViewBag.RejectCount = reject.Count.ToString();
            ViewBag.QPAllotmentCount = qpallotted.Count.ToString();
            ViewBag.PaperUploadCount = paperupload.Count.ToString();
            ViewBag.SendToAgencyCount = sendpaper.Count.ToString();
            ViewBag.AcceptPaper = AcceptPaper.Count.ToString();
            //end report

            //var data1= await _pCPSendPaperService.GetAllPCPSendPaper();
            //foreach (var _data in data)
            //{
            //    foreach (var _data1 in data1)
            //    {
            //        if(_data.PaperId==_data1.PaperId)
            //        {
            //            pCPSendPaperModel.AgencyId = _data1.AgencyId;
            //            pCPSendPaperModel.PaperId = _data1.PaperId;
            //            list.Add(pCPSendPaperModel);
            //        }
            //    }
            //    list2.Add(list);
            //}
            //ViewBag.list = list2;
            return View("~/Views/PCP/PCPReport/Index.cshtml", data);
        }

        [Obsolete]
        public async Task<IActionResult> DownloadAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaperEncrption");
                    var path = System.IO.Path.Combine(uploadsFolder, data.PaperPath);
                    //string dycriptpassword = _commonController.Decrypt(data.PaperPassword);
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
