using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugZapper.Models
{
    public class Project
    {
        private string ProjectTitle { get; set; }
        private string[] GetProjectTickets() 
        {
            return new string[2] { "These","Tickets"};
        }
        private string[] GetProjectUsers() 
        {
            return new string[2] { "My", "Users" };
        }
    }
}
