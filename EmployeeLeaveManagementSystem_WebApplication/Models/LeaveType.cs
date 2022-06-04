using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class LeaveType
    {
      
        public int id { get; set; }
        [Required(ErrorMessage = "Enter Leave Type")]
        [Display(Name = "Leave Type ")]
        public string LeaveTyp { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Creation Date")]
        public string creationDate { get; set; }
        
    }
}

