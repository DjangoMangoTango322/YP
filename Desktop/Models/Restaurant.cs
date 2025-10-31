using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Capacity { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string Tematic { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public Restaurant()
        {
            IsActive = true;
        }
    }
}
