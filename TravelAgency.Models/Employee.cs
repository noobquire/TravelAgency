using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TravelAgency.Models.Utils;


namespace TravelAgency.Models
{
    public sealed class Employee : IPerson
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        private string PasswordHash { get; set; }

        public Employee(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == Hasher.CalculateHash(password, Username);
        }
    }
}
