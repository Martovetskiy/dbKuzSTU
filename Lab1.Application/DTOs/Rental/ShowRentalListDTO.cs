namespace Lab1.Application.DTOs.Rental
{
    public class ShowRentalListDTO
    {
        public long RentalId { get; set; }
        public long CustomerId { get; set; }
        public long CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public float TotalPrice { get; set; } = 1.0f;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
