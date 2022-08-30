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

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProgramController : Controller
    {
        private readonly ILogger<ProgramController> _logger;
        private readonly IDataProtector _protector;
        private readonly IProgramService _programService;
        public ProgramController(ILogger<ProgramController> logger, IDataProtectionProvider provider, IProgramService programService)
        {
            _logger = logger;
            _programService = programService;
            _protector = provider.CreateProtector("Program.ProgramController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var program = await _programService.GetAllProgram();
                foreach (var _program in program)
                {
                    var stringId = _program.ProgramId.ToString();
                    _program.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxprogramid = 0;
                foreach (var _program in program)
                {
                    maxprogramid = _program.ProgramId;
                }
                maxprogramid = maxprogramid + 1;
                ViewBag.MaxProgramId = _protector.Protect(maxprogramid.ToString());
                //end
                return View(program);

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
                var data = await _programService.GetProgramById(Convert.ToInt32(guid_id));
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

        //public void BindCountry()
        //{
        //    var countryList = (from country in _countryService.GetAllCountry().Result
        //                       select new SelectListItem()
        //                       {
        //                           Text = country.CountryName,
        //                           Value = country.CountryId.ToString(),
        //                       }).ToList();

        //    countryList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    ViewBag.CountryList = countryList;//roleList.Select(l => l.CountryId).ToList();
        //}

        //Create Get Action Method
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
        public async Task<IActionResult> Create(ProgramModel programModel)
        {
            programModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            programModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
             
                    var res = await _programService.CreateProgramAsync(programModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Program has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Program has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
               
            }
            return View(programModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _programService.GetProgramById(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Edit(int ProgramId, ProgramModel programModel)
        {
            try
            {
                programModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                programModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _programService.GetProgramById(ProgramId);
                    if (await TryUpdateModelAsync<ProgramModel>(value))
                    {
                        var res = await _programService.UpdateProgramAsync(programModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Program has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Program has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(programModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _programService.GetProgramById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _programService.DeleteProgramAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Program has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Program has not been deleted";
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
        public IActionResult VerifyName(string programName)
        {
           
                var already = (from program in _programService.GetAllProgram().Result
                               where program.ProgramName == programName.Trim()
                               select new SelectListItem()
                               {
                                   Text = program.ProgramName,
                                   Value = program.ProgramId.ToString(),
                               }).ToList();

                if (already.Count > 0)
                {
                    return Json($"{programName} is already in use.");
                }

                return Json(true);
           

        }
    }
}