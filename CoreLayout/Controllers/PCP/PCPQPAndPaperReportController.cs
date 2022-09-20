using CoreLayout.Services.PCP.PCPRegistration;
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
        public PCPQPAndPaperReportController(ILogger<PCPQPAndPaperReportController> logger, IPCPRegistrationService pCPRegistrationService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var data = await _pCPRegistrationService.GetReportQPAndPaperWise();

            return View("~/Views/PCP/PCPRegistration/Index.cshtml", data);
        }
    }
}
