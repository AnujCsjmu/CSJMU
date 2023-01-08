using CoreLayout.Models.WRN;
using CoreLayout.Repositories.WRN.WRNPayment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNPayment
{
    public class WRNPaymentService : IWRNPaymentService
    {
        private readonly IWRNPaymentRepository _wRNPaymentRepository;

        public WRNPaymentService(IWRNPaymentRepository wRNPaymentRepository)
        {
            _wRNPaymentRepository = wRNPaymentRepository;
        }

        public async Task<List<WRNPaymentModel>> GetAllWRNPaymentAsync()
        {
            return await _wRNPaymentRepository.GetAllAsync();
        }

        public async Task<WRNPaymentModel> GetWRNPaymentByIdAsync(int id)
        {
            return await _wRNPaymentRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateWRNPaymentAsync(WRNPaymentModel wRNPaymentModel)
        {
            return await _wRNPaymentRepository.CreateAsync(wRNPaymentModel);
        }

        public async Task<int> UpdateWRNPaymentAsync(WRNPaymentModel wRNPaymentModel)
        {
            return await _wRNPaymentRepository.UpdateAsync(wRNPaymentModel);
        }

        public async Task<int> DeleteWRNPaymentAsync(WRNPaymentModel wRNPaymentModel)
        {
            return await _wRNPaymentRepository.DeleteAsync(wRNPaymentModel);
        }
    }
}
