using Lab1.Application.DTOs.Payment;

namespace Lab1.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<ShowPaymentListDTO>> GetPaymentsAsync(
            long? rentalId = null,
            string? email = null,
            float? amount = null,
            int? step = null,
            DateTime? paymentDate = null,
            string? paymentMethod = null,
            DateTime? createdAt = null,
            string sortBy = "paymentId",
            string sortDirection = "ASC"
            );

        Task<ShowPaymentDTO> GetPaymentByIdAsync(long id);
        Task<ShowPaymentDTO> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO);
        Task<ShowPaymentDTO> UpdatePaymentAsync(long id, UpdatePaymentDTO updatePaymentDTO);
        Task DeletePaymentAsync(long id);
    }
}
