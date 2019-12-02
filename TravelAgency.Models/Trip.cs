using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Start { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime End { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        public string[] Services { get; set; }
        [DisplayName("Available services")]
        public string ServicesList => string.Join(", ", Services);
        [Required]
        [Range(0, double.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public override string ToString()
        {
            return $"{Name}, {City}";
        }
    }
}
