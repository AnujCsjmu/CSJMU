using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class AuditModel :BaseEntity
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public string PageAccessed { get; set; }
        public string LoggedInAt { get; set; }
        public string LoggedOutAt { get; set; }
        public string LoginStatus { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UrlReferrer { get; set; }
        public string Area { get; set; }
        public string RoleId { get; set; }
        public string AuthorizationToken { get; set; }
        public string Userbrowser { get; set; }
        //public DateTime? ModifiedDate { get; set; }
    }
}
