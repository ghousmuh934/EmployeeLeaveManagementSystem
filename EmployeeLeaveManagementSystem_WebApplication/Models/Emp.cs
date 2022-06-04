using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class Emp
    {
       [Display(Name = "Sr No")]
        public int Id { get; set; }
        [Display(Name = "Employee Id ")]
        public string empid { get; set; }
     
        [Display(Name = "Full Name ")]
        public string FullName { get; set; }

        [Display(Name = "First Name ")]
        public string FirstName { get; set; }
       
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
       
        [Display(Name = "Email")]
        public string EmailId { get; set; }
     
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm  Password")]
   
        [Compare("Password", ErrorMessage = "The  password  and confirmation  password  do  not  match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime ? Dob { get; set; }

        //[Required(ErrorMessage = "Enter Department")]
        [Display(Name = "Department")]
        public string Department { get; set; }

       
        [Display(Name = "Address")]
        public string Address { get; set; }

        
        [Display(Name = "City")]
        public string City { get; set; }

       
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "Status")]
        public int Status { get; set; }
        
        

        public string RegDate { get; set; }
        public string ImageURL { get; set; }


        [Display(Name = "Upload  Photo")]
        public HttpPostedFileBase file { get; set; }

    }
}