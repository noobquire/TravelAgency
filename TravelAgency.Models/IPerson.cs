namespace TravelAgency.Models
{
    public interface IPerson
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
    }
}