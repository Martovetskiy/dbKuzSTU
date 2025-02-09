namespace Lab1.Application.DTOs.Rental
{
    public class CreateRentalDTO
    {
        public long CustomerId { get; set; }
        public long CarId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public float TotalPrice { get; set; } = 1.0f;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
