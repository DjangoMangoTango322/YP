using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int ActorId { get; set; }          
        public string ActorType { get; set; }    
        public string Action { get; set; }       
        public string Entity { get; set; }       
        public string Details { get; set; }   
        public DateTime Timestamp { get; set; }
    }
}
