using ceTe.DynamicPDF.Cryptography;
using ceTe.DynamicPDF.Merger;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.PCP.PCPAssignedQP;
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
        //private readonly IExamCourseMappingService _examCourseMappingService;
        //private readonly ICourseBranchMappingService _courseBranchMappingService;
        private readonly IExamMasterService _examMasterService;
        private readonly IQPMasterService _qPMasterService;

        [Obsolete]
        public PCPUploadPaperController(ILogger<PCPUploadPaperController> logger, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment, ICourseDetailsService courseDetailsService, IPCPAssignedQPService pCPAssignedQPService, CommonController commonController
            //,IExamCourseMappingService examCourseMappingService, ICourseBranchMappingService courseBranchMappingService
            , IQPMasterService qPMasterService, IExamMasterService examMasterService)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPUploadPaper.PCPUploadPaperController");
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
            _courseDetailsService = courseDetailsService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _commonController = commonController;
            //_examCourseMappingService = examCourseMappingService;
            //_courseBranchMappingService = courseBranchMappingService;
            _qPMasterService = qPMasterService;
            _examMasterService = examMasterService;
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
            try
            {
                string paperid = Request.Form["paperid"];
                if (paperid != "")
                {
                    //generate random pwd
                    pCPUploadPaperModel.PaperRandomPassword = CreateRandomPassword();
                    string aesencrytion = _commonController.Encrypt(pCPUploadPaperModel.PaperRandomPassword);
                    pCPUploadPaperModel.PaperPassword = aesencrytion;
                    pCPUploadPaperModel.paperids = paperid;
                    pCPUploadPaperModel.CreatedBy= HttpContext.Session.GetInt32("UserId");
                    var data1 = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(paperid));
                    if (data1 != null)
                    {
                        #region upload question paper in aes encrytion
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

                        #region upload answer paper in aes encrytion
                        if (data1.AnswerPath != null)
                        {
                            string uploadsFolderAnswer = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "AnswerPaper");
                            string uploadsFolder1Answer = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "AnswerPaperEncryption");

                            string filePathAnswer = System.IO.Path.Combine(uploadsFolderAnswer, data1.AnswerPath);
                            string filePath1Answer = System.IO.Path.Combine(uploadsFolder1Answer, data1.AnswerPath);

                            MergeDocument document1 = new MergeDocument(filePathAnswer);
                            Aes256Security security1 = new Aes256Security(pCPUploadPaperModel.PaperRandomPassword);
                            security1.AllowCopy = false;
                            security1.AllowPrint = false;
                            security1.AllowFormFilling = false;
                            security1.AllowEdit = false;
                            document1.Security = security;
                            //insert file in other folder UploadPaperEncryption
                            document1.Draw(filePath1Answer);
                            //delete file from UploadPaper folder
                            FileInfo file1 = new FileInfo(filePathAnswer);
                            if (file1.Exists)//check file exsit or not.
                            {
                                file1.Delete();
                            }
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
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadPaper/Index.cshtml", pCPUploadPaperModel);
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
                //int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
                pCPUploadPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                //var data = (from qp in (await _pCPAssignedQPService.GetAllPCPAssignedQP())
                //            where qp.UserId == CreatedBy
                //            select qp).ToList();
                //pCPUploadPaperModel.QPList = data;

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
            try
            {
                //start check paper already uploaded
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");

                int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                var alreadyexit = (dynamic)null;
                if (RoleId != 19)
                { 
                  alreadyexit = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                                    where reg.AssignedQPId == pCPUploadPaper.AssignedQPId
                                    select reg).ToList();
                }
                else
                {
                    alreadyexit = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                                   where reg.CreatedBy == CreatedBy && reg.AssignedQPId == pCPUploadPaper.AssignedQPId
                                   select reg).ToList();
                }
                //end

                pCPUploadPaper.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadPaper.IPAddress = HttpContext.Session.GetString("IPAddress");
                //pCPUploadPaper.SessionList = await _courseDetailsService.GetAllSession();
                //var data = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                //            where qp.UserId == CreatedBy
                //            select qp).ToList();
                //pCPUploadPaper.QPList = data;
                pCPUploadPaper.ExamList = await _examMasterService.GetAllExamMasterAsync();
                if (pCPUploadPaper.UploadPaper != null)//&& pCPUploadPaper.AnswerPaper != null
                {
                    var supportedTypes = new[] { "pdf", "PDF" };
                    var fileExtQuestion = System.IO.Path.GetExtension(pCPUploadPaper.UploadPaper.FileName).Substring(1);
                    
                    //2nd file is not mandatory
                    if (pCPUploadPaper.AnswerPaper != null)
                    {
                        var fileExtAnswer = System.IO.Path.GetExtension(pCPUploadPaper.AnswerPaper.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExtAnswer))
                        {
                            ModelState.AddModelError("", "File Extension Is InValid of Answer Paper - Only Upload PDF File");
                            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                        }
                        if (pCPUploadPaper.AnswerPaper.Length > 2100000)
                        {
                            ModelState.AddModelError("", "Answer paper size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                        }
                        if (pCPUploadPaper.AnswerPaper.FileName == null)
                        {
                            ModelState.AddModelError("", "File name is blank of answer paper");
                            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                        }
                    }
                    if (!supportedTypes.Contains(fileExtQuestion))
                    {
                        ModelState.AddModelError("", "File Extension Is InValid of Question Paper - Only Upload PDF File");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }
                    if (pCPUploadPaper.UploadPaper.Length > 2100000)
                    {
                        ModelState.AddModelError("", "Question paper size must be less than 2 mb");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }
                    else if (pCPUploadPaper.UploadPaper.FileName == null)
                    {
                        ModelState.AddModelError("", "File name is blank of question paper");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }

                    else if (alreadyexit.Count > 0)
                    {
                        ModelState.AddModelError("", "Paper already uploaded for this QP");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }
                    else if (!ModelState.IsValid)
                    {
                        ModelState.AddModelError("", "Model state is not valid");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                    }

                    else
                    {
                        var uniqueFileNameQuestion = UploadedFile(pCPUploadPaper.UploadPaper, "", "UploadPaper");//fileupload,filename,foldername
                        string uniqueFileNameAnswer = null;
                        if (pCPUploadPaper.AnswerPaper != null)
                        {
                             uniqueFileNameAnswer = UploadedFile(pCPUploadPaper.AnswerPaper, "", "AnswerPaper");//fileupload,filename,foldername
                        }
                        if (uniqueFileNameQuestion != null)//&& uniqueFileNameAnswer != null
                        {
                            pCPUploadPaper.PaperPath = uniqueFileNameQuestion;
                            pCPUploadPaper.AnswerPath = uniqueFileNameAnswer;

                            var res = await _pCPUploadPaperService.CreatePCPUploadPaperAsync(pCPUploadPaper);
                            if (res.Equals(1))
                            {
                                TempData["success"] = "Paper has been saved";
                            }
                            else
                            {
                                TempData["error"] = "Paper has not been saved";
                                if (System.IO.File.Exists(pCPUploadPaper.PaperPath))
                                {
                                    System.IO.File.Delete(pCPUploadPaper.PaperPath);
                                }
                                if (System.IO.File.Exists(pCPUploadPaper.AnswerPath))
                                {
                                    System.IO.File.Delete(pCPUploadPaper.AnswerPath);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Some thing went wrong in file upload");
                            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please select file");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadPaper);
        }

        [Obsolete]
        private string UploadedFile(IFormFile model, string oldpath, string foldername)
        {
            try
            {
                string uniqueFileName = null;

                if (model != null)
                {
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername);
                    string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    uniqueFileName = userid + "_" + datetime + "_" + model.FileName;
                    string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);
                    //if folder not exit
                    if (!Directory.Exists(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername)))
                    {
                        Directory.CreateDirectory(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername));
                    }
                    //delete old file if edit
                    if (oldpath != null)
                    {
                        string deletefilePath = System.IO.Path.Combine(uploadsFolder, oldpath);
                        if (System.IO.File.Exists(deletefilePath))
                        {
                            System.IO.File.Delete(deletefilePath);
                        }
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.CopyTo(fileStream);
                    }
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw ex;
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

                data.QPList = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                               where qp.UserId == CreatedBy && qp.ExamId == data.ExamId
                               select qp).ToList();
                data.ExamList = (from exam in await _examMasterService.GetAllExamMasterAsync()
                                 where exam.ExamId == data.ExamId
                                 select exam).ToList();
                data.CourseList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                   where qpmaster.AssignedQPId == data.AssignedQPId
                                   select qpmaster).ToList();
                data.BranchList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                   where qpmaster.AssignedQPId == data.AssignedQPId
                                   select qpmaster).ToList();
                data.SessionList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                    where qpmaster.AssignedQPId == data.AssignedQPId
                                    select qpmaster).ToList();
                data.SemYearList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                    where qpmaster.AssignedQPId == data.AssignedQPId
                                    select qpmaster).ToList();

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
                var value = await _pCPUploadPaperService.GetPCPUploadPaperById(PaperId);

                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                //var alreadyexit = (from reg in await _pCPUploadPaperService.GetAllPCPUploadPaper()
                //                   where reg.CreatedBy == CreatedBy && reg.QPId == pCPUploadPaperModel.QPId
                //                   select reg).ToList();
                //end
                pCPUploadPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPUploadPaperModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");

                pCPUploadPaperModel.QPList = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                              where qp.UserId == CreatedBy && qp.ExamId == pCPUploadPaperModel.ExamId
                                              select qp).ToList();
                pCPUploadPaperModel.ExamList = (from exam in await _examMasterService.GetAllExamMasterAsync()
                                                where exam.ExamId == pCPUploadPaperModel.ExamId
                                                select exam).ToList();
                pCPUploadPaperModel.CourseList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                                  where qpmaster.AssignedQPId == pCPUploadPaperModel.AssignedQPId
                                                  select qpmaster).ToList();
                pCPUploadPaperModel.BranchList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                                  where qpmaster.AssignedQPId == pCPUploadPaperModel.AssignedQPId
                                                  select qpmaster).ToList();
                pCPUploadPaperModel.SessionList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                                   where qpmaster.AssignedQPId == pCPUploadPaperModel.AssignedQPId
                                                   select qpmaster).ToList();
                pCPUploadPaperModel.SemYearList = (from qpmaster in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                                   where qpmaster.AssignedQPId == pCPUploadPaperModel.AssignedQPId
                                                   select qpmaster).ToList();
                //Question paper upload
                var supportedTypes = new[] { "pdf", "PDF" };
                if (pCPUploadPaperModel.UploadPaperEdit != null)
                {
                    var fileExtQuestion = System.IO.Path.GetExtension(pCPUploadPaperModel.UploadPaperEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExtQuestion))
                    {
                        ModelState.AddModelError("", "File Extension Is InValid of Question Paper - Only Upload PDF File");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                    if (pCPUploadPaperModel.UploadPaperEdit.FileName == null)
                    {
                        ModelState.AddModelError("", "File name is blank of question paper");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                    var uniqueFileNameQuestion = UploadedFile(pCPUploadPaperModel.UploadPaperEdit, value.PaperPath, "UploadPaper");//fileupload,filename,foldername
                    if (uniqueFileNameQuestion != null)
                    {
                        pCPUploadPaperModel.PaperPath = uniqueFileNameQuestion;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Paper is not uploaded");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }

                }
                else
                {
                    pCPUploadPaperModel.PaperPath = value.PaperPath;
                }
                //answer paper upload
                if (pCPUploadPaperModel.AnswerPaperEdit != null)
                {
                    var fileExtAnswer = System.IO.Path.GetExtension(pCPUploadPaperModel.AnswerPaperEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExtAnswer))
                    {
                        ModelState.AddModelError("", "File Extension Is InValid of Answer Paper - Only Upload PDF File");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                    if (pCPUploadPaperModel.AnswerPaperEdit.FileName == null)
                    {
                        ModelState.AddModelError("", "File name is blank of answer paper");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                    var uniqueFileNameAnswer = UploadedFile(pCPUploadPaperModel.AnswerPaperEdit, value.AnswerPath, "AnswerPaper");//fileupload,filename,foldername
                    if (uniqueFileNameAnswer != null)
                    {
                        pCPUploadPaperModel.AnswerPath = uniqueFileNameAnswer;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Answer is not uploaded");
                        return View("~/Views/PCP/PCPUploadPaper/Edit.cshtml", pCPUploadPaperModel);
                    }
                }
                else
                {
                    pCPUploadPaperModel.AnswerPath = value.AnswerPath;
                }

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
        public async Task<IActionResult> DownloadPaperAsync(string id)
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
                    pCPUploadPaperModel.DownloadStatus = "Download Paper By Setter";
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

        [Obsolete]
        public async Task<IActionResult> DownloadAnswerAsync(string id)
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
                    pCPUploadPaperModel.PaperPath = data.AnswerPath;
                    pCPUploadPaperModel.DownloadStatus = "Download Answer By Setter";
                    var res = await _pCPUploadPaperService.InsertDownloadLogAsync(pCPUploadPaperModel);
                    //end

                    if (res.Equals(1))
                    {
                        //TempData["success"] = "Paper has been downloaded";

                        #region file download
                        string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "AnswerPaper");
                        var path = System.IO.Path.Combine(uploadsFolder, data.AnswerPath);
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
                           where data.AssignedQPId == qpId
                           select new SelectListItem()
                           {
                               Text = data.QPName,
                               Value = data.AssignedQPId.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"Paper is already uploaded for this qp");
            }

            return Json(true);


        }

        public async Task<JsonResult> GetCourseSubjectSemYearSyl(int AssignedQPId)
        {
            var Combinelist = (from assignqp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                               where assignqp.AssignedQPId == AssignedQPId
                               select assignqp).ToList();
            return Json(Combinelist);
        }
        public async Task<JsonResult> GetQP(int ExamId)
        {
            int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
            int RoleId = (int)HttpContext.Session.GetInt32("RoleId");
            var QPList = (dynamic)null;
            if (RoleId != 19)
            {
                QPList = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                          where qp.ExamId == ExamId
                          select new SelectListItem()
                          {
                              Text = qp.QPCode + " - " + qp.QPName,
                              Value = qp.AssignedQPId.ToString(),
                          }).ToList();
            }
            else
            {
                 QPList = (from qp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                              where qp.UserId == CreatedBy && qp.ExamId == ExamId
                              select new SelectListItem()
                              {
                                  Text = qp.QPCode + " - " + qp.QPName,
                                  Value = qp.AssignedQPId.ToString(),
                              }).ToList();
            }
            QPList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(QPList);
        }
    }
}
