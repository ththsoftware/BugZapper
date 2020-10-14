using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugZapper.Models
{
    public class User
    {
        private string UserName { get; set; }
        private int PermissionLevel { get; set; }

        private string GetAssociatedProject() 
        {
            return "UserLinkFromDatabase";
        }
        private string[] GetOwnedTickets() 
        {
            return new string[2] { "List", "Tickets" };
        }

    }
}
