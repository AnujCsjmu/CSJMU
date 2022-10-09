using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPSendReminder;
using CoreLayout.Services.PCP.PCPUploadPaper;
using CoreLayout.Services.QPDetails.QPMaster;
using CoreLayout.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Controller Of Examination")]
    public class PCPSendPaperController : Controller
    {
        private readonly ILogger<PCPSendPaperController> _logger;
        private readonly IDataProtector _protector;
        private readonly IPCPSendReminderService _pCPSendReminderService;
        private readonly IPCPSendPaperService _pCPSendPaperService;
        private readonly IRegistrationService _registrationService;
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        private readonly ICourseService _courseService;
        private readonly CommonController _commonController;
        private readonly IExamMasterService _examMasterService;
        private readonly IExamCourseMappingService _examCourseMappingService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        private readonly IQPMasterService _qPMasterService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PCPSendPaperController(ILogger<PCPSendPaperController> logger, IDataProtectionProvider provider,
            IPCPSendPaperService pCPSendPaperService, IRegistrationService registrationService,
            IPCPUploadPaperService pCPUploadPaperService, IHostingEnvironment environment,
            CommonController commonController, IExamMasterService examMasterService, IExamCourseMappingService examCourseMappingService,
            ICourseBranchMappingService courseBranchMappingService, IQPMasterService qPMasterService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("PCPSendPaper.PCPSendPaperController");
            _pCPSendPaperService = pCPSendPaperService;
            _registrationService = registrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            hostingEnvironment = environment;
            _commonController = commonController;
            _examMasterService = examMasterService;
            _examCourseMappingService = examCourseMappingService;
            _courseBranchMappingService = courseBranchMappingService;
            _qPMasterService = qPMasterService;
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _pCPSendPaperService.GetAllPCPSendPaper();
                //end
                List<string> pcslist = new List<string>();
                //start encrypt id for update,delete & details
                foreach (var _data in data)
                {
                    var stringId = _data.SendPaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);

                    //for decrypt pwd
                    if (_data.PaperPassword != null)
                    {
                        _data.DecryptPassword = _commonController.Decrypt(_data.PaperPassword);
                        pcslist.Add(_data.DecryptPassword);
                    }
                }
                ViewBag.EncryptPwdList = pcslist;
                //end
                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.SendPaperId;
                }
                id = id + 1;
                ViewBag.MaxSendPaperId = _protector.Protect(id.ToString());
                //end

                return View("~/Views/PCP/PCPSendPaper/Index.cshtml", data);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPSendPaper/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                PCPSendPaperModel pCPSendPaperModel = new PCPSendPaperModel();
                //pCPSendPaperModel.CourseList = (from course in await _courseService.GetAllCourse()
                //                                select course).ToList();
                pCPSendPaperModel.AgencyList = (from reg in await _registrationService.GetAllRegistrationAsync()
                                                where reg.RoleId == 21
                                                select reg).ToList();
                pCPSendPaperModel.PaperSetterList = (from setter in await _pCPSendPaperService.GetAllPCPUser_UploadPaperAsync()
                                                     where setter.RoleId == 19
                                                     select setter).ToList();
                pCPSendPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();

                var data = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                                //where user.CreatedBy == PaperSetterId
                            select paper).ToList();
                ViewBag.PaperList = data;

                //end
                return View("~/Views/PCP/PCPSendPaper/Create.cshtml", pCPSendPaperModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(PCPSendPaperModel pCPSendPaperModel)
        {
            int result = 0;
            pCPSendPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            pCPSendPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            pCPSendPaperModel.AgencyList = (from reg in await _registrationService.GetAllRegistrationAsync()
                                            where reg.RoleId == 21
                                            select reg).ToList();
            pCPSendPaperModel.PaperSetterList = (from setter in await _pCPSendPaperService.GetAllPCPUser_UploadPaperAsync()
                                                 where setter.RoleId == 19
                                                 select setter).ToList();
            pCPSendPaperModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
            if (pCPSendPaperModel.paperids != null)
            {

                String[] array = pCPSendPaperModel.paperids.Split(",");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        int paperid = Convert.ToInt32(array[i]);
                        var data = (from sendpaper in await _pCPSendPaperService.GetAllPCPSendPaper()
                                        //where sendpaper.AgencyId == pCPSendPaperModel.UserId && sendpaper.PaperId == paperid
                                    where sendpaper.PaperId == paperid
                                    select sendpaper).ToList();
                        if (data.Count > 0)
                        {
                            result = 1;
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Check atleast one paper!");
                return View("~/Views/PCP/PCPSendPaper/Create.cshtml", pCPSendPaperModel);
            }
            //end


            if (result == 1)
            {
                ModelState.AddModelError("", "Paper already sent to the agency!");
                return View("~/Views/PCP/PCPSendPaper/Create.cshtml", pCPSendPaperModel);
            }
            else if (ModelState.IsValid)
            {

                var res = await _pCPSendPaperService.CreatePCPSendPaperAsync(pCPSendPaperModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Paper has been send";
                }
                else
                {
                    TempData["error"] = "Paper has not been send";
                }
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/PCP/PCPSendPaper/Create.cshtml", pCPSendPaperModel);
        }

        public IActionResult SendPaper(string uid)
        {
            PCPSendPaperModel pCPRegistrationModel = new PCPSendPaperModel();
            if (uid != null)
            {
                List<string> list = new List<string>();
                List<PCPSendPaperModel> list1 = new List<PCPSendPaperModel>();
                String[] array = uid.Split(",");
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(array[i]);
                }
                pCPRegistrationModel.SelectedPaperList = list;
            }

            return View("~/Views/PCP/PCPSendPaper/Create.cshtml", pCPRegistrationModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var dbRole = await _pCPSendPaperService.GetPCPSendPaperById(Convert.ToInt32(guid_id));
                if (dbRole != null)
                {
                    var res = await _pCPSendPaperService.DeletePCPSendPaperAsync(dbRole);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "Data has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Data has not been deleted";
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
        public async Task<IActionResult> DownloadAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPUploadPaperService.GetPCPUploadPaperById(Convert.ToInt32(guid_id)); // get paer details for download
                var data1 = await _pCPSendPaperService.GetPCPSendPaperById(Convert.ToInt32(guid_id));//get peper open time
                var data2 = await _pCPSendPaperService.GetServerDateTime();//get server datetime

                DateTime paperOpenTime = data1.PaperOpenTime;
                DateTime serverDatetime = data2.ServerDateTime;
                if (data == null)
                {
                    return NotFound();
                }
                else if (serverDatetime <= paperOpenTime)
                {
                    TempData["error"] = "Paper open time is over";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaperEncrption");
                    var path = System.IO.Path.Combine(uploadsFolder, data.PaperPath);
                    //string dycriptpassword = _commonController.Decrypt(data.PaperPassword);
                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetPaper(int ExamId, int CourseId, int BranchId, int SessionId, int PaperSetterId, int QPId, int SemYearId)
        {
            object list = null;

            #region 1st cast 7 out of 7 not blank
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId!=0 && PaperSetterId!=0 && QPId!=0 && SemYearId != 0)
            {
                list= (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                       where paper.ExamId==ExamId && paper.CourseId==CourseId && paper.BranchId==BranchId && paper.SessionId==SessionId &&  paper.CreatedBy == PaperSetterId && paper.QPId ==QPId && paper.SemYearId == SemYearId
                       select paper).ToList();
            }
            #endregion

            #region start 2nd case 7 out of 6 is not blank
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            #endregion end 2 case

            #region start 2nd case 7 out of 5 is not blank
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where  paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }

            //2nd
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId  && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //3rd
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SessionId == SessionId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }

            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //4th
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId  && paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //5th
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //6th
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            #endregion end 2 case

            #region 3rd cast 7 out of 4 is not blank
            //1st
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId &&  paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where  paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
           // 2nd
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId  && paper.CreatedBy == PaperSetterId  && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId  && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //3rd
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SessionId == SessionId  && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SessionId == SessionId  && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //4th
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId  && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where  paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //5th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }

            #endregion

            #region 4th cast 7 out of 3 is not blank
         
            if (ExamId != 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.BranchId == BranchId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SessionId == SessionId
                        select paper).ToList();
            }

            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //2nd
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where  paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SessionId == SessionId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.CreatedBy== PaperSetterId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //3rd
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where  paper.BranchId == BranchId && paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SessionId == SessionId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SessionId == SessionId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //4th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }

            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //5th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId && paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            #endregion

            #region 4th cast 7 out of 2 is not blank
            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CourseId == CourseId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.BranchId == BranchId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SessionId == SessionId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }

            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //2nd

            if (ExamId == 0 && CourseId != 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.BranchId == BranchId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.SessionId == SessionId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //3rd
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SessionId == SessionId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //4th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.CreatedBy == PaperSetterId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //5th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CreatedBy == PaperSetterId && paper.QPId == QPId
                        select paper).ToList();
            }
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CreatedBy == PaperSetterId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }
            //6th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId != 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.QPId == QPId && paper.SemYearId == SemYearId
                        select paper).ToList();
            }




            #endregion

            #region 4th cast 7 out of 1 is not blank
            if (ExamId != 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.ExamId == ExamId
                        select paper).ToList();
            }
           
            //2nd

            if (ExamId != 0 && CourseId != 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CourseId == CourseId
                        select paper).ToList();
            }
            
            //3rd
            if (ExamId == 0 && CourseId == 0 && BranchId != 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.BranchId == BranchId
                        select paper).ToList();
            }
           
            //4th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId != 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.SessionId == SessionId 
                        select paper).ToList();
            }
          
           
            //5th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId != 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.CreatedBy == PaperSetterId 
                        select paper).ToList();
            }
           
            //6th
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId != 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        where paper.QPId == QPId
                        select paper).ToList();
            }
            #endregion

            #region start 7nd case 7 out of 7 is blank
            if (ExamId == 0 && CourseId == 0 && BranchId == 0 && SessionId == 0 && PaperSetterId == 0 && QPId == 0 && SemYearId == 0)
            {
                list = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                        select paper).ToList();
            }
            #endregion end 7 case

            return Json(list);
        }
        public async Task<JsonResult> GetCourse(int ExamId)
        {
            var CourseList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                            where examcoursemapping.ExamId == ExamId
                            select new SelectListItem()
                            {
                                Text = examcoursemapping.CourseName,
                                Value = examcoursemapping.CourseID.ToString(),
                            }).ToList();

            CourseList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            }); 
            return Json(CourseList);
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
                              Text = qpmaster.QPCode,
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
