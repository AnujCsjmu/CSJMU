using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using CoreLayout.Enum;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Models.UserManagement;
using Microsoft.AspNetCore.Http;

namespace CoreLayout.Filters
{
    public class AuthorizeContextFilter : IAsyncAuthorizationFilter
    {
        private readonly IButtonPermissionService _buttonPermissionService;
        private readonly ViewAction _viewAction;
        private readonly RegistrationModel _registrationModel;
        private readonly IConfiguration _configuration;
        public AuthorizeContextFilter(ViewAction viewAction,
            IButtonPermissionService buttonPermissionService,
            RegistrationModel registrationModel,
            IConfiguration configuration) 
        {
            _buttonPermissionService = buttonPermissionService;
            _viewAction = viewAction;
            _registrationModel = registrationModel;
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];
            int userid = 0;
            int roleid = 0;
            if (context.HttpContext.Session.GetInt32("UserId") != null && context.HttpContext.Session.GetInt32("RoleId") != null)
            {
                 userid = (int)context.HttpContext.Session.GetInt32("UserId");
                 roleid = (int)context.HttpContext.Session.GetInt32("RoleId");
            }
           
            string url = "/" + controllerName + "/" ;
            var result = await _buttonPermissionService.GetAllButtonActionPermissionAsync(_viewAction, userid, roleid,controllerName);
            //viewAction,_siteContext.RoleId.Value, _configuration.GetSection("ModuleId").Value,url
            //bool hasPermission = false;
            //if (result.Count > 0)
            //{
            //    hasPermission = true;
            //}

            if (result.Count == 0)
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "UnAuthorized",
                        }));
            }
        }
    }

    public class AuthorizeContextAttribute : TypeFilterAttribute
    {
        public AuthorizeContextAttribute(ViewAction viewAction) 
            : base(typeof(AuthorizeContextFilter))
        {
            Arguments = new object[] { viewAction };
        }
    }
}
