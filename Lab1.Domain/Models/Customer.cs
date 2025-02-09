namespace Lab1.Domain.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DriverLicense { get; set; }
        public bool IsBanned { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
