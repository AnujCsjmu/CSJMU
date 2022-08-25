using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using CoreLayout.Services.Audit;
using CoreLayout.Models;
using CoreLayout.Models.Common;

namespace CoreLayout.Filters
{
    public class AuditFilterAttribute  : ActionFilterAttribute
    {
        private readonly IAuditService _auditService;      
       
        //private readonly ISiteContext _siteContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditFilterAttribute( IAuditService auditService ,IHttpContextAccessor httpContextAccessor)    
        {
           
           
            _auditService = auditService;
            //_siteContext = siteContext;
           
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var objaudit = new AuditModel();
            try
            {
                objaudit.ControllerName = filterContext.ActionDescriptor.RouteValues["controller"];
                objaudit.ActionName = filterContext.ActionDescriptor.RouteValues["action"];
                objaudit.UrlReferrer = "/" + objaudit.ControllerName + "/";
                //objaudit.UserId = _siteContext.UserId.ToString();
                //objaudit.RoleId = _siteContext.RoleId.ToString();
                objaudit.LoginStatus = "A";
                objaudit.Area = "Exam Master";
                var actionDescriptorRouteValues = ((ControllerBase)filterContext.Controller)
                .ControllerContext.ActionDescriptor.RouteValues;
               
                var request = filterContext.HttpContext.Request;

                //LangId AuthorizationToken
                if (filterContext.HttpContext.Request.Cookies["AuthToken"]!=null)
                {
                    objaudit.AuthorizationToken = filterContext.HttpContext.Request.Cookies["AuthToken"].ToString();
                }
                else
                {
                    objaudit.AuthorizationToken = "NA";
                }
                //IsFirstLogin Userbrowser
                if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.Headers["User-Agent"].ToString()))
                {
                    objaudit.Userbrowser = filterContext.HttpContext.Request.Headers["User-Agent"].ToString();
                }
                else
                {
                    objaudit.Userbrowser = "NA";
                }

                objaudit.SessionId = filterContext.HttpContext.Session.Id; ; // Application SessionID // User IPAddress 
                if (_httpContextAccessor.HttpContext != null)
                    objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                objaudit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path); // URL User Requested 

                objaudit.LoginStatus = "A";              
                RequestHeaders header = request.GetTypedHeaders();
                Uri uriReferer = header.Referer;

                if (uriReferer != null)
                {
                    objaudit.UrlReferrer = header.Referer.AbsoluteUri;
                }

               
                Task<bool> Result=  _auditService.InsertAuditLogs(objaudit);
              
            }
            catch (Exception)
            {
               
            }
        }

        
    }
}