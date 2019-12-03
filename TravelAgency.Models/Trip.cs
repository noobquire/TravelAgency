using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Models
{
    public sealed class Trip
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(300)]
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        public string[] Services { get; set; }
        [DisplayName("Available services")]
        public string ServicesList => string.Join(", ", Services);
        [Required]
        public decimal Price { get; set; }
        [DisplayName("Amount of trips")]
        public int AmountOfTrips { get; set; }
        public override string ToString()
        {
            return $"{Name}, {City}";
        }
    }
}
