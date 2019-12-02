using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TravelAgency.Models.Utils;


namespace TravelAgency.Models
{
    public sealed class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Employee(string firstName, string middleName, string lastName, string username, string passwordHash)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
        }

        public Employee()
        {
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == Hasher.CalculateHash(password, Username);
        }
    }
}
