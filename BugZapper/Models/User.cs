using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugZapper.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Permission Level")]
        public int PermissionLevel { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}
