using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class Department
    {

        
        public int id { get; set; }
        [Required(ErrorMessage = "Enter Department Name")]
        [Display(Name = "Department Name ")]
        public string depName { get; set; }

        [Required(ErrorMessage = "Enter Short Department Name")]
        [Display(Name = "Department Short Name ")]
        public string depShortNm { get; set; }
        [Required(ErrorMessage = "Enter Department Code")]
        [Display(Name = "Department Code ")]
        public string depCode { get; set; }

        [Display(Name = "Creation Date")]
        public string creationDate { get; set; }
    }
}