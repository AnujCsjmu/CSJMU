using ceTe.DynamicPDF.Cryptography;
using ceTe.DynamicPDF.Merger;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPUploadOldPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Paper Setter, Controller Of Examination")]
    public class PCPViewOldPaperController : Controller
    {
        private readonly ILogger<PCPViewOldPaperController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly IPCPUploadOldPaperService _pCPUploadOldPaperService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PCPViewOldPaperController(ILogger<PCPViewOldPaperController> logger, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPUploadOldPaperService pCPUploadOldPaperService, IHostingEnvironment environment)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPViewOldPaper.PCPViewOldPaperController");
            _pCPUploadOldPaperService = pCPUploadOldPaperService;
            hostingEnvironment = environment;
        }
        [HttpGet]
        //[AuthorizeContext(ViewAction.View)]
        [Obsolete]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var data = (from reg in await _pCPUploadOldPaperService.GetAllPCPUploadOldPaper()
                            where reg.FinalSubmit != null //&&  reg.QpAssignedUserId == CreatedBy
                            select reg).ToList();
                foreach (var _data in data)
                {
                    var stringId = _data.OldPaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end
                return View("~/Views/PCP/PCPViewOldPaper/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPViewOldPaper/Index.cshtml");
        }

        [Obsolete]
        public async Task<IActionResult> DownloadOldPaper(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadOldPaper");
                    var path = System.IO.Path.Combine(uploadsFolder, data.OldPaperPath);
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return await Index();
        }

        [Obsolete]
        public async Task<IActionResult> DownloadOldSyllabus(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadOldSyllabus");
                    var path = System.IO.Path.Combine(uploadsFolder, data.OldSyllabusPath);
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return await Index();
        }
        [Obsolete]
        public async Task<IActionResult> DownloadOldPattern(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadOldPattern");
                    var path = System.IO.Path.Combine(uploadsFolder, data.OldPatternPath);
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return await Index();
        }
    }
}
