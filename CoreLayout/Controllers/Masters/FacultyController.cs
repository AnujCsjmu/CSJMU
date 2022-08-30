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
using CoreLayout.Services.Masters.Faculty;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class FacultyController : Controller
    {
        private readonly ILogger<FacultyController> _logger;
        private readonly IFacultyService _facultyService;
        private readonly IProgramService _programService;
        private readonly IDataProtector _protector;
      
        [Obsolete]
        public FacultyController(ILogger<FacultyController> logger, IFacultyService facultyService, IProgramService programService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _facultyService = facultyService;
            _programService = programService;
            _protector = provider.CreateProtector("Faculty.FacultyController");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var faculty = await _facultyService.GetAllFaculty();
                foreach (var _faculty in faculty)
                {
                    var stringId = _faculty.FacultyID.ToString();
                    _faculty.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxfacultyid = 0;
                foreach (var _faculty in faculty)
                {
                    maxfacultyid = _faculty.FacultyID;
                }
                maxfacultyid = maxfacultyid + 1;
                ViewBag.MaxFacultyId = _protector.Protect(maxfacultyid.ToString());
                //end
                return View(faculty);

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
                var data = await _facultyService.GetFacultyById(Convert.ToInt32(guid_id));
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
        public async Task<IActionResult> Create(string id)
        {
            try
            {
                FacultyModel facultyModel = new FacultyModel();
                facultyModel.ProgramList = await _programService.GetAllProgram();
                var guid_id = _protector.Unprotect(id);
                return View(facultyModel);
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
        public async Task<IActionResult> Create(FacultyModel facultyModel)
        {
            facultyModel.ProgramList = await _programService.GetAllProgram();
            facultyModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            facultyModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
                    var res = await _facultyService.CreateFacultyAsync(facultyModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Faculty has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Faculty has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
               
            }
            return View(facultyModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _facultyService.GetFacultyById(Convert.ToInt32(guid_id));
                data.ProgramList = await _programService.GetAllProgram();
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
        public async Task<IActionResult> Edit(int FacultyId, FacultyModel facultyModel)
        {
            try
            {
                facultyModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                facultyModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var value = await _facultyService.GetFacultyById(FacultyId);
                    if (await TryUpdateModelAsync<FacultyModel>(value))
                    {
                        var res = await _facultyService.UpdateFacultyAsync(facultyModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Faculty has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Faculty has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(facultyModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _facultyService.GetFacultyById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _facultyService.DeleteFacultyAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Faculty has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Faculty has not been deleted";
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
        public IActionResult VerifyName(string facultyName)
        {

            var already = (from faculty in _facultyService.GetAllFaculty().Result
                           where faculty.FacultyName == facultyName.Trim()
                           select new SelectListItem()
                           {
                               Text = faculty.FacultyName,
                               Value = faculty.FacultyID.ToString(),
                           }).ToList();

            if (already.Count > 0)
            {
                return Json($"{facultyName} is already in use.");
            }

            return Json(true);


        }
    }
}