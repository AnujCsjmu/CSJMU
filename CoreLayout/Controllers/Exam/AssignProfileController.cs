using CoreLayout.Models.Exam;
using CoreLayout.Services.Exam.SubjectProfile;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.Exam
{
    [Authorize(Roles = "Administrator")]
    public class AssignProfileController : Controller
    {
        private readonly ILogger<AssignProfileController> _logger;
        private readonly IDataProtector _protector;
        private readonly ISubjectProfileService _subjectProfileService;
        private readonly IQPMasterService _qPMasterService;
        public AssignProfileController(ILogger<AssignProfileController> logger, IDataProtectionProvider provider, ISubjectProfileService subjectProfileService, IQPMasterService qPMasterService)
        {
            _logger = logger;
            _subjectProfileService = subjectProfileService;
            _qPMasterService = qPMasterService;
            _protector = provider.CreateProtector("AssignProfile.AssignProfileController");
        }
        public async Task<IActionResult> IndexAsync(int? subjectprofileid)
        {
            var data = (dynamic)null;
            data = await _subjectProfileService.GetAllStudent();
            ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
            return View("~/Views/Exam/AssignProfile/Index.cshtml", data);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(AssignProfileModel assignProfileModel)
        {
            var data = (dynamic)null;
            data = await _subjectProfileService.GetAllStudent();
            ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
            return View("~/Views/Exam/AssignProfile/Index.cshtml", data);
        }
    }
}
