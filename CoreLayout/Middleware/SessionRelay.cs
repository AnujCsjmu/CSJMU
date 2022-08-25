using System; 
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Http; 

namespace CoreLayout.Middleware
{
    public class SessionRelay
    {
        private readonly RequestDelegate _next;

        public SessionRelay(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                 context.Response.Redirect("Home/Logout");
                await context.Response.WriteAsync("You're not logged in. "); 
                return;
            }

            string _sessionIPAdress = string.Empty;
            string _sessionBrowserInfo = string.Empty;
            string _CurrentUserID = string.Empty;
            string _sessionUserID = string.Empty;
            if (context.Session != null)
            {
                if (context.Request.Cookies["AuthToken"] != null)
                {
                    string _encryptedString = context.Request.Cookies["AuthToken"];
                    if(!string.IsNullOrEmpty(_encryptedString))
                    { 
                    byte[] _encodedAsBytes = System.Convert.FromBase64String(_encryptedString);
                    string _decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(_encodedAsBytes);
                    char[] _separator = new char[] { '^' };
                    if (_decryptedString != string.Empty && _decryptedString != "" && _decryptedString != null)
                    {
                        string[] _splitStrings = _decryptedString.Split(_separator);
                        if (_splitStrings.Count() > 0)
                        {
                            _sessionUserID = _splitStrings[0];
                            if (_splitStrings[1].Count() > 0)
                            {
                                string[] _userBrowserInfo = _splitStrings[2].Split('~');
                                if (_userBrowserInfo.Count() > 0)
                                {
                                    _sessionBrowserInfo = _userBrowserInfo[0];
                                    _sessionIPAdress = _userBrowserInfo[1];
                                }
                            }
                        }
                    }
                    string _currentuseripAddress;
                    if (string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"].FirstOrDefault()))
                    {
                        _currentuseripAddress = context.Connection.RemoteIpAddress.ToString();
                    }
                    else
                    {
                        _currentuseripAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    }
                    System.Net.IPAddress result;
                    if (!System.Net.IPAddress.TryParse(_currentuseripAddress, out result))
                    {
                        result = System.Net.IPAddress.None;
                    }

                    string _currentBrowserInfo = context.Request.Headers["User-Agent"].ToString();
                    _CurrentUserID = context.User.Claims.FirstOrDefault(a => a.Type == "sub").Value;
                    if (_sessionIPAdress != "" && _sessionIPAdress != string.Empty)
                    {
                        if (_sessionIPAdress != _currentuseripAddress || _sessionBrowserInfo != _currentBrowserInfo || _sessionUserID != _CurrentUserID)
                        {
                            context.Session.Clear();
                            CookieOptions optionss2 = new CookieOptions();
                            optionss2.HttpOnly = true;
                            optionss2.Secure = true;
                            optionss2.Expires = DateTime.Now.AddSeconds(-30);
                            context.Response.Cookies.Append(".AspNetCore.Session", Guid.NewGuid().ToString(), optionss2);
                            CookieOptions optionss = new CookieOptions();
                            optionss.HttpOnly = true;
                            optionss.Secure = true;
                            context.Response.Cookies.Append(".AspNetCore.Session", "", optionss);
                            context.Response.StatusCode = 401; //Bad Request                
                            await context.Response.WriteAsync("Dear User,You're not logged in. Error 401 Invalid Session");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401; //Bad Request                
                            await context.Response.WriteAsync("Dear User,You're not logged in. Error 401 Invalid Session");
                            return;
                    }
                }else
                {
                    context.Response.StatusCode = 401; //Bad Request                
                        await context.Response.WriteAsync("Dear User,You're not logged in. Error 401 Invalid Session");
                        return;
                }
                }
                else
                {
                    context.Response.StatusCode = 401; //Bad Request                
                    await context.Response.WriteAsync("Dear User,You're not logged in. Error 401 Invalid Session");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401; //Bad Request                
                await context.Response.WriteAsync("Dear User,You're not logged in. Error 401 Invalid Session");
                return;
            }

            await _next.Invoke(context);
        }
    }
}
