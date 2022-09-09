using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPApproval;
using CoreLayout.Services.PCP.PCPRegistration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class PCPApprovalController : Controller
    {
        private readonly ILogger<PCPApprovalController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        private readonly IPCPApprovalService _pCPApprovalService;
        public PCPApprovalController(ILogger<PCPApprovalController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IDataProtectionProvider provider, IPCPApprovalService pCPApprovalService)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _protector = provider.CreateProtector("PCPApproval.PCPApprovalController");
            _pCPApprovalService = pCPApprovalService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _pCPRegistrationService.GetAllPCPRegistration();

                foreach (var _data in data)
                {
                    var stringId = _data.UserID.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.UserID;
                }
                id = id + 1;
                ViewBag.MaxUserIdId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/PCP/PCPApproval/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPApproval/Index.cshtml");

        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPApproval/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _pCPRegistrationService.DeletePCPRegistrationAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "User has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "User has not been deleted";
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
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(guid_id));


                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PCP/PCPApproval/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPApproval/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int QPId, PCPRegistrationModel pCPRegistrationModel)
        {
            try
            {
                pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pCPRegistrationModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");

                if (ModelState.IsValid)
                {
                    var value = await _pCPRegistrationService.GetPCPRegistrationById(QPId);
                    if (await TryUpdateModelAsync<PCPRegistrationModel>(value))
                    {
                        var res = await _pCPRegistrationService.UpdatePCPRegistrationAsync(pCPRegistrationModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "User has been updated";
                        }
                        else
                        {
                            TempData["error"] = "User has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPApproval/Edit.cshtml", pCPRegistrationModel);
        }



        public async Task<JsonResult> GetJsonDataAsync(string uid)
        {
            int result = 0;
            string msg = string.Empty;
            if (uid != null)
            {
                PCPRegistrationModel pCPRegistrationModel  = new PCPRegistrationModel();
                List<string> list = new List<string>();

                String[] array = uid.Split(",");
                for (int i = 0; i < array.Length; i++)
                {
                    //list.Add(array[i]);
                    var data = await _pCPRegistrationService.GetPCPRegistrationById(Convert.ToInt32(array[i]));
                    pCPRegistrationModel.UserID = Convert.ToInt32(array[i]);
                    pCPRegistrationModel.UserName = data.UserName;
                    pCPRegistrationModel.LoginID = data.LoginID;
                    pCPRegistrationModel.MobileNo = data.MobileNo;
                    pCPRegistrationModel.EmailID = data.EmailID;
                    pCPRegistrationModel.Salt = data.Salt;
                    pCPRegistrationModel.SaltedHash = data.SaltedHash;
                    pCPRegistrationModel.IsUserActive = data.IsUserActive;
                    pCPRegistrationModel.RefID = data.RefID;
                    pCPRegistrationModel.RefType = data.RefType;
                    pCPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                    pCPRegistrationModel.InstituteId = data.InstituteId;
                    pCPRegistrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    pCPRegistrationModel.RoleId = 19;
                    pCPRegistrationModel.IsApproved = 1;

                     result = await _pCPApprovalService.CreatePCPApprovalAsync(pCPRegistrationModel);
                }
                //pCPRegistrationModel.UserList = list;
                //var data = _pCPApprovalService.CreatePCPApprovalAsync(pCPRegistrationModel);
                if(result == 1)
                {
                    msg = "saved";
                }
                msg = "not saved";
            }
            else
            {
                msg = "error";
            }
            return Json(msg);
        }
    }
}


