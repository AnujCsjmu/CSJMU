using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.PCP.PCPSendPaper;
using CoreLayout.Services.PCP.PCPSendReminder;
using CoreLayout.Services.PCP.PCPUploadPaper;
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
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PCPSendPaperController(ILogger<PCPSendPaperController> logger, IDataProtectionProvider provider, IPCPSendReminderService pCPSendReminderService, IPCPSendPaperService pCPSendPaperService, IRegistrationService registrationService, IPCPUploadPaperService pCPUploadPaperService, ICourseService courseService, IHostingEnvironment environment, CommonController commonController)
        {
            _logger = logger;
            _protector = provider.CreateProtector("PCPSendPaper.PCPSendPaperController");
            _pCPSendReminderService = pCPSendReminderService;
            _pCPSendPaperService = pCPSendPaperService;
            _registrationService = registrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _courseService = courseService;
            hostingEnvironment = environment;
            _commonController = commonController;
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
                pCPSendPaperModel.CourseList = (from course in await _courseService.GetAllCourse()
                                                select course).ToList();
                pCPSendPaperModel.AgencyList = (from reg in await _registrationService.GetAllRegistrationAsync()
                                                where reg.RoleId == 21
                                                select reg).ToList();
                pCPSendPaperModel.PaperSetterList = (from setter in await _pCPSendPaperService.GetAllPCPUser_UploadPaperAsync()
                                                     where setter.RoleId == 19
                                                     select setter).ToList();
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
            //start check paper already sent to agency
            int result = 0;
            String[] array = pCPSendPaperModel.paperids.Split(",");
            for (int i = 0; i < array.Length; i++)
            {
                int paperid = Convert.ToInt32(array[i]);
                var data = (from sendpaper in await _pCPSendPaperService.GetAllPCPSendPaper()
                            where sendpaper.AgencyId == pCPSendPaperModel.UserId && sendpaper.PaperId == paperid
                            select sendpaper).ToList();
                if (data.Count > 0)
                {
                    result = 1;
                }
            }
            //end
            pCPSendPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            pCPSendPaperModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            pCPSendPaperModel.CourseList = (from course in await _courseService.GetAllCourse()
                                            select course).ToList();
            pCPSendPaperModel.AgencyList = (from reg in await _registrationService.GetAllRegistrationAsync()
                                            where reg.RoleId == 21
                                            select reg).ToList();
            pCPSendPaperModel.PaperSetterList = (from setter in await _pCPSendPaperService.GetAllPCPUser_UploadPaperAsync()
                                                 where setter.RoleId == 19
                                                 select setter).ToList();
            ViewBag.PaperList = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                                     //where user.CreatedBy == PaperSetterId
                                 select paper).ToList();
            if (pCPSendPaperModel.paperids != null)
            {
                if (result == 1)
                {
                    ModelState.AddModelError("", "Paper already sent to this agency!");
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

            }
            else
            {
                ModelState.AddModelError("", "Please select atleast one checkbox");
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
        public JsonResult GetPaper(int PaperSetterId)
        {
            var GetUserList = (from user in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                               where user.CreatedBy == PaperSetterId
                               select new SelectListItem()
                               {
                                   Text = user.PaperName,
                                   Value = user.PaperId.ToString(),
                               }).ToList();
            //GetUserList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = string.Empty
            //});

            //var GetUserList = _buttonPermissionService.GetUserByRoleAsync(role);

            return Json(GetUserList);
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
    }
}
