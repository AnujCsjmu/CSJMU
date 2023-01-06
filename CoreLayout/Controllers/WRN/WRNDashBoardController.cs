using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.WRN;
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
    //[Authorize(Roles = "Administrator,Institute,Sub-Institute,Controller Of Examination,Paper Setter,Assistant Registrar,Examination AO,Paper Printing")]
    public class WRNDashBoardController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<DashBoardController> _logger;
        public IConfiguration _configuration;
        public WRNDashBoardController(ILogger<DashBoardController> logger, IDataProtectionProvider provider, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _protector = provider.CreateProtector("WRNDashBoard.WRNDashBoardController");
        }
        [HttpGet]
        public IActionResult DashBoard()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNDashboard/Dashboard.cshtml", wRNRegistrationModel);
        }

        [HttpGet]
        public IActionResult CompleteRegistration()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNRegistration/CompleteRegistration.cshtml", wRNRegistrationModel);
        }

        [HttpGet]
        public IActionResult Qualification()
        {
            WRNQualificationModel wRNQualificationModel = new WRNQualificationModel();
            wRNQualificationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNQualification/Qualification.cshtml", wRNQualificationModel);
        }
    }

}
