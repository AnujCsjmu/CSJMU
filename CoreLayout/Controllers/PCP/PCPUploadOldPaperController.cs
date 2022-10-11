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
    [Authorize(Roles = "QPAssign, Controller Of Examination")]
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
        [Obsolete]
        public PCPUploadOldPaperController(ILogger<PCPUploadOldPaperController> logger, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment, ICourseDetailsService courseDetailsService, IPCPAssignedQPService pCPAssignedQPService
            , IQPMasterService qPMasterService, IPCPUploadOldPaperService pCPUploadOldPaperService, IExamMasterService examMasterService,
            IExamCourseMappingService examCourseMappingService, ICourseBranchMappingService courseBranchMappingService)
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
        [Obsolete]
        public async Task<IActionResult> CreateAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            try
            {
                //start check paper already uploaded
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var alreadyexit = (from reg in await _pCPUploadOldPaperService.GetAllPCPUploadOldPaper()
                                   where reg.CreatedBy == CreatedBy && reg.QPId == pCPUploadOldPaperModel.QPId
                                   select reg).ToList();
                //end

                pCPUploadOldPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPUploadOldPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPUploadOldPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                if (pCPUploadOldPaperModel.FUOldPape != null && pCPUploadOldPaperModel.FUOldSyllabus != null && pCPUploadOldPaperModel.FUOldPattern != null)
                {
                    var supportedTypes = new[] { "pdf", "PDF", "jpg", "JPG", "jpeg", "JPEG", "png", "PNG" };
                    var oldPaper = Path.GetExtension(pCPUploadOldPaperModel.FUOldPape.FileName).Substring(1);
                    var oldSyllabus = Path.GetExtension(pCPUploadOldPaperModel.FUOldSyllabus.FileName).Substring(1);
                    var oldPattern = Path.GetExtension(pCPUploadOldPaperModel.FUOldPattern.FileName).Substring(1);

                    //certificate
                    if (pCPUploadOldPaperModel.FUCertificate != null)
                    {
                        var certificate = Path.GetExtension(pCPUploadOldPaperModel.FUOldPattern.FileName).Substring(1);
                        if (!supportedTypes.Contains(certificate))
                        {
                            ModelState.AddModelError("", "certificate extention is invalid");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                        if (pCPUploadOldPaperModel.FUCertificate.Length > 2100000)
                        {
                            ModelState.AddModelError("", "FUCertificate size must be less than 2 mb");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                        if (pCPUploadOldPaperModel.FUCertificate.FileName == null)
                        {
                            ModelState.AddModelError("", "Certificate name is blank");
                            return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }

                    if (!supportedTypes.Contains(oldPaper) && !supportedTypes.Contains(oldSyllabus) && !supportedTypes.Contains(oldPattern))
                    {
                        ModelState.AddModelError("", "file extention is invalid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    if (pCPUploadOldPaperModel.FUOldPape.Length > 2100000 && pCPUploadOldPaperModel.FUOldSyllabus.Length > 2100000 && pCPUploadOldPaperModel.FUOldPattern.Length > 2100000)
                    {
                        ModelState.AddModelError("", "Old Paper/Syllabus/Pattern size must be less than 2 mb");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    if (pCPUploadOldPaperModel.FUOldPape.FileName == null && pCPUploadOldPaperModel.FUOldSyllabus.FileName == null && pCPUploadOldPaperModel.FUOldPattern.FileName == null)
                    {
                        ModelState.AddModelError("", "File name is blank");
                        return View("~/Views/PCP/PCPUploadPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    if (alreadyexit.Count > 0)
                    {
                        ModelState.AddModelError("", "Data already exit");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    if (ModelState.IsValid)
                    {
                        var uniqueFileNameOld = UploadedFile(pCPUploadOldPaperModel.FUOldPape, "", "UploadOldPaper");
                        var uniqueFileNameSyllabus = UploadedFile(pCPUploadOldPaperModel.FUOldSyllabus, "", "UploadOldSyllabus");
                        var uniqueFileNamePattern = UploadedFile(pCPUploadOldPaperModel.FUOldPattern, "", "UploadOldPattern");

                        if (uniqueFileNameOld != null && uniqueFileNameSyllabus != null && uniqueFileNamePattern != null)
                        {
                            var uniqueFileNameCertificate = (dynamic)null;
                            if (pCPUploadOldPaperModel.FUCertificate != null)
                            {
                                uniqueFileNameCertificate = UploadedFile(pCPUploadOldPaperModel.FUCertificate, "", "UploadCertificate");
                            }
                            pCPUploadOldPaperModel.OldPaperPath = uniqueFileNameOld;
                            pCPUploadOldPaperModel.OldSyllabusPath = uniqueFileNameSyllabus;
                            pCPUploadOldPaperModel.OldPatternPath = uniqueFileNamePattern;
                            pCPUploadOldPaperModel.CertificatePath = uniqueFileNameCertificate;

                            var res = await _pCPUploadOldPaperService.CreatePCPUploadOldPaperAsync(pCPUploadOldPaperModel);
                            if (res.Equals(1))
                            {
                                TempData["success"] = "Documents has been saved";
                            }
                            else
                            {
                                TempData["error"] = "Documents has not been saved";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "issue in file uploaded!");
                            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Model state is not valid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "File not uploaded");
                    return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
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
                    string deletefilePath = System.IO.Path.Combine(uploadsFolder, oldpath);
                    //if folder not exit
                    if (!Directory.Exists(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername)))
                    {
                        Directory.CreateDirectory(System.IO.Path.Combine(hostingEnvironment.WebRootPath, foldername));
                    }
                    //delete old file if edit the record
                    if (System.IO.File.Exists(deletefilePath))
                    {
                        System.IO.File.Delete(deletefilePath);
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

                var supportedTypes = new[] { "pdf", "PDF", "jpg", "JPG", "jpeg", "JPEG", "png", "PNG" };
                var uniqueFileNameOld = (dynamic)null;
                var uniqueFileNameSyllabus = (dynamic)null;
                var uniqueFileNamePattern = (dynamic)null;
                var uniqueFileNameCertificate = (dynamic)null;
                //previus paper
                if (pCPUploadOldPaperModel.FUOldPaperEdit != null)
                {
                    var oldPaper = Path.GetExtension(pCPUploadOldPaperModel.FUOldPaperEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(oldPaper))
                    {
                        ModelState.AddModelError("", "previous paper extention is invalid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    uniqueFileNameOld = UploadedFile(pCPUploadOldPaperModel.FUOldPape, data.OldPaperPath, "UploadOldPaper");
                }
                else
                {
                    uniqueFileNameOld = data.OldPaperPath;

                }
                //previus syllabus
                if (pCPUploadOldPaperModel.FUOldSyllabusEdit != null)
                {
                    var oldSyllabus = Path.GetExtension(pCPUploadOldPaperModel.FUOldSyllabusEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(oldSyllabus))
                    {
                        ModelState.AddModelError("", "previous syllabus extention is invalid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    uniqueFileNameSyllabus = UploadedFile(pCPUploadOldPaperModel.FUOldSyllabus, data.OldSyllabusPath, "UploadOldSyllabus");
                }
                else
                {
                    uniqueFileNameSyllabus = data.OldSyllabusPath;

                }
                //previus pattern
                if (pCPUploadOldPaperModel.FUOldPatternEdit != null)
                {
                    var oldPattern = Path.GetExtension(pCPUploadOldPaperModel.FUOldPatternEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(oldPattern))
                    {
                        ModelState.AddModelError("", "previous pattern extention is invalid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    uniqueFileNamePattern = UploadedFile(pCPUploadOldPaperModel.FUOldPattern, data.OldPatternPath, "UploadOldPattern");
                }
                else
                {
                    uniqueFileNamePattern = data.OldPatternPath;
                }
                //previus certificate
                if (pCPUploadOldPaperModel.FUCertificateEdit != null)
                {
                    var certificate = Path.GetExtension(pCPUploadOldPaperModel.FUCertificateEdit.FileName).Substring(1);
                    if (!supportedTypes.Contains(certificate))
                    {
                        ModelState.AddModelError("", "certificate extention is invalid");
                        return View("~/Views/PCP/PCPUploadOldPaper/Create.cshtml", pCPUploadOldPaperModel);
                    }
                    uniqueFileNameCertificate = UploadedFile(pCPUploadOldPaperModel.FUCertificate, data.CertificatePath, "UploadCertificate");
                }
                else
                {
                    uniqueFileNameCertificate = data.CertificatePath;
                }
                //if (ModelState.IsValid) model is not valid due to old file upload
                //{
                    if (uniqueFileNameOld != null && uniqueFileNameSyllabus != null && uniqueFileNamePattern != null)
                    {
                        pCPUploadOldPaperModel.OldPaperPath = uniqueFileNameOld;
                        pCPUploadOldPaperModel.OldSyllabusPath = uniqueFileNameSyllabus;
                        pCPUploadOldPaperModel.OldPatternPath = uniqueFileNamePattern;
                        pCPUploadOldPaperModel.CertificatePath = uniqueFileNameCertificate;

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

        [Obsolete]
        public async Task<IActionResult> DownloadCertificate(string id)
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
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadCertificate");
                    var path = System.IO.Path.Combine(uploadsFolder, data.CertificatePath);
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

        public async Task<JsonResult> GetCourseSubjectSemYearSyl(int QPId)
        {
            var Combinelist = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                               where qpmaster.QPId == QPId
                               select qpmaster).ToList();
            return Json(Combinelist);
        }

    }
}
