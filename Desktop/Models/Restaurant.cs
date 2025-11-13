using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public TimeSpan Open_Time { get; set; }
        [Required]
        public TimeSpan Close_Time { get; set; }
        [Required]
        public string Tematic { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}