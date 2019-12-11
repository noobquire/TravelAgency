using System.ComponentModel.DataAnnotations;
using TravelAgency.Models.Utils;


namespace TravelAgency.Models
{
    public class Employee : IPerson
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        private string PasswordHash { get; set; }
        public bool IsAdmin { get; private set; }

        public Employee(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        private Employee() { }

        public bool CheckPassword(string password)
        {
            return PasswordHash == Hasher.CalculateHash(password, Username);
        }
    }
}
