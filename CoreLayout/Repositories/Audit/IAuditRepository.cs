using CoreLayout.Models;
using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Audit
{
    public interface IAuditRepository : IRepository<AuditModel>
    {
        Task<bool> InsertAuditLogs(AuditModel model);
    }
}
