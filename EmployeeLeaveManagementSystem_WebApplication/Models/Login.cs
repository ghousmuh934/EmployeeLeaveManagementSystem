using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class Login
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [Display(Name = "Email")]
        public string UserEmail{ get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UpadationDate { get; set; }
    }
}