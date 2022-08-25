using CoreLayout.Models;
using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Audit
{
    public interface IAuditService
    {
        Task<bool> InsertAuditLogs(AuditModel model);
    }
}
