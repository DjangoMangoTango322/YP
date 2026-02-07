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
        public int ActorId { get; set; }           // ID пользователя или админа
        public string ActorType { get; set; }      // "User" или "Admin"
        public string Action { get; set; }         // LOGIN, CREATE, UPDATE и т.д.
        public string Entity { get; set; }         // User, Booking, Restaurant...
        public string Details { get; set; }        // дополнительные данные
        public DateTime Timestamp { get; set; }
    }
}
