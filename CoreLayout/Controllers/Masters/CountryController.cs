using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.UserManagement.ButtonPermission;
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

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CountryController : Controller
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        //private readonly IButtonPermissionService _buttonPermissionService;
        private readonly IDataProtector _protector;
        public CountryController(ILogger<CountryController> logger, ICountryService countryService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _countryService = countryService;
            //_buttonPermissionService = buttonPermissionService;
            _protector = provider.CreateProtector("Country.CountryController");
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var country = await _countryService.GetAllCountry();
                foreach (var _country in country)
                {
                    var stringId = _country.CountryId.ToString();
                    _country.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxcountryid = 0;
                foreach (var _country in country)
                {
                    maxcountryid = _country.CountryId;
                }
                maxcountryid = maxcountryid + 1;
                ViewBag.MaxCountryId = _protector.Protect(maxcountryid.ToString());
                //end
                return View(country);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _countryService.GetCountryById(Convert.ToInt32(guid_id));
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
            return View();
        }


        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create(string id)
        {
            var guid_id = _protector.Unprotect(id);
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(CountryModel countryModel)
        {
            countryModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            countryModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                int alradyExits = AlreadyExits(countryModel.CountryName.Trim());
                if (alradyExits==0)
                {
                    var res = await _countryService.CreateCountryAsync(countryModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Country has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Country has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    ModelState.AddModelError("", "Country Name already exits");
                }
               
            }
            return View(countryModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var Data = await _countryService.GetCountryById(Convert.ToInt32(guid_id));
                if (Data == null)
                {
                    return NotFound();
                }
                return View(Data);
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
        public async Task<IActionResult> Edit(int CountryId, CountryModel countryModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var dbCountry = await _countryService.GetCountryById(CountryId);
                    if (await TryUpdateModelAsync<CountryModel>(dbCountry))
                    {
                        countryModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                        countryModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                        //countryModel.CountryId = (int)HttpContext.Session.GetInt32("Id");
                        var res = await _countryService.UpdateCountryAsync(countryModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Country has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Country has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(countryModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var dbCountry = await _countryService.GetCountryById(Convert.ToInt32(guid_id));
                if (dbCountry != null)
                {
                    var res = await _countryService.DeleteCountryAsync(dbCountry);

                    if (res.Equals(1))
                    {
                        TempData["success"] = "Country has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Country has not been deleted";
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
                var already = (from country in _countryService.GetAllCountry().Result
                               where country.CountryName == name
                               select new SelectListItem()
                               {
                                   Text = country.CountryName,
                                   Value = country.CountryId.ToString(),
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
