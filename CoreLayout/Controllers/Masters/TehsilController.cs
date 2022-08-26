using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TehsilController : Controller
    {
        private readonly ILogger<TehsilController> _logger;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDataProtector _protector;
        private readonly ICommonService _commonService;
        private readonly CommonController _commonController;
        private readonly IDistrictService _districtService;
        private readonly ITehsilService _tehsilService;
        [Obsolete]
        private readonly IHostingEnvironment _env;

        //private readonly BaseEntity _baseEntity;
        [Obsolete]
        public TehsilController(ILogger<TehsilController> logger, IStateService stateService, ICountryService countryService, IDataProtectionProvider provider, ICommonService commonService, IHostingEnvironment env, CommonController commonController, IDistrictService districtService, ITehsilService tehsilService)
        {
            _logger = logger;
            _stateService = stateService;
            _countryService = countryService;
            _protector = provider.CreateProtector("Tehsil.TehsilController");
            _commonService = commonService;
            _env = env;
            _commonController = commonController;
            _districtService = districtService;
            _tehsilService = tehsilService;
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var tehsil = await _tehsilService.GetAllTehsil();
                foreach (var _tehsil in tehsil)
                {
                    var stringId = _tehsil.TehsilId.ToString();
                    _tehsil.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxtehsilid = 0;
                foreach (var _tehsil in tehsil)
                {
                    maxtehsilid = _tehsil.TehsilId;
                }
                maxtehsilid = maxtehsilid + 1;
                ViewBag.MaxTehsilId = _protector.Protect(maxtehsilid.ToString());
                //end
                return View(tehsil);

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
                var data = await _tehsilService.GetTehsilById(Convert.ToInt32(guid_id));
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

     

        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(string id)
        {
            try
            {
                TehsilModel tehsilModel = new TehsilModel();
                tehsilModel.DistrictList = await _districtService.GetAllDistrict();
                var guid_id = _protector.Unprotect(id);
                return View(tehsilModel);
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
        public async Task<IActionResult> Create(TehsilModel tehsilModel)
        {
            tehsilModel.DistrictList = await _districtService.GetAllDistrict();
            tehsilModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            tehsilModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
                int alradyExits = AlreadyExits(tehsilModel.TehsilName.Trim());
                if (alradyExits == 0)
                {
                    var res = await _tehsilService.CreateTehsilAsync(tehsilModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Tehsil has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Tehsil has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Tehsil Name already exits");
                }
            }
            return View(tehsilModel);
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _tehsilService.GetTehsilById(Convert.ToInt32(guid_id));
                data.DistrictList = await _districtService.GetAllDistrict();
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
        public async Task<IActionResult> Edit(int TehsilId, TehsilModel tehsilModel)
        {
            try
            {
                tehsilModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                tehsilModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var dbState = await _tehsilService.GetTehsilById(TehsilId);
                    if (await TryUpdateModelAsync<TehsilModel>(dbState))
                    {
                        //stateModel.StateId = (int)HttpContext.Session.GetInt32("Id");
                        //stateModel.CountryId = Convert.ToInt32(Request.Form["CountryId"]);//get country id from form collection
                        var res = await _tehsilService.UpdateTehsilAsync(tehsilModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Tehsil has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Tehsil has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(tehsilModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _tehsilService.GetTehsilById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _tehsilService.DeleteTehsilAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Tehsil has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Tehsil has not been deleted";
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
                var already = (from tehsil in _tehsilService.GetAllTehsil().Result
                               where tehsil.TehsilName == name
                               select new SelectListItem()
                               {
                                   Text = tehsil.TehsilName,
                                   Value = tehsil.TehsilId.ToString(),
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
