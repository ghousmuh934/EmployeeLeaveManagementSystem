using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class Admin
    {
        public int id { get; set; }

        public string AdminUserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UpadationDate { get; set; }

    }
}