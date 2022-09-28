using ceTe.DynamicPDF.Cryptography;
using ceTe.DynamicPDF.Merger;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPUploadPaper;
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
    [Authorize(Roles = "Paper Setter")]
    public class PCPUploadPaperController : Controller
    {
        private readonly ILogger<PCPUploadPaperController> _logger;

        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly ICourseDetailsService _courseDetailsService;//for session
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        private readonly CommonController _commonController;

        [Obsolete]
        public PCPUploadPaperController(ILogger<PCPUploadPaperController> logger, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment, ICourseDetailsService courseDetailsService, IPCPAssignedQPService pCPAssignedQPService, CommonController commonController)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPUploadPaper.PCPUploadPaperController");
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
            _courseDetailsService = courseDetailsService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _commonController = commonController;
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
                //var data = await _pCPUploadPaperService.GetAllPCPUploadPaper();
                var data = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                            where reg.CreatedBy == CreatedBy
                            select reg).ToList();
                foreach (var _data in data)
                {
                    var stringId = _data.PaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.PaperId;
                }
                id = id + 1;
                ViewBag.MaxPaperId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/PCP/PCPUploadPaper/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadPaper/Index.cshtml");
        }
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Index(PCPUploadPaperModel pCPUploadPaperModel)
        {
            string paperid = Request.Form["paperid"];
            if(paperid!="")
            {
                //generate random pwd
                pCPUploadPaperModel.PaperRandomPassword = CreateRandomPassword();
                string aesencrytion = _commonController.Encrypt(pCPUploadPaperModel.PaperRandomPassword);
                pCPUploadPaperModel.PaperPassword = aesencrytion;
                pCPUploadPaperModel.paperids = paperid;
                var data1 = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(paperid));
                if (data1 != null)
                {
                    #region upload file in aes encrytion
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
                    string uploadsFolder1 = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaperEncrption");

                    string filePath = System.IO.Path.Combine(uploadsFolder, data1.PaperPath);
                    string filePath1 = System.IO.Path.Combine(uploadsFolder1, data1.PaperPath);

                    MergeDocument document = new MergeDocument(filePath);
                    Aes256Security security = new Aes256Security(pCPUploadPaperModel.PaperRandomPassword);
                    security.AllowCopy = false;
                    security.AllowPrint = false;
                    security.AllowFormFilling = false;
                    security.AllowEdit = false;
                    document.Security = security;
                    //insert file in other folder UploadPaperEncryption
                    document.Draw(filePath1);
                    //delete file from UploadPaper folder
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)//check file exsit or not.
                    {
                        file.Delete();
                    }
                    #endregion
                    var res = await _pCPUploadPaperService.FinalSubmitAsync(pCPUploadPaperModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Data has been final submit";
                    }
                    else
                    {
                        TempData["error"] = "Data has not been final submit";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Selected data is not available");
                    TempData["error"] = "Selected data is not available";
                }
            }
            else
            {
                ModelState.AddModelError("", "Select at least one checkbox");
                TempData["error"] = "Select at least one checkbox";
            }

            //start encrypt id for update,delete & details
            int id = (int)HttpContext.Session.GetInt32("UserId");
            //var data = await _pCPUploadPaperService.GetAllPCPUploadPaper();
            var data = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                        where reg.CreatedBy == id
                        select reg).ToList();
            foreach (var _data in data)
            {
                var stringId = _data.PaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);
            }
            //end
            return View("~/Views/PCP/PCPUploadPaper/Index.cshtml", data);
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Details)]
        [Obsolete]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id));

                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPUploadPaper/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
                pCPUploadPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadPaperModel.SessionList = await _courseDetailsService.GetAllSession();
                var data = (from qp in (await _pCPAssignedQPService.GetAllPCPAssignedQP())
                            where qp.UserId == CreatedBy
                            select qp).ToList();
                pCPUploadPaperModel.QPList = data;

                var guid_id = _protector.Unprotect(id);
                return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaperModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Add)]
        [Obsolete]
        public async Task<IActionResult> CreateAsync(PCPUploadPaperModel pCPUploadPaper)
        {
            //start check paper already uploaded
            int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
            var alreadyexit = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                               where reg.CreatedBy == CreatedBy && reg.QPId == pCPUploadPaper.QPId
                               select reg).ToList();
            //end

            pCPUploadPaper.CreatedBy = HttpContext.Session.GetInt32("UserId");
            pCPUploadPaper.IPAddress = HttpContext.Session.GetString("IPAddress");
            pCPUploadPaper.SessionList = await _courseDetailsService.GetAllSession();
            var data = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                        where qp.UserId == CreatedBy
                        select qp).ToList();
            pCPUploadPaper.QPList = data;
            if (pCPUploadPaper.UploadPaper != null)
            {
                //check file name is not null
               
                //var supportedTypes = new[] { "jpg", "jpeg", "pdf", "png", "JPG", "JPEG", "PDF", "PNG" };
                var supportedTypes = new[] { "pdf", "PDF" };
                var fileExt = System.IO.Path.GetExtension(pCPUploadPaper.UploadPaper.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ModelState.AddModelError("", "File Extension Is InValid - Only Upload PDF File");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                }
                else if (pCPUploadPaper.UploadPaper.FileName == null)
                {
                    ModelState.AddModelError("", "File name is blank");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                }
                else if (alreadyexit.Count > 0)
                {
                    ModelState.AddModelError("", "Paper already uploaded for this QP");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                }
                else
                {
                    var uniqueFileName = UploadedFile(pCPUploadPaper,"");
                    pCPUploadPaper.PaperPath = uniqueFileName;
                    if (ModelState.IsValid && uniqueFileName != null)
                    {
                        var res = await _pCPUploadPaperService.CreatePCPUploadPaperAsync(pCPUploadPaper);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Paper has been saved";
                        }
                        else
                        {
                            TempData["error"] = "Paper has not been saved";
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Some thing went wrong");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select file");
                return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
            }
        }

        //[AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id));
                data.SessionList = await _courseDetailsService.GetAllSession();
                var qpdata = (from qp in (await _pCPAssignedQPService.GetAllPCPAssignedQP())
                              where qp.UserId == CreatedBy
                              select qp).ToList();
                data.QPList = qpdata;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Edit)]
        [Obsolete]
        public async Task<IActionResult> Edit(int PaperId, PCPUploadPaperModel pCPUploadPaperModel)
        {
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                pCPUploadPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPUploadPaperModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadPaperModel.SessionList = await _courseDetailsService.GetAllSession();
                ////start pdf pwd encrypt
                //pCPUploadPaperModel.PaperPassword = _commonController.Encrypt(pCPUploadPaperModel.PaperPassword);
                ////end
                var qpdata = (from qp in (await _pCPAssignedQPService.GetAllPCPAssignedQP())
                              where qp.UserId == CreatedBy
                              select qp).ToList();
                pCPUploadPaperModel.QPList = qpdata;
                if (pCPUploadPaperModel.UploadPaper != null)
                {
                    var supportedTypes = new[] { "pdf", "PDF" };
                    var fileExt = System.IO.Path.GetExtension(pCPUploadPaperModel.UploadPaper.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("", "File Extension Is InValid - Only Upload PDF File");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            var value = await _pCPUploadPaperService.GetPCPUploadPaperById(PaperId);
                            var uniqueFileName = UploadedFile(pCPUploadPaperModel, value.PaperPath);
                            pCPUploadPaperModel.PaperPath = uniqueFileName;
                            
                            if (await TryUpdateModelAsync<PCPUploadPaperModel>(value))
                            {
                                var res = await _pCPUploadPaperService.UpdatePCPUploadPaperAsync(pCPUploadPaperModel);
                                if (res.Equals(1))
                                {
                                    TempData["success"] = "Paper has been updated";
                                }
                                else
                                {
                                    TempData["error"] = "Paper has not been updated";
                                }
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                else
                {
                    return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _pCPUploadPaperService.DeletePCPUploadPaperAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Paper has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Paper has not been deleted";
                    }
                }
                else
                {
                    TempData["error"] = "Some thing went wrong!";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        [Obsolete]
        private string UploadedFile(PCPUploadPaperModel model,string oldpath)
        {
            try
            {
                string uniqueFileName = null;

                if (model.UploadPaper != null)
                {

                    //start pdf pwd encrypt
                    //pCPUploadPaper.PaperPassword = _commonController.Encrypt(pCPUploadPaper.PaperPassword);
                    //end
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
                    string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    uniqueFileName = userid + "_" + datetime + "_" + model.UploadPaper.FileName;
                    string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);
                    string deletefilePath = System.IO.Path.Combine(uploadsFolder, oldpath);
                    if (System.IO.File.Exists(deletefilePath))
                    {
                        System.IO.File.Delete(deletefilePath);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.UploadPaper.CopyTo(fileStream);
                    }

                    //MergeDocument document = new MergeDocument();
                    //Page page = new Page(ceTe.DynamicPDF.PageSize.Letter, PageOrientation.Portrait);
                    //page.Elements.Add(new Label("Cover Page", 0, 0, 512, 12));
                    //Aes256Security security = new Aes256Security(model.PaperPassword);
                    //security.AllowCopy = false;
                    //security.AllowPrint = false;
                    //security.AllowFormFilling = false;
                    //document.Pages.Add(page);
                    //document.Security = security;
                    //document.Append(@"DocumentA.pdf");
                    //document.Draw(filePath);
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateRandomPassword(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        // Get mime types
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
    };
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
                        string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
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
                //TempData["error"] = "Some thing went wrong";
            }
            return await Index();
        }



        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(int qpId)
        {

            var already = (from data in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                           where data.QPId == qpId
                           select new SelectListItem()
                           {
                               Text = data.QPName,
                               Value = data.QPId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"Paper is already uploaded for this qp");
            }

            return Json(true);


        }

    }
}
