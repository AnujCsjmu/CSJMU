using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Branch;
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

namespace CoreLayout.Controllers.Masters
{
    [Authorize(Roles = "Administrator")]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IDataProtector _protector;
        private readonly IBranchService _branchService;
        public BranchController(ILogger<BranchController> logger, IDataProtectionProvider provider, IBranchService branchService)
        {
            _logger = logger;
            _branchService = branchService;
            _protector = provider.CreateProtector("Branch.BranchController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _branchService.GetAllBranch();
                foreach (var _data in data)
                {
                    var stringId = _data.BranchID.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxbranchid = 0;
                foreach (var _data in data)
                {
                    maxbranchid = _data.BranchID;
                }
                maxbranchid = maxbranchid + 1;
                ViewBag.MaxBranchId = _protector.Protect(maxbranchid.ToString());
                //end
                return View(data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _branchService.GetBranchById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public IActionResult Create(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                return View();
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
        public async Task<IActionResult> Create(BranchModel branchModel)
        {
            branchModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            branchModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
            branchModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {

                var res = await _branchService.CreateBranchAsync(branchModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Branch has been saved";
                }
                else
                {
                    TempData["error"] = "Branch has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View(branchModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _branchService.GetBranchById(Convert.ToInt32(guid_id));
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int BranchId, BranchModel branchModel)
        {
            try
            {
                branchModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                branchModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                branchModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _branchService.GetBranchById(BranchId);
                    if (await TryUpdateModelAsync<BranchModel>(value))
                    {
                        var res = await _branchService.UpdateBranchAsync(branchModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Branch has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Branch has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(branchModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _branchService.GetBranchById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    //var res = await _branchService.DeleteBranchAsync(value);
                    //if (res.Equals(1))
                    //{
                    //    TempData["success"] = "Branch has been deleted";
                    //}
                    //else
                    //{
                    //    TempData["error"] = "Branch has not been deleted";
                    //}
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


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string branchCode)
        {

            var already = (from data in _branchService.GetAllBranch().Result
                           where data.BranchCode == branchCode.Trim()
                           select new SelectListItem()
                           {
                               Text = data.BranchCode,
                               Value = data.BranchID.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{branchCode} is already in use.");
            }

            return Json(true);


        }
    }
}
