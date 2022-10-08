using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPDetailsReport;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Controller Of Examination")]
    public class PCPDetailsReportController : Controller
    {
        private readonly IPCPDetailsReportService _pCPDetailsReportService;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        public PCPDetailsReportController(IPCPDetailsReportService pCPDetailsReportService, IPCPRegistrationService pCPRegistrationService,
            IPCPAssignedQPService pCPAssignedQPService, IPCPUploadPaperService pCPUploadPaperService, IPCPSendPaperService pCPSendPaperService)
        {
            _pCPDetailsReportService = pCPDetailsReportService;
            _pCPRegistrationService = pCPRegistrationService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _pCPSendPaperService = pCPSendPaperService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var data = await _pCPDetailsReportService.GetAllAsync();
                return View("~/Views/PCP/PCPDetailsReport/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View("~/Views/PCP/PCPDetailsReport/Index.cshtml");
            }
        }

        public async Task<IActionResult> ViewReminder(string link, int? examId, int courseId, int subjectId)
        {
            try
            {
                var data = (dynamic)null;
                if(link== "Registration")
                {
                     data = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                where reg.CourseID == courseId && reg.BranchId == subjectId
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/RegisterReport.cshtml", data);
                }
                else if (link == "Approved")
                {
                     data = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                where reg.CourseID == courseId && reg.BranchId == subjectId && reg.IsApproved != null
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/ApprovedReport.cshtml", data);
                }
                else if (link == "QPAssigned")
                {
                     data = (from reg in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                where reg.ExamId==examId && reg.CourseId == courseId && reg.BranchId == subjectId
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/QPAssignedReport.cshtml", data);
                }
                else if (link == "PaperUpload")
                {
                     data = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                                where reg.ExamId == examId && reg.CourseId == courseId && reg.BranchId == subjectId && reg.FinalSubmit != null
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/PaperUploadReport.cshtml", data);
                }
                else if (link == "SendAgency")
                {
                     data = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                                //where reg.ExamId == examId && reg.CourseId == courseId && reg.BranchId == subjectId && reg.AcceptedStatus is null
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/SendToAgencyReport.cshtml", data);
                }
                else if (link == "AcceptAgency")
                {
                     data = (from reg in await _pCPSendPaperService.GetAllPCPSendPaper()
                                where  reg.AcceptedStatus != null
                                select reg).ToList();
                    return View("~/Views/PCP/PCPDetailsReport/AgencyAcceptReport.cshtml", data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
