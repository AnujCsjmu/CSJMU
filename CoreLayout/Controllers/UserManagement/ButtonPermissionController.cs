﻿using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Services.UserManagement.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ButtonPermissionController : Controller
    {
        private readonly ILogger<ButtonPermissionController> _logger;
        private readonly IButtonPermissionService _buttonPermissionService;
        private readonly IRoleService _roleService;
        private readonly IButtonService _buttonService;
        private readonly CommonController _commonController;
        private readonly IMenuService _menuService;
        public ButtonPermissionController(ILogger<ButtonPermissionController> logger, IButtonPermissionService buttonPermissionService, IRoleService roleService, IButtonService buttonService, CommonController commonController, IMenuService menuService)
        {
            _logger = logger;
            _buttonPermissionService = buttonPermissionService;
            _roleService = roleService;
            _buttonService = buttonService;
            _commonController = commonController;
            _menuService = menuService;
        }
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            bindRoleDropdown();
            List<ButtonPermissionModel> buttonPermissionModels = new List<ButtonPermissionModel>();
            var AllButtonPermissionData = (dynamic)null;
            if (HttpContext.Session.GetString("SearchList")==null)
            {
                 AllButtonPermissionData = await _buttonPermissionService.DistinctButtonPermissionAsync();
            }
            else
            {
                int SearchRoleId = (int)HttpContext.Session.GetInt32("SearchRoleId");
                int SearchUserId = (int)HttpContext.Session.GetInt32("SearchUserId");
                 AllButtonPermissionData = await _buttonPermissionService.GetAllButtonPermissionUserWiseAsync(SearchUserId);
            }
            //var AllButtonPermissionData = await _menuService.GetAllMenuAsync();
            foreach (var _allButtonPermissionData in AllButtonPermissionData)
            {
                ButtonPermissionModel buttonPermission = new ButtonPermissionModel();
                buttonPermission.Controller = _allButtonPermissionData.Controller;
               // buttonPermission.Action = _allButtonPermissionData.Action;
                buttonPermission.URL = _allButtonPermissionData.URL;
                buttonPermission.UserName = _allButtonPermissionData.UserName;
                buttonPermission.RoleName = _allButtonPermissionData.RoleName;
                buttonPermission.UserId = _allButtonPermissionData.UserId;
                buttonPermission.RoleId = _allButtonPermissionData.RoleId;

                List<ButtonModel> buttonModellist  = new List<ButtonModel>();
                ButtonPermissionModel buttonPermissionModel = new ButtonPermissionModel();
                var ButtonPermissionDataUserWise = await _buttonPermissionService.GetAllButtonPermissionUserWiseAsync(_allButtonPermissionData.UserId);
                var AllButtonsData = await _buttonService.GetAllButton();
                foreach (var _allButtonsData in AllButtonsData)
                {
                    ButtonModel buttonModel = new ButtonModel();
                    buttonModel.ButtonName = _allButtonsData.ButtonName;
                    foreach (var item3 in ButtonPermissionDataUserWise)
                    {
                        if (_allButtonsData.ButtonId == item3.ButtonId )
                        {
                            buttonModel.isChecked = "checked";
                         
                        }
                    }
                    buttonModellist.Add(buttonModel);

                }

                buttonPermission.ButtonModelList = buttonModellist;
                buttonPermissionModels.Add(buttonPermission);
            }
           // HttpContext.Session.SetString("SearchList",null);
            return View(buttonPermissionModels);
        }

        //[HttpGet("[search]")]
        public async Task<IActionResult> Search()
        {
            try
            {
                int RoleId = 1;//Convert.ToInt32(Request.Form["RoleId"]);
                int UserId = 13353; //Convert.ToInt32(Request.Form["UserId"]);

                List<ButtonPermissionModel> permissionModels = new List<ButtonPermissionModel>();
                permissionModels = await _buttonPermissionService.GetAllButtonPermissionAsync();
                var result = permissionModels.Where(x => x.RoleId == RoleId && x.UserId==UserId);
                if (result == null)
                {
                    TempData["error"] = "User has not been updated";
                    return View(result);
                }

                HttpContext.Session.SetString("SearchList", result.ToString());
                HttpContext.Session.SetInt32("SearchRoleId", RoleId);
                HttpContext.Session.SetInt32("SearchUserId", UserId);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction("Index");
        }

        //[HttpGet("[save]")]
        public IActionResult Save()
        {
            return View();
        }
        public void bindRoleDropdown()
        {
            var ControllerList = (from menu in _menuService.GetAllMenuAsync().Result
                                  select new SelectListItem()
                                  {
                                      Text = menu.Controller,
                                      Value = menu.Controller.ToString(),
                                  }).ToList();
            ControllerList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.ControllerList = ControllerList;


            var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleID.ToString(),
                            }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.RoleList = RoleList;

            var GetButtonList = (from button in _buttonService.GetAllButton().Result
                                 select new SelectListItem()
                                 {
                                     Text = button.ButtonName,
                                     Value = button.ButtonId.ToString(),
                                 }).ToList();
            //GetButtonList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = string.Empty
            //});

            ViewBag.GetButtonList = GetButtonList;
        }
        public void bindUserDropdown(int RoleId)
        {

            var GetUserList = (from user in _buttonPermissionService.GetAllUsersAsync(RoleId).Result
                               select new SelectListItem()
                               {
                                   Text = user.UserName,
                                   Value = user.UserID.ToString(),
                               }).ToList();
            GetUserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.GetUserList = GetUserList;
        }
        //Create Get Action Method
        [AuthorizeContext(ViewAction.Add)]
        public ActionResult Create()
        {
            bindRoleDropdown();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(ButtonPermissionModel buttonPermissionModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {


                StringBuilder stringBuilder = new StringBuilder();
                buttonPermissionModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                buttonPermissionModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                int buttons = 0;
                foreach (int buttonid in buttonPermissionModel.ButtonList)
                {
                    int res = _commonController.AlreadyExitsButtonPermission(buttonid, (int)buttonPermissionModel.CreatedBy, buttonPermissionModel.RoleId, buttonPermissionModel.Controller, buttonPermissionModel.Action);
                    if (res > 0)
                    {
                        buttons = 1;
                    }
                }

                if (buttons == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var res = await _buttonPermissionService.CreateButtonPermissionAsync(buttonPermissionModel);
                        if (res > 0)
                        {
                            TempData["success"] = "ButtonPermission has been saved";
                        }
                        else
                        {
                            TempData["error"] = "ButtonPermission has not been saved";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    string msg = "already permission to this user " + buttonPermissionModel.UserName + " of the this page " + buttonPermissionModel.Controller + "/" + buttonPermissionModel.Action + "";
                    ModelState.AddModelError("", msg);
                }

            }
            bindRoleDropdown();
            bindUserDropdown(buttonPermissionModel.RoleId);
            return View(buttonPermissionModel);
        }

        public JsonResult GetUser(int RoleId)
        {
            var GetUserList = (from user in _buttonPermissionService.GetAllUsersAsync(RoleId).Result
                               select new SelectListItem()
                               {
                                   Text = user.UserName,
                                   Value = user.UserID.ToString(),
                               }).ToList();
            GetUserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            //var GetUserList = _buttonPermissionService.GetUserByRoleAsync(role);

            return Json(GetUserList);
        }
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _buttonPermissionService.GetButtonPermissionByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var list = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);

                bindRoleDropdown();
                bindUserDropdown(list.RoleId);
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int id, ButtonPermissionModel buttonPermissionModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    buttonPermissionModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    buttonPermissionModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                    int buttons = 0;
                    foreach (int buttonid in buttonPermissionModel.ButtonList)
                    {
                        int result = _commonController.AlreadyExitsButtonPermission(buttonid, (int)buttonPermissionModel.ModifiedBy, buttonPermissionModel.RoleId, buttonPermissionModel.Controller, buttonPermissionModel.Action);
                        if (result > 0)
                        {
                            buttons = 1;
                        }
                    }
                    if (buttons == 0)
                    {
                        if (ModelState.IsValid)
                        {
                            var dbRole = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);
                            if (await TryUpdateModelAsync<ButtonPermissionModel>(dbRole))
                            {
                                buttonPermissionModel.UserId = (int)HttpContext.Session.GetInt32("Id");
                                var res = await _buttonPermissionService.UpdateButtonPermissionAsync(buttonPermissionModel);
                                if (res.ToString().Equals("1"))
                                {
                                    TempData["success"] = "ButtonPermission has been updated";
                                }
                                else
                                {
                                    TempData["error"] = "ButtonPermission has not been updated";
                                }
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    else
                    {
                        string msg = "already permission to this user " + buttonPermissionModel.UserName + " of the this page " + buttonPermissionModel.Controller + "/" + buttonPermissionModel.Action + "";
                        ModelState.AddModelError("", msg);
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
            bindRoleDropdown();
            bindUserDropdown(buttonPermissionModel.RoleId);
            return View(buttonPermissionModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {

                    var value = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);
                    if (value != null)
                    {
                        var res = await _buttonPermissionService.DeleteButtonPermissionAsync(value);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "ButtonPermission has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "ButtonPermission has not been deleted";
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