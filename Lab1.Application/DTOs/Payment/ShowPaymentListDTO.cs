namespace Lab1.Application.DTOs.Payment
{
    public class ShowPaymentListDTO
    {
        public long PaymentId { get; set; }
        public long RentalId { get; set; }
        public required string Email { get; set; }
        public float Amount { get; set; }
        public int Step { get; set; } = 1;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public required string PaymentMethod { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
