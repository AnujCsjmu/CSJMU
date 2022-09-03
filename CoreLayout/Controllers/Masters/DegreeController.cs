using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using CoreLayout.Models.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using CoreLayout.Filters;
using CoreLayout.Enum;
using CoreLayout.Services.Masters.Program;
using CoreLayout.Services.Masters.Degree;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DegreeController : Controller
    {
        private readonly ILogger<DegreeController> _logger;
        private readonly IDataProtector _protector;
        private readonly IDegreeService _degreeService;
        public DegreeController(ILogger<DegreeController> logger, IDataProtectionProvider provider, IDegreeService degreeService)
        {
            _logger = logger;
            _degreeService = degreeService;
            _protector = provider.CreateProtector("Degree.DegreeController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var degree = await _degreeService.GetAllDegree();
                foreach (var _degree in degree)
                {
                    var stringId = _degree.DegreeId.ToString();
                    _degree.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxdegreeid = 0;
                foreach (var _degree in degree)
                {
                    maxdegreeid = _degree.DegreeId;
                }
                maxdegreeid = maxdegreeid + 1;
                ViewBag.MaxDegreeId = _protector.Protect(maxdegreeid.ToString());
                //end
                return View(degree

                    );

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
                var data = await _degreeService.GetDegreeById(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Create(DegreeModel degreeModel)
        {
            degreeModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            degreeModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
             
                    var res = await _degreeService.CreateDegreeAsync(degreeModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Degree has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Degree has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
               
            }
            return View(degreeModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _degreeService.GetDegreeById(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Edit(int DegreeId, DegreeModel degreeModel)
        {
            try
            {
                degreeModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                degreeModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _degreeService.GetDegreeById(DegreeId);
                    if (await TryUpdateModelAsync<DegreeModel>(value))
                    {
                        var res = await _degreeService.UpdateDegreeAsync(degreeModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Degree has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Degree has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(degreeModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _degreeService.GetDegreeById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _degreeService.DeleteDegreeAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Degree has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Degree has not been deleted";
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


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string degreeCode)
        {
           
                var already = (from degree in _degreeService.GetAllDegree().Result
                               where degree.DegreeCode == degreeCode.Trim()
                               select new SelectListItem()
                               {
                                   Text = degree.DegreeCode,
                                   Value = degree.DegreeId.ToString(),
                               }).ToList();

                if (already.Count > 0)
                {
                    return Json($"{degreeCode} is already in use.");
                }

                return Json(true);
           

        }
    }
}