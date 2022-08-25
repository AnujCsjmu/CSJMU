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
using CoreLayout.Services.Masters.District;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DistrictController : Controller
    {
        private readonly ILogger<DistrictController> _logger;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDataProtector _protector;
        private readonly ICommonService _commonService;
        private readonly CommonController _commonController;
        private readonly IDistrictService _districtService;
        [Obsolete]
        private readonly IHostingEnvironment _env;

        //private readonly BaseEntity _baseEntity;
        [Obsolete]
        public DistrictController(ILogger<DistrictController> logger, IStateService stateService, ICountryService countryService, IDataProtectionProvider provider, ICommonService commonService, IHostingEnvironment env, CommonController commonController, IDistrictService districtService)
        {
            _logger = logger;
            _stateService = stateService;
            _countryService = countryService;
            _protector = provider.CreateProtector("District.DistrictController");
            _commonService = commonService;
            _env = env;
            _commonController = commonController;
            _districtService = districtService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var distict = await _districtService.GetAllDistrict();
                foreach (var _distict in distict)
                {
                    var stringId = _distict.DistrictId.ToString();
                    _distict.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxdistrictid = 0;
                foreach (var _distict in distict)
                {
                    maxdistrictid = _distict.DistrictId;
                }
                maxdistrictid = maxdistrictid + 1;
                ViewBag.MaxDistrictId = _protector.Protect(maxdistrictid.ToString());
                //end
                return View(distict);

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
                var data = await _districtService.GetDistrictById(Convert.ToInt32(guid_id));
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
                DistrictModel districtModel = new DistrictModel();
                districtModel.StateList = await _stateService.GetAllState();
                var guid_id = _protector.Unprotect(id);
                return View(districtModel);
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
        public async Task<IActionResult> Create(DistrictModel districtModel)
        {
            districtModel.StateList = await _stateService.GetAllState();
            districtModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            districtModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
                int alradyExits = AlreadyExits(districtModel.DistrictName.Trim());
                if (alradyExits == 0)
                {
                    var res = await _districtService.CreateDistrictAsync(districtModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "District has been saved";
                    }
                    else
                    {
                        TempData["error"] = "District has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "District Name already exits");
                }
            }
            return View(districtModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _districtService.GetDistrictById(Convert.ToInt32(guid_id));
                data.StateList = await _stateService.GetAllState();
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
        public async Task<IActionResult> Edit(int DistrictId, DistrictModel districtModel)
        {
            try
            {
                districtModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                districtModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var dbState = await _districtService.GetDistrictById(DistrictId);
                    if (await TryUpdateModelAsync<DistrictModel>(dbState))
                    {
                        //stateModel.StateId = (int)HttpContext.Session.GetInt32("Id");
                        //stateModel.CountryId = Convert.ToInt32(Request.Form["CountryId"]);//get country id from form collection
                        var res = await _districtService.UpdateDistrictAsync(districtModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "District has been updated";
                        }
                        else
                        {
                            TempData["error"] = "District has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(districtModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _districtService.GetDistrictById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _districtService.DeleteDistrictAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "District has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "District has not been deleted";
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

        public int AlreadyExits(string name)
        {
            int result = 0;
            try
            {
                var already = (from district in _districtService.GetAllDistrict().Result
                               where district.DistrictName == name
                               select new SelectListItem()
                               {
                                   Text = district.DistrictName,
                                   Value = district.DistrictId.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
        }
    }
}