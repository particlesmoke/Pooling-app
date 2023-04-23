namespace Pooling_Backend.Models
{
    public class Ride
    {
        public Guid Id { get; set; }
        public string DepartureTime { get; set; }
        public string DepartureLocation { get; set; }
        public string Destination { get; set; }
        public int Capacity { get; set; }
        public int OccupiedCapacity { get; set; }
        public bool IsBooked { get; set; }

    }


}
