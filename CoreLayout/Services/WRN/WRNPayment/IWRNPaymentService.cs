using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNPayment
{
    public interface IWRNPaymentService
    {
        public Task<List<WRNPaymentModel>> GetAllWRNPaymentAsync();
        public Task<WRNPaymentModel> GetWRNPaymentByIdAsync(int id);
        public Task<int> CreateWRNPaymentAsync(WRNPaymentModel wRNPaymentModel);
        public Task<int> UpdateWRNPaymentAsync(WRNPaymentModel wRNPaymentModel);
        public Task<int> DeleteWRNPaymentAsync(WRNPaymentModel wRNPaymentModel);
    }
}
