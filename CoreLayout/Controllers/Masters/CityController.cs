using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Services;
using CoreLayout.Services.Masters.City;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Authorization;
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
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        public CityController(ILogger<CityController> logger, ICityService cityService, IStateService stateService, ICountryService countryService)
        {
            _logger = logger;
            _cityService = cityService;
            _stateService = stateService;
            _countryService = countryService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _cityService.GetAllCity());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _cityService.GetCityById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            var CountryList = (from country in _countryService.GetAllCountry().Result
                               select new SelectListItem()
                               {
                                   Text = country.CountryName,
                                   Value = country.CountryId.ToString(),
                               }).ToList();

            CountryList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });


            var StateList = (from state in _stateService.GetAllState().Result
                               select new SelectListItem()
                               {
                                   Text = state.StateName,
                                   Value = state.StateId.ToString(),
                               }).ToList();

            StateList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.CountryList = CountryList;
            ViewBag.StateList = StateList;
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityModel cityModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                cityModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var res = await _cityService.CreateCityAsync(cityModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "City has been saved";
                    }
                    else
                    {
                        TempData["error"] = "City has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(cityModel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var countryList = (from country in _countryService.GetAllCountry().Result
                                   select new SelectListItem()
                                   {
                                       Text = country.CountryName,
                                       Value = country.CountryId.ToString(),
                                   }).ToList();

                countryList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                var stateList = (from state in _stateService.GetAllState().Result
                                 select new SelectListItem()
                                 {
                                     Text = state.StateName,
                                     Value = state.StateId.ToString(),
                                 }).ToList();

                stateList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                ViewBag.CountryList = countryList;
                ViewBag.StateList = stateList;
                return View(await _cityService.GetCityById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CityId, CityModel cityModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                   
                    if (ModelState.IsValid)
                    {
                        var dbCity = await _cityService.GetCityById(CityId);
                        if (await TryUpdateModelAsync<CityModel>(dbCity))
                        {
                            cityModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                            cityModel.CityId = (int)HttpContext.Session.GetInt32("Id");
                            cityModel.CountryId = Convert.ToInt32(Request.Form["CountryId"]);//get country id from form
                            cityModel.StateId = Convert.ToInt32(Request.Form["StateId"]);//get country id from form
                            var res = await _cityService.UpdateCityAsync(dbCity);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "City has been updated";
                            }
                            else
                            {
                                TempData["error"] = "City has not been updated";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(cityModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbCity = await _cityService.GetCityById(id);
                    if (dbCity != null)
                    {
                        var res = await _cityService.DeleteCityAsync(dbCity);
                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "City has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "City has not been deleted";
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
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
