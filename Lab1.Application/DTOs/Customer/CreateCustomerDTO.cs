namespace Lab1.Application.DTOs.Customer
{
    public class CreateCustomerDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DriverLicense { get; set; }
        public bool IsBanned { get; set; } = false;
        public required DateTime CreateAt { get; set; }
    }
}
