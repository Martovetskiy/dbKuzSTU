namespace Lab1.Application.DTOs.Customer
{
    public class UpdateCustomerDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DriverLicense { get; set; }
        public required bool IsBanned { get; set; }
        public required DateTime CreateAt { get; set; }
    }
}
