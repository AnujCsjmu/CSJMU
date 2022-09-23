using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Common;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPApproval;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        [Obsolete]
        public PCPViewPaperByAgencyController(ILogger<PCPViewPaperByAgencyController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor,  IDataProtectionProvider provider, IPCPSendPaperService pCPSendPaperService, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPViewPaperByAgency.PCPViewPaperByAgencyController");
            _pCPSendPaperService = pCPSendPaperService;
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                int? id = HttpContext.Session.GetInt32("UserId");
                var data = (from reg in await  _pCPSendPaperService.GetAllPCPSendPaper()
                            where reg.AgencyId == id
                            select reg).ToList();

                //start encrypt id for update, delete & details
                foreach (var _data in data)
                    {
                        var stringId = _data.PaperId.ToString();
                        _data.EncryptedId = _protector.Protect(stringId);
                    }
                //end
                return View("~/Views/PCP/PCPViewPaperByAgency/Index.cshtml", data);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPViewPaperByAgency/Index.cshtml");
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
                    //insert download record
                    PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
                    pCPUploadPaperModel.CreatedBy= HttpContext.Session.GetInt32("UserId");
                    pCPUploadPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    pCPUploadPaperModel.PaperId = data.PaperId;
                    pCPUploadPaperModel.PaperPath = data.PaperPath;
                    pCPUploadPaperModel.DownloadStatus = "Download";
                    var res = await _pCPUploadPaperService.InsertDownloadLogAsync(pCPUploadPaperModel);
                    //end
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Paper has been downloaded";

                        #region file download
                        string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
                        var path = System.IO.Path.Combine(uploadsFolder, data.PaperPath);
                        byte[] bytes = System.IO.File.ReadAllBytes(path);
                        using (MemoryStream inputData = new MemoryStream(bytes))
                        {
                            using (MemoryStream outputData = new MemoryStream())
                            {
                                string PDFFilepassword = data.PaperPassword;
                                PdfReader reader = new PdfReader(inputData);
                                PdfReader.unethicalreading = true;
                                PdfEncryptor.Encrypt(reader, outputData, true, PDFFilepassword, PDFFilepassword, PdfWriter.ALLOW_SCREENREADERS);
                                bytes = outputData.ToArray();
                                return File(bytes, "application/pdf");
                            }
                        }
                        #endregion
                        
                    }
                    else
                    {
                        TempData["error"] = "Paper has not been downloaded";
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
