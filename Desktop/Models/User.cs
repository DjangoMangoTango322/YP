using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Models
{
    public class User 
    {
        public int Id { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public string Login { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}