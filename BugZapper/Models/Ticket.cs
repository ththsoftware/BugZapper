using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugZapper.Models
{

    public class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "Ticket Number")]
        public string TicketNumber { get; set; }
        [Display(Name = "Subject")]
        public string TicketSubject { get; set; }
        [Display(Name = "Owner")]
        public string TicketOwner { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set;}
        [Display(Name = "Creation Date")]
        public DateTime CreatedDate;
        [Display(Name = "Closed Date")]
        public string ClosedDate { get; set; }
        [Display(Name = "Status")]
        public string TicketStatus 
        {
            get;
            set; 
        }
        [Display(Name = "Description")]
        public string BugDescription { get; set; }

    }
}
