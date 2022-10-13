using ceTe.DynamicPDF.Cryptography;
using ceTe.DynamicPDF.Merger;
using CoreLayout.Helper;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Assistant Registrar,Examination AO, Controller Of Examination")]
    public class PCPUploadOldPaperController : Controller
    {
        private readonly ILogger<PCPUploadOldPaperController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly IPCPUploadOldPaperService _pCPUploadOldPaperService;
        private readonly ICourseDetailsService _courseDetailsService;//for session
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload
        private readonly IQPMasterService _qPMasterService;
        private readonly IExamMasterService _examMasterService;
        private readonly IExamCourseMappingService _examCourseMappingService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        public IConfiguration _configuration;
        [Obsolete]
        public PCPUploadOldPaperController(ILogger<PCPUploadOldPaperController> logger, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment, ICourseDetailsService courseDetailsService, IPCPAssignedQPService pCPAssignedQPService
            , IQPMasterService qPMasterService, IPCPUploadOldPaperService pCPUploadOldPaperService, IExamMasterService examMasterService,
            IExamCourseMappingService examCourseMappingService, ICourseBranchMappingService courseBranchMappingService, IConfiguration configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("PCPUploadOldPaper.PCPUploadOldPaperController");
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
            _courseDetailsService = courseDetailsService;
            _pCPAssignedQPService = pCPAssignedQPService;
            _qPMasterService = qPMasterService;
            _pCPUploadOldPaperService = pCPUploadOldPaperService;
            _examMasterService = examMasterService;
            _examCourseMappingService = examCourseMappingService;
            _courseBranchMappingService = courseBranchMappingService;
            _configuration = configuration;
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
                            where reg.CreatedBy == CreatedBy
                            select reg).ToList();
                foreach (var _data in data)
                {
                    var stringId = _data.OldPaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.OldPaperId;
                }
                id = id + 1;
                ViewBag.MaxOldPaperId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/PCP/PCPUploadOldPaper/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadOldPaper/Index.cshtml");
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Index(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            string oldpaperid = Request.Form["paperid"];
            if (oldpaperid != "")
            {
                pCPUploadOldPaperModel.oldpaperids = oldpaperid;
                pCPUploadOldPaperModel.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                //var data1 = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(oldpaperid));
                //if (data1 != null)
                //{
                var res = await _pCPUploadOldPaperService.FinalSubmitAsync(pCPUploadOldPaperModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Data has been final submit";
                }
                else
                {
                    TempData["error"] = "Data has not been final submit";
                }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "data is not available");
                //}
            }
            else
            {
                ModelState.AddModelError("", "Select at least one checkbox");
            }
            //start encrypt id for update,delete & details
            int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
            var data = (from reg in await _pCPUploadOldPaperService.GetAllPCPUploadOldPaper()
                        where reg.CreatedBy == CreatedBy
                        select reg).ToList();
            foreach (var _data in data)
            {
                var stringId = _data.OldPaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);
            }
            //end
            return View("~/Views/PCP/PCPUploadOldPaper/Index.cshtml", data);
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Details)]
        [Obsolete]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));

                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPUploadOldPaper/Details.cshtml", data);

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
                PCPUploadOldPaperModel pCPUploadOldPaperModel = new PCPUploadOldPaperModel();
                pCPUploadOldPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadOldPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();

                var guid_id = _protector.Unprotect(id);
                return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
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
        public async Task<IActionResult> CreateAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            string UploadOldPaperDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPaper").Value.ToString();
            string UploadOldPatternDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPattern").Value.ToString();
            string UploadOldSyllabusDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldSyllabus").Value.ToString();
            string UploadCertificateDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadCertificate").Value.ToString();
            var supportedTypes = new[] { "pdf", "PDF", "jpg", "JPG", "jpeg", "JPEG", "png", "PNG" };
            FileHelper fileHelper = new FileHelper();
            try
            {
                //start check paper already uploaded
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var alreadyexit = (from reg in await _pCPUploadOldPaperService.GetAllPCPUploadOldPaper()
                                   where reg.CreatedBy == CreatedBy && reg.QPId == pCPUploadOldPaperModel.QPId
                                   select reg).ToList();
                if (alreadyexit.Count > 0)
                {
                    ModelState.AddModelError("", "Data already exit");
                    return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
                //end

                #region Previous paper
                if (pCPUploadOldPaperModel.FUOldPape != null)
                {
                    var oldPaperExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldPape.FileName).Substring(1);
                    if (supportedTypes.Contains(oldPaperExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldPape.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldPaperPath = fileHelper.SaveFile(UploadOldPaperDocument, "", pCPUploadOldPaperModel.FUOldPape);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous Paper size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous paper extension is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Please upload previous paper");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
                #endregion
                #region previous syllabus
                if (pCPUploadOldPaperModel.FUOldSyllabus != null)
                {
                    var oldSyllabusExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldSyllabus.FileName).Substring(1);
                    if (supportedTypes.Contains(oldSyllabusExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldSyllabus.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldSyllabusPath = fileHelper.SaveFile(UploadOldSyllabusDocument, "", pCPUploadOldPaperModel.FUOldSyllabus);
                        }
                        else
                        {
                            fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                            ModelState.AddModelError("", "previous syllabus size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                        ModelState.AddModelError("", "previous syllabus extension is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }

                }
                else
                {
                    fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                    ModelState.AddModelError("", "Please upload previous syllabus");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
                #endregion
                #region prevois pattern             
                if (pCPUploadOldPaperModel.FUOldPattern != null)
                {
                    var oldPatternExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldPattern.FileName).Substring(1);
                    if (supportedTypes.Contains(oldPatternExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldPattern.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldPatternPath = fileHelper.SaveFile(UploadOldPatternDocument, "", pCPUploadOldPaperModel.FUOldPattern);
                        }
                        else
                        {
                            fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                            fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                            ModelState.AddModelError("", "previous pattern size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                        fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                        ModelState.AddModelError("", "previous pattern extension is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                    fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                    ModelState.AddModelError("", "Please upload previous pattern");
                    return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
                #endregion
                #region previous certificate not mandator
                if (pCPUploadOldPaperModel.FUCertificate != null)
                {
                    var oldCertificateExt = Path.GetExtension(pCPUploadOldPaperModel.FUCertificate.FileName).Substring(1);
                    if (supportedTypes.Contains(oldCertificateExt))
                    {
                        if (pCPUploadOldPaperModel.FUCertificate.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.CertificatePath = fileHelper.SaveFile(UploadCertificateDocument, "", pCPUploadOldPaperModel.FUCertificate);
                        }
                        else
                        {
                            fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                            fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                            fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                            ModelState.AddModelError("", "previous certificate size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                        fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                        fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                        ModelState.AddModelError("", "previous certificate is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }

                }
                #endregion

                pCPUploadOldPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadOldPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPUploadOldPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();

                if (pCPUploadOldPaperModel.OldPaperPath != null && pCPUploadOldPaperModel.OldPatternPath != null && pCPUploadOldPaperModel.OldSyllabusPath != null)
                {
                    if (ModelState.IsValid)
                    {
                        var res = await _pCPUploadOldPaperService.CreatePCPUploadOldPaperAsync(pCPUploadOldPaperModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Documents has been saved";
                        }
                        else
                        {
                            fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                            fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                            fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                            fileHelper.DeleteFileAnyException(UploadCertificateDocument, pCPUploadOldPaperModel.CertificatePath);
                            TempData["error"] = "Documents has not been saved";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                        fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                        fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                        fileHelper.DeleteFileAnyException(UploadCertificateDocument, pCPUploadOldPaperModel.CertificatePath);
                        ModelState.AddModelError("", "Model state is not valid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                    fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                    fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                    fileHelper.DeleteFileAnyException(UploadCertificateDocument, pCPUploadOldPaperModel.CertificatePath);
                    ModelState.AddModelError("", "Some thing went wrong in file upload");
                    return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
            }
            catch (Exception ex)
            {
                fileHelper.DeleteFileAnyException(UploadOldPaperDocument, pCPUploadOldPaperModel.OldPaperPath);
                fileHelper.DeleteFileAnyException(UploadOldSyllabusDocument, pCPUploadOldPaperModel.OldSyllabusPath);
                fileHelper.DeleteFileAnyException(UploadOldPatternDocument, pCPUploadOldPaperModel.OldPatternPath);
                fileHelper.DeleteFileAnyException(UploadCertificateDocument, pCPUploadOldPaperModel.CertificatePath);
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
        }

        //[Obsolete]
        //private string UploadedFile(IFormFile model, string oldpath, string foldername)
        //{
        //    try
        //    {
        //        string uniqueFileName = null;

        //        if (model != null)
        //        {
        //            string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername);
        //            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //            int userid = (int)HttpContext.Session.GetInt32("UserId");
        //            uniqueFileName = userid + "_" + datetime + "_" + model.FileName;
        //            string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);
        //            string deletefilePath = System.IO.Path.Combine(uploadsFolder, oldpath);
        //            //if folder not exit
        //            if (!Directory.Exists(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername)))
        //            {
        //                Directory.CreateDirectory(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername));
        //            }
        //            //delete old file if edit the record
        //            if (System.IO.File.Exists(deletefilePath))
        //            {
        //                System.IO.File.Delete(deletefilePath);
        //            }
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                model.CopyTo(fileStream);
        //            }
        //        }
        //        return uniqueFileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var guid_id = _protector.Unprotect(id);
            var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");

                data.ExamList = await _examMasterService.GetAllExamMasterAsync();

                data.CourseList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                   where examcoursemapping.ExamId == data.ExamId
                                   select examcoursemapping).ToList();

                data.SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcoursemapping.CourseID == data.CourseId
                                    select examcoursemapping).ToList();

                data.BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                                   where coursbranchemapping.CourseId == data.CourseId
                                   select coursbranchemapping).ToList();


                data.SessionList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcourseemapping.ExamId == data.ExamId
                                    select examcourseemapping).ToList();


                data.QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                               where qpmaster.CourseId == data.CourseId
                               select qpmaster).ToList();

                if (data == null)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizeContext(ViewAction.Edit)]
        [Obsolete]
        public async Task<IActionResult> Edit(int OldPaperId, PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            string UploadOldPaperDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPaper").Value.ToString();
            string UploadOldPatternDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPattern").Value.ToString();
            string UploadOldSyllabusDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldSyllabus").Value.ToString();
            string UploadCertificateDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadCertificate").Value.ToString();
            var supportedTypes = new[] { "pdf", "PDF", "jpg", "JPG", "jpeg", "JPEG", "png", "PNG" };
            FileHelper fileHelper = new FileHelper();
            try
            {
                var data = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(OldPaperId);


                pCPUploadOldPaperModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadOldPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPUploadOldPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();

                pCPUploadOldPaperModel.CourseList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                                     where examcoursemapping.ExamId == pCPUploadOldPaperModel.ExamId
                                                     select examcoursemapping).ToList();

                pCPUploadOldPaperModel.SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                                      where examcoursemapping.CourseID == pCPUploadOldPaperModel.CourseId
                                                      select examcoursemapping).ToList();

                pCPUploadOldPaperModel.BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                                                     where coursbranchemapping.CourseId == pCPUploadOldPaperModel.CourseId
                                                     select coursbranchemapping).ToList();


                pCPUploadOldPaperModel.SessionList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                                      where examcourseemapping.ExamId == pCPUploadOldPaperModel.ExamId
                                                      select examcourseemapping).ToList();

                pCPUploadOldPaperModel.QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                                                 where qpmaster.CourseId == pCPUploadOldPaperModel.CourseId
                                                 select qpmaster).ToList();
                #region Previous paper
                if (pCPUploadOldPaperModel.FUOldPaperEdit != null)
                {
                    var oldPaperExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldPaperEdit.FileName).Substring(1);
                    if (supportedTypes.Contains(oldPaperExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldPaperEdit.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldPaperPath = fileHelper.SaveFile(UploadOldPaperDocument, data.OldPaperPath, pCPUploadOldPaperModel.FUOldPaperEdit);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous Paper size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous paper extention is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    pCPUploadOldPaperModel.OldPaperPath = data.OldPaperPath;
                }
                #endregion
                #region previous syllabus
                if (pCPUploadOldPaperModel.FUOldSyllabusEdit != null)
                {
                    var oldSyllabusExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldSyllabusEdit.FileName).Substring(1);
                    if (supportedTypes.Contains(oldSyllabusExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldSyllabusEdit.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldSyllabusPath = fileHelper.SaveFile(UploadOldSyllabusDocument, data.OldSyllabusPath, pCPUploadOldPaperModel.FUOldSyllabusEdit);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous syllabus size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous syllabus extention is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                    }

                }
                else
                {
                    pCPUploadOldPaperModel.OldSyllabusPath = data.OldSyllabusPath;
                }
                #endregion
                #region prevois pattern             
                if (pCPUploadOldPaperModel.FUOldPatternEdit != null)
                {
                    var oldPatternExt = Path.GetExtension(pCPUploadOldPaperModel.FUOldPatternEdit.FileName).Substring(1);
                    if (supportedTypes.Contains(oldPatternExt))
                    {
                        if (pCPUploadOldPaperModel.FUOldPatternEdit.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.OldPatternPath = fileHelper.SaveFile(UploadOldPatternDocument, data.OldPatternPath, pCPUploadOldPaperModel.FUOldPatternEdit);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous pattern size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous pattern extention is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    pCPUploadOldPaperModel.OldPatternPath = data.OldPatternPath;
                }
                #endregion
                #region previous certificate not mandator
                if (pCPUploadOldPaperModel.FUCertificateEdit != null)
                {
                    var oldCertificateExt = Path.GetExtension(pCPUploadOldPaperModel.FUCertificateEdit.FileName).Substring(1);
                    if (supportedTypes.Contains(oldCertificateExt))
                    {
                        if (pCPUploadOldPaperModel.FUCertificateEdit.Length < 2100000)
                        {
                            pCPUploadOldPaperModel.CertificatePath = fileHelper.SaveFile(UploadCertificateDocument, data.CertificatePath, pCPUploadOldPaperModel.FUCertificateEdit);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous certificate size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous certificate is invalid- accept only pdf,jpg,jpeg,png");
                        return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    pCPUploadOldPaperModel.CertificatePath = data.CertificatePath;
                }
                #endregion

                //if (ModelState.IsValid) model is not valid due to old file upload
                //{
                if (pCPUploadOldPaperModel.OldPaperPath != null && pCPUploadOldPaperModel.OldSyllabusPath != null && pCPUploadOldPaperModel.OldPatternPath != null)
                {
                    var res = await _pCPUploadOldPaperService.UpdatePCPUploadOldPaperAsync(pCPUploadOldPaperModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Documents has been updated";
                    }
                    else
                    {
                        TempData["error"] = "Documents has not been updated";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "issue in file uploaded!");
                    return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Model state is not valid");
                //    return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
                // }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return View("~/Views/PCP/PCPUploadOldPaper/Edit.cshtml", pCPUploadOldPaperModel);
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _pCPUploadOldPaperService.GetPCPUploadOldPaperById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _pCPUploadOldPaperService.DeletePCPUploadOldPaperAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Documents has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Documents has not been deleted";
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
        public async Task<IActionResult> Download(string id, string paper, string syllabus, string patterns, string certificate)
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
                    var path = string.Empty;
                    var ext = string.Empty;
                    if (paper != null)
                    {
                        string UploadOldPaperDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPaper").Value.ToString();
                        path = Path.Combine(UploadOldPaperDocument, data.OldPaperPath);
                        ext = Path.GetExtension(data.OldPaperPath).Substring(1);
                    }
                    else if (syllabus != null)
                    {
                        string UploadOldSyllabusDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldSyllabus").Value.ToString();
                        path = Path.Combine(UploadOldSyllabusDocument, data.OldSyllabusPath);
                        ext = Path.GetExtension(data.OldSyllabusPath).Substring(1);
                    }
                    else if (patterns != null)
                    {
                        string UploadOldPatternDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadOldPattern").Value.ToString();
                        path = Path.Combine(UploadOldPatternDocument, data.OldPatternPath);
                        ext = Path.GetExtension(data.OldPatternPath).Substring(1);
                    }
                    else if (certificate != null)
                    {
                        string UploadCertificateDocument = _configuration.GetSection("FilePaths:PreviousDocuments:UploadCertificate").Value.ToString();
                        path = Path.Combine(UploadCertificateDocument, data.CertificatePath);
                        ext = Path.GetExtension(data.CertificatePath).Substring(1);
                    }
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    if (ext == "png" || ext == "PNG")
                    {
                        return File(FileBytes, "image/png");
                    }
                    else if (ext == "jpg" || ext == "JPG")
                    {
                        return File(FileBytes, "image/jpg");
                    }
                    else if (ext == "jpeg" || ext == "JPEG")
                    {
                        return File(FileBytes, "image/jpeg");
                    }
                    else
                    {
                        return File(FileBytes, "application/pdf");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return await Index();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(int qpId)
        {

            var already = (from data in _pCPUploadOldPaperService.GetAllPCPUploadOldPaper().Result
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

        public async Task<JsonResult> GetCourse(int ExamId)
        {
            var ExamList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                            where examcoursemapping.ExamId == ExamId
                            select new SelectListItem()
                            {
                                Text = examcoursemapping.CourseName,
                                Value = examcoursemapping.CourseID.ToString(),
                            }).ToList();

            ExamList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(ExamList);
        }

        public async Task<JsonResult> GetSemYear(int CourseId)
        {
            var SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                               where examcoursemapping.CourseID == CourseId
                               select new SelectListItem()
                               {
                                   Text = examcoursemapping.SemYearId.ToString(),
                                   Value = examcoursemapping.SemYearId.ToString(),
                               }).ToList();

            SemYearList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(SemYearList);
        }
        public async Task<JsonResult> GetBranch(int CourseId)
        {
            var BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                              where coursbranchemapping.CourseId == CourseId
                              select new SelectListItem()
                              {
                                  Text = coursbranchemapping.BranchName,
                                  Value = coursbranchemapping.BranchId.ToString(),
                              }).ToList();

            BranchList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(BranchList);
        }
        //public async Task<JsonResult> GetSetterByCourse(int BranchId)
        //{
        //    var SetterList = (from reg in await _pCPRegistrationService.GetSetterList()
        //                      where reg.BranchId == BranchId
        //                      select new SelectListItem()
        //                      {
        //                          Text = reg.UserName,
        //                          Value = reg.UserId.ToString(),
        //                      }).ToList();
        //    //SetterList.Insert(0, new SelectListItem()
        //    //{
        //    //    Text = "----Select----",
        //    //    Value = string.Empty
        //    //});
        //    return Json(SetterList);
        //}
        public async Task<JsonResult> GetSyllabus(int ExamId)
        {
            var SyllabusList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                where examcourseemapping.ExamId == ExamId
                                select new SelectListItem()
                                {
                                    Text = examcourseemapping.Session,
                                    Value = examcourseemapping.SessionId.ToString(),
                                }).ToList();

            SyllabusList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(SyllabusList);
        }
        public async Task<JsonResult> GetQP(int CourseId)
        {
            var QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                          where qpmaster.CourseId == CourseId
                          select new SelectListItem()
                          {
                              Text = qpmaster.QPCode + "-" + qpmaster.QPName,
                              Value = qpmaster.QPId.ToString(),
                          }).ToList();

            QPList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(QPList);
        }

    }
}
