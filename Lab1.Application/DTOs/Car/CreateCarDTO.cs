namespace Lab1.Application.DTOs.Car
{
    public class CreateCarDTO
    {
        public required string Make { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
        public required string ColorHex { get; set; }
        public float PricePerDay { get; set; }
        public required string NumberPlate { get; set; }
        public required string Status { get; set; }
        public string Type { get; set; } = "car";
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
