using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugZapper.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public readonly string TicketNumber;
        public string TicketSubject { get; set; }
        public string TicketOwner { get; set; }
        public readonly string CreatedBy;
        public readonly string CreatedDate;
        public string ClosedDate { get; set; }
        public string TicketStatus { get; set; }
        public string BugDescription { get; set; }
    }
}
