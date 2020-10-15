using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugZapper.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        private readonly string TicketNumber;
        private string TicketOwner { get; set; }
        private readonly string CreatedBy;
        private readonly string CreatedDate;
        private string ClosedDate { get; set; }
        private string TicketStatus { get; set; }
        private string BugDescription { get; set; }
    }
}
