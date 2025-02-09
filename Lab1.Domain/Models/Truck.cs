namespace Lab1.Domain.Models
{
    public class Truck
    {
        public long TruckId { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
        public required string ColorHex { get; set; }
        public float PricePerDay { get; set; }
        public int HorsePower { get; set; }
        public required string NumberPlate { get; set; }
        public required string Status { get; set; }
        public string Type { get; set; } = "truck";
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
