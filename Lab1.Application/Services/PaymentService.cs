using Lab1.Application.DTOs.Payment;
using Lab1.Application.Interfaces;

namespace Lab1.Application.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<ShowPaymentListDTO>> GetPaymentsAsync(
            long? rentalId = null,
            string? email = null,
            float? amount = null,
            int? step = null,
            DateTime? paymentDate = null,
            string? paymentMethod = null,
            DateTime? createdAt = null,
            string sortBy = "paymentId",
            string sortDirection = "ASC")
        {
            var result = await _paymentRepository.GetPaymentsAsync(rentalId, email, amount, step,
                paymentDate, paymentMethod, createdAt, sortBy, sortDirection);
            return result;
        }

        public async Task<ShowPaymentDTO> GetPaymentByIdAsync(long id)
        {
            var result = await _paymentRepository.GetPaymentByIdAsync(id);
            return result;
        }

        public async Task<ShowPaymentDTO> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO)
        {
            var result = await _paymentRepository.CreatePaymentAsync(createPaymentDTO);
            return result;
        }

        public async Task<ShowPaymentDTO> UpdatePaymentAsync(long id, UpdatePaymentDTO updatePaymentDTO)
        {
            var result = await _paymentRepository.UpdatePaymentAsync(id, updatePaymentDTO);
            return result;
        }

        public async Task DeletePaymentAsync(long id)
        {
            await _paymentRepository.DeletePaymentAsync(id);
        }
    }
}
