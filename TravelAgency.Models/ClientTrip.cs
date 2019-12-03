namespace TravelAgency.Models
{
    public sealed class ClientTrip
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}