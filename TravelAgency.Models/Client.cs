using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TravelAgency.Models
{
    public sealed class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Middle name")]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Surname")]
        public string LastName { get; set; }
        [Phone]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
        [MaxLength(9)]
        [DisplayName("Passport")]
        public string PassportNumber { get; set; }
        [Range(0, 1)]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        public decimal Discount { get; set; }
        // TODO: switch to many-to-many relationship
        public ICollection<ClientTrip> ClientTrips { get; set; }
        [DisplayName("Trips")]
        public string TextTrips => string.Join(", ", ClientTrips.Select(ct => ct.Trip));
        public override string ToString()
        {
            return $"{FirstName} {MiddleName} {LastName}";
        }
    }
}  