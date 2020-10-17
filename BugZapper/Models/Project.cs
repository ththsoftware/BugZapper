using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugZapper.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectTitle { get; set; }
    }
}
