using CoreLayout.Models.PCP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PCP
{

    public class PCPQPAndPaperReportController : Controller
    {
        private readonly ILogger<PCPQPAndPaperReportController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        public PCPQPAndPaperReportController(ILogger<PCPQPAndPaperReportController> logger, IPCPRegistrationService pCPRegistrationService, IPCPUploadPaperService pCPUploadPaperService, IPCPSendPaperService pCPSendPaperService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _pCPSendPaperService = pCPSendPaperService;
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
    }
}
