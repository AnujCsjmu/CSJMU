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

namespace CoreLayout.Controllers.PCP
{
    public class PCPApprovedController : Controller
    {
        private readonly ILogger<PCPApprovedController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        private readonly IPCPApprovalService _pCPApprovalService;
        private readonly IMailService _mailService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        public PCPApprovedController(ILogger<PCPApprovedController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IDataProtectionProvider provider, IPCPApprovalService pCPApprovalService, IMailService mailService, CommonController commonController)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _protector = provider.CreateProtector("PCPApproved.PCPApprovedController");
            _pCPApprovalService = pCPApprovalService;
            _mailService = mailService;
            _commonController = commonController;
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                            //where reg.IsApproved != null
                            select reg).ToList();
                //var data = await _pCPRegistrationService.GetAllPCPRegistration();

                foreach (var _data in data)
                {
                    var stringId = _data.UserID.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end


                return View("~/Views/PCP/PCPApproved/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPApproved/Index.cshtml");
        }

        public async Task<IActionResult> SendReminderAsync(string uid)
        {
            try
            {
                MailRequest request = new MailRequest();
                string emailresult = string.Empty;
                string msg = string.Empty;
                bool mobileresult = false;
                if (uid != null)
                {
                    PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
                    List<string> list = new List<string>();

                    String[] array = uid.Split(",");
                    for (int i = 0; i < array.Length; i++)
                    {
                        //list.Add(array[i]);
                        var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(array[i]));
                        pCPRegistrationModel.UserID = Convert.ToInt32(array[i]);
                        pCPRegistrationModel.UserName = data.UserName;
                        string papercode = "P001";
                        pCPRegistrationModel.MobileNo = data.MobileNo;
                        pCPRegistrationModel.EmailID = data.EmailID;
                        pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                        pCPRegistrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        request.ToEmail = data.EmailID;
                        request.Subject = "Gentel Reminder!!";
                        string htmlString = @"<html>
                       <body>
                       <p>Dear Sir ,</p>
                       <p>You are not uploaded paper which is make by you.
                          I shall be happy to be there as requested and will upload papers.</p>
                       <p>Sincerely,<br>-CSJMU</br></p>
                       </body>
                       </html>";
                        //string message = "";
                        request.Body = htmlString;
                        string message = "Dear " + pCPRegistrationModel.UserName + ", Attendance for Subject Codes: " + papercode + " is not entered today. Please specify reasons at COE office. CSJMU, Kanpur";

                        mobileresult = _commonController.SendSMSWithTemplateId(pCPRegistrationModel.MobileNo, message, "1607100000000228609");
                        //if (mobileresult == true)
                        //{
                        //    pCPRegistrationModel.MobileStatus = "success";
                        //    pCPRegistrationModel.MobileReminder = "Mobile";
                        //}
                        //else
                        //{
                        //    pCPRegistrationModel.MobileStatus = "failed";
                        //    pCPRegistrationModel.MobileReminder = "Mobile";
                        //}

                        //emailresult = await _mailService.SendEmailAsync(request);
                        //if (emailresult != null)
                        //{
                        //    pCPRegistrationModel.EmailStatus = "success";
                        //    pCPRegistrationModel.EmailReminder = "Email";
                        //}
                        //else
                        //{
                        //    pCPRegistrationModel.EmailStatus = "failed";
                        //    pCPRegistrationModel.EmailReminder = "Email";
                        //}

                        var res = await _pCPApprovalService.CreateReminderAsync(pCPRegistrationModel);
                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "Reminder send successfully";
                        }
                        else
                        {
                            TempData["error"] = "Reminder send failed";
                        }

                    }
                    return View(pCPRegistrationModel);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
                return View();
            }
            return View();
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();

                var guid_id = _protector.Unprotect(id);
                var data = await _pCPApprovalService.GetReminderById(Convert.ToInt32(guid_id));
                if (data == null)
                {
                    return NotFound();
                }
                // return View("~/Views/PCP/PCPApproved/Details.cshtml", data);
                return PartialView("_CreateEdit", pCPRegistrationModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(IndexAsync));
        }
        public async Task<JsonResult> ViewReminderAsync(string uid)
        {
            List<PCPRegistrationModel> data = null;
            try
            {
                data = await _pCPApprovalService.GetReminderById(Convert.ToInt32(uid));

                ViewBag.ViewReminder = data;

            }
            catch (Exception ex)
            {
                TempData["error"] = "Some thing went wrong";
            }
            return Json(data,new Newtonsoft.Json.JsonSerializerSettings());
        }

    }
}
