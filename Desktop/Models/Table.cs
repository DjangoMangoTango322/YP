using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; } // "window", "center", "terrace", etc.
        public bool IsAvailable { get; set; }

        public Table()
        {
            IsAvailable = true;
        }
    }
}
