using CoreLayout.Models;
using CoreLayout.Models.Common;
using CoreLayout.Repositories.Audit;
using System.Threading.Tasks;


namespace CoreLayout.Services.Audit
{
    public class AuditService : IAuditService
    {
          //private readonly IUnitOfWork _unitOfWork;
          private readonly IAuditRepository _auditRepository;
           //protected readonly DataContext _dbContext;
        public AuditService(IAuditRepository auditrepository)
        {
            
            _auditRepository = auditrepository;
        }

        public Task<bool> InsertAuditLogs(AuditModel model)
        {
         
            return  _auditRepository.InsertAuditLogs(model);
        }

       
    }
}
