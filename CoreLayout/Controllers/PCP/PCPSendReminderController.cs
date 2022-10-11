using CoreLayout.Models.Common;
using CoreLayout.Models.PCP;
using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPApproval;
using CoreLayout.Services.PCP.PCPRegistration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPSendReminder;
using CoreLayout.Services.PCP.PCPUploadPaper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "Controller Of Examination")]
    public class PCPSendReminderController : Controller
    {
        private readonly ILogger<PCPSendReminderController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        private readonly IPCPApprovalService _pCPApprovalService;
        private readonly IPCPSendReminderService _pCPSendReminderService;
        private readonly IMailService _mailService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        private readonly IPCPUploadPaperService _pCPUploadPaperService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PCPSendReminderController(ILogger<PCPSendReminderController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IDataProtectionProvider provider, IPCPApprovalService pCPApprovalService, IMailService mailService, CommonController commonController, IHostingEnvironment environment, IPCPAssignedQPService pCPAssignedQPService, IPCPSendReminderService pCPSendReminderService, IPCPUploadPaperService pCPUploadPaperService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _protector = provider.CreateProtector("PCPSendReminder.PCPSendReminderController");
            _pCPApprovalService = pCPApprovalService;
            _mailService = mailService;
            _commonController = commonController;
            hostingEnvironment = environment;
            _pCPAssignedQPService = pCPAssignedQPService;
            _pCPSendReminderService = pCPSendReminderService;
            _pCPUploadPaperService = pCPUploadPaperService;
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = (from upload in await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload()
                                //where qp.UserId == CreatedBy
                            select upload).ToList();
                //var data = await _pCPUploadPaperService.GetAllPCPUploadPaper();
                //ViewBag.PaperUploadedlist = allUploadedQP;
                //end
                List<string> pcslist = new List<string>();
                //start encrypt id for update,delete & details
                foreach (var _data in data)
                {
                    var stringId = _data.PaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);

                    if (_data.PaperPassword != null)
                    {
                        _data.DecryptPassword = _commonController.Decrypt(_data.PaperPassword);
                        pcslist.Add(_data.DecryptPassword);
                    }
                }
                //end
                ViewBag.EncryptPwdList = pcslist;

                //start bind filter qplist
                ViewBag.QPList = new SelectList(await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload(), "QPId", "QPCode");
                //end
                return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPSendReminder/Index.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(PCPRegistrationModel pCPRegistrationModel)//int? paperupload
        {
            int? paperupload = pCPRegistrationModel.IsPaperUploaded;

            //var data = (dynamic)null;
            if (paperupload == 1)//paper uploded
            {
                var data = (from upload in await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload()
                            where upload.PaperPath != null
                            select upload).ToList();

                //start encrypt id for update,delete & details
                foreach (var _data in data)
                {
                    var stringId = _data.PaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end
                return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);
            }
            else if (paperupload == 2)//paper not uploded
            {
                var data = (from upload in await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload()
                            where upload.PaperPath == null
                            select upload).ToList();
                return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);
            }
            else
            {
                var data = (from upload in await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload()
                                //where upload.PaperPath != null
                            select upload).ToList();
                //start encrypt id for update,delete & details
                foreach (var _data in data)
                {
                    var stringId = _data.PaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end
                return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);
            }


        }

        [HttpPost]
        public async Task<IActionResult> ResetAsync()//int? paperupload
        {
            var data = (from upload in (await _pCPUploadPaperService.BothUserPaperUploadAndNotUpload())
                            //where upload.PaperPath != null
                        select upload).ToList();
            //end

            //start encrypt id for update,delete & details
            foreach (var _data in data)
            {
                var stringId = _data.PaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);
            }
            foreach (var _data in data)
            {
                var stringId = _data.PaperId.ToString();
                _data.EncryptedId = _protector.Protect(stringId);
            }
            return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);
        }
        public async Task<IActionResult> SendReminder(string assignedQPId)
        {
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            try
            {
                if (assignedQPId != null)
                {

                    List<string> list = new List<string>();

                    String[] array = assignedQPId.Split(",");
                    for (int i = 0; i < array.Length; i++)
                    {
                        //list.Add(array[i]);
                        var data = await _pCPRegistrationService.ForSendReminderGetUseByIdAsync(Convert.ToInt32(array[i]));
                        pCPRegistrationModel.AssignedQPId = Convert.ToInt32(array[i]);
                        pCPRegistrationModel.PCPRegID = data.PCPRegID;
                        pCPRegistrationModel.UserId = data.UserId;
                        pCPRegistrationModel.UserName = data.UserName;
                        pCPRegistrationModel.MobileNo = data.MobileNo;
                        pCPRegistrationModel.EmailID = data.EmailID;
                        pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                        pCPRegistrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        pCPRegistrationModel.AssignedQPId = data.AssignedQPId;

                        #region Send email and sms
                        //send message
                        bool messageResult = false;
                        messageResult = SendRegistraionMeassage(pCPRegistrationModel.UserName, pCPRegistrationModel.MobileNo, pCPRegistrationModel.LoginID, pCPRegistrationModel.Password);
                        if (messageResult == false)
                        {
                            pCPRegistrationModel.IsMobileReminder = "N";
                        }
                        else
                        {
                            pCPRegistrationModel.IsMobileReminder = messageResult.ToString();
                        }

                        //send mail
                        bool emailResult = false;
                        emailResult = SendRegistraionMail(pCPRegistrationModel.UserName, pCPRegistrationModel.EmailID, pCPRegistrationModel.LoginID, pCPRegistrationModel.Password);
                        if (emailResult == false)
                        {
                            pCPRegistrationModel.IsEmailReminder = "N";
                        }
                        else
                        {
                            pCPRegistrationModel.IsEmailReminder = emailResult.ToString();
                        }
                        #endregion

                        var finaldata = await _pCPSendReminderService.CreateReminderAsync(pCPRegistrationModel);
                        if (finaldata.Equals(1))
                        {
                            TempData["success"] = "Reminder send successfully";
                        }
                        else
                        {
                            TempData["error"] = "Reminder send failed";
                        }

                    }
                    //return View("~/Views/PCP/PCPSendReminder/Index.cshtml", pCPRegistrationModel);
                    //return RedirectToAction("~/Views/PCP/PCPSendReminder/Index.cshtml");
                    return await IndexAsync(pCPRegistrationModel);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
                ModelState.AddModelError("", ex.ToString());
                return View();
            }
            return View("~/Views/PCP/PCPSendReminder/Index.cshtml");
        }

        //public async Task<JsonResult> ViewReminderAsync(string uid)
        //{
        //    List<PCPRegistrationModel> data = null;
        //    try
        //    {
        //        data = await _pCPSendReminderService.GetReminderById(Convert.ToInt32(uid));

        //        ViewBag.ViewReminder = data;

        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Some thing went wrong";
        //    }
        //    return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        //}

        public bool SendRegistraionMeassage(string username, string mobile, string loginid, string password)
        {
            bool result = false;
            string message = "Dear " + username + ", Attendance for Subject Codes: " + mobile + " is not entered today. Please specify reasons at " + loginid + " " + password + ". CSJMU, Kanpur";

            result = _commonController.SendSMSWithTemplateId(mobile, message, "1607100000000228609");
            return result;
        }

        public bool SendRegistraionMail(string username, string email, string loginid, string password)
        {
            MailRequest request = new MailRequest();

            request.Subject = "Gentel Reminder!!";
            request.Body = "<br/> This is reminder mail" +
                   "<br/> Please upload paper as soon as possible.";
            //request.Attachments = "";
            request.ToEmail = email;
            return _mailService.SendEmailAsync(request);
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
                    pCPUploadPaperModel.DownloadStatus = "Download Paper By COE";
                    var res = await _pCPUploadPaperService.InsertDownloadLogAsync(pCPUploadPaperModel);
                    //end

                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
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
        [Obsolete]
        public async Task<IActionResult> DownloadAnswer(string id)
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
                    pCPUploadPaperModel.DownloadStatus = "Download Answer By COE";
                    var res = await _pCPUploadPaperService.InsertDownloadLogAsync(pCPUploadPaperModel);
                    //end

                    #region file download
                    string uploadsFolder = System.IO.Path.Combine(hostingEnvironment.WebRootPath, "AnswerPaper");
                    var path = System.IO.Path.Combine(uploadsFolder, data.AnswerPath);
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

        public async Task<IActionResult> ViewReminder(int id, int assignedQPId)
        {
            try
            {
                //var guid_id = _protector.Unprotect(id);
                var data = await _pCPSendReminderService.GetReminderById(id, assignedQPId);

                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    return View("~/Views/PCP/PCPSendReminder/ViewReminder.cshtml", data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> RequestQuestionPassword(string id)
        {
            PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
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
                    pCPUploadPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    pCPUploadPaperModel.PaperId = data.PaperId;
                    pCPUploadPaperModel.RequestQuestionPwdStatus = "Request For QuestionPaper Password By COE";
                    var res = await _pCPUploadPaperService.RequestQuestionPassword(pCPUploadPaperModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Question Paper Password has been requested";
                    }
                    else
                    {
                        TempData["error"] = "Question Paper Password has not been requested";
                    }
                    //end
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RequestAnswerPassword(string id)
        {
            PCPUploadPaperModel pCPUploadPaperModel = new PCPUploadPaperModel();
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
                    //insert download record
                    pCPUploadPaperModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    pCPUploadPaperModel.PaperId = data.PaperId;
                    pCPUploadPaperModel.RequestAnswerPwdStatus = "Request For AnswerPaper Password By COE";
                    var res = await _pCPUploadPaperService.RequestAnswerPassword(pCPUploadPaperModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "AnswerPaper Password has been requested";
                    }
                    else
                    {
                        TempData["error"] = "AnswerPaper Password has not been requested";
                    }
                    //end
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
