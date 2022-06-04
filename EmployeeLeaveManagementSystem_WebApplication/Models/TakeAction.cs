using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class TakeAction
    {
        
        public int Id { get; set; }
        [Display(Name = "Admin Approval ")]
        public string approveoption { get; set; }

        [Display(Name = "Description ")]
        public string Description { get; set; }
       
    }
}