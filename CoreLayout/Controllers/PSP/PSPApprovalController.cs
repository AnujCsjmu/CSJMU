using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.PSP;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PSP;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PSP
{
    public class PSPApprovalController : Controller
    {
        private readonly ILogger<PSPApprovalController> _logger;
        private readonly IPSPRegistrationService _pSPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IDataProtector _protector;
        public PSPApprovalController(ILogger<PSPApprovalController> logger, IPSPRegistrationService pSPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _pSPRegistrationService = pSPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _protector = provider.CreateProtector("PSPApproval.PSPApprovalController");
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _pSPRegistrationService.GetAllPSPRegistration();
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
                return View("~/Views/PSP/PSPApproval/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PSP/PSPApproval/Index.cshtml");
           
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pSPRegistrationService.GetPSPRegistrationById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PSP/PSPApproval/Details.cshtml", data);

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
                var value = await _pSPRegistrationService.GetPSPRegistrationById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _pSPRegistrationService.DeletePSPRegistrationAsync(value);
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
                var data = await _pSPRegistrationService.GetPSPRegistrationById(Convert.ToInt32(guid_id));
                

                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/PSP/PSPApproval/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PSP/PSPApproval/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int QPId, PSPRegistrationModel pSPRegistrationModel)
        {
            try
            {
                pSPRegistrationModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                pSPRegistrationModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
              
                if (ModelState.IsValid)
                {
                    var value = await _pSPRegistrationService.GetPSPRegistrationById(QPId);
                    if (await TryUpdateModelAsync<PSPRegistrationModel>(value))
                    {
                        var res = await _pSPRegistrationService.UpdatePSPRegistrationAsync(pSPRegistrationModel);
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
            return View("~/Views/PSP/PSPApproval/Edit.cshtml", pSPRegistrationModel);
        }
    }
}
