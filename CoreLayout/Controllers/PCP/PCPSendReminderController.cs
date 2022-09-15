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
using System.IO;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPSendReminder;
using iTextSharp.text.pdf;
using CoreLayout.Services.PCP.PCPUploadPaper;

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

                var data = (from reg in await _pCPSendReminderService.GetAllAssingedQP()
                            //where reg.IsApproved != null
                            select reg).ToList();

                //end

                //start encrypt id for update,delete & details
                foreach (var _data in data)
                {
                    var stringId = _data.PaperId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end


                return View("~/Views/PCP/PCPSendReminder/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPSendReminder/Index.cshtml");
        }
        //[HttpGet]
        //[AuthorizeContext(ViewAction.Details)]
        //[Obsolete]
        //public async Task<IActionResult> Details(string id)
        //{
        //    try
        //    {
        //        var guid_id = _protector.Unprotect(id);
        //        var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));

        //        // string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "PCPPhoto");
        //        //string filePath = Path.Combine(uploadsFolder, data.UploadFileName);
        //        string filePath = "~/PCPPhoto/" + data.UploadFileName;
        //        data.UploadFileName = filePath;
        //        data.EncryptedId = id;
        //        if (data == null)
        //        {
        //            return NotFound();
        //        }
        //        return View("~/Views/PCP/PCPSendReminder/Details.cshtml", data);

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.ToString());
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> SendReminderAsync(string uid)
        {
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            try
            {
                if (uid != null)
                {

                    List<string> list = new List<string>();

                    String[] array = uid.Split(",");
                    for (int i = 0; i < array.Length; i++)
                    {
                        //list.Add(array[i]);
                        var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(array[i]));
                        pCPRegistrationModel.PCPRegID = Convert.ToInt32(array[i]);
                        pCPRegistrationModel.UserId = data.UserId;
                        pCPRegistrationModel.UserName = data.UserName;
                        pCPRegistrationModel.MobileNo = data.MobileNo;
                        pCPRegistrationModel.EmailID = data.EmailID;
                        pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                        pCPRegistrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        pCPRegistrationModel.QPId = data.QPId;

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
                    return View("~/Views/PCP/PCPSendReminder/Index.cshtml", pCPRegistrationModel);
                    //return View(pCPRegistrationModel);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
                return View();
            }
            return View("~/Views/PCP/PCPSendReminder/Index.cshtml");
        }

        public async Task<JsonResult> ViewReminderAsync(string uid)
        {
            List<PCPRegistrationModel> data = null;
            try
            {
                data = await _pCPSendReminderService.GetReminderById(Convert.ToInt32(uid));

                ViewBag.ViewReminder = data;

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
            }
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings());
        }

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
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "UploadPaper");
                    var path = Path.Combine(uploadsFolder, data.PaperPath);
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
