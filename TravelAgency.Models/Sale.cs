using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public sealed class Sale
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        [MaxLength(50)]
        [DisplayName("Employee name")]
        public string EmployeeFirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Employee middle Name")]
        public string EmployeeMiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Employee surname")]
        public string EmployeeLastName { get; set; }
        [Required]
        public Trip Trip { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayName("Trips sold")]
        public int SalesCount { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayName("Trips sold")]
        public int TripsLeft { get; set; }
        [DisplayName("Last trip sale")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}")]
        public DateTime LastSale { get; set; }
    }
}
