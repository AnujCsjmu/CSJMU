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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public PCPSendPaperController(ILogger<PCPSendPaperController> logger, IDataProtectionProvider provider, IPCPSendReminderService pCPSendReminderService, IPCPSendPaperService pCPSendPaperService, IRegistrationService registrationService, IPCPUploadPaperService pCPUploadPaperService, ICourseService courseService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("PCPSendPaper.PCPSendPaperController");
            _pCPSendReminderService = pCPSendReminderService;
            _pCPSendPaperService = pCPSendPaperService;
            _registrationService = registrationService;
            _pCPUploadPaperService = pCPUploadPaperService;
            _courseService = courseService;
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _pCPSendPaperService.GetAllPCPSendPaper();
                //end

                //start encrypt id for update,delete & details
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
                ViewBag.PaperList = (from paper in _pCPUploadPaperService.GetAllPCPUploadPaper().Result
                                         //where user.CreatedBy == PaperSetterId
                                     select paper).ToList();
                var guid_id = _protector.Unprotect(id);
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
                if (ModelState.IsValid)
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
                               where user.CreatedBy== PaperSetterId
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
    }
}
