using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class ApplyLeave
    {
      
        public int id { get; set; }
       
        [Required(ErrorMessage = "Enter Leave Type")]
        [Display(Name = "Leave Type ")]
        public string LeaveType { get; set; }

        [Display(Name = "To Date:")]
       // [Required(ErrorMessage = "Required")]
       // [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
        public string ToDate { get; set; }


        [Display(Name = "From Date:")]
        //[Required(ErrorMessage = "Required")]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }


        
        [Display(Name = "Posting Date ")]
        public string Postingdate { get; set; }

        [Display(Name = "Admin Remarks")]
        public string AdminRemarks { get; set; }

        [Display(Name = "Admin Remarks Date")]
        public string AdminRemarksDate { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        public int Stat { get; set; }

   

        public int IsRead { get; set; }
  
        public string empid { get; set; }

        [Display(Name = "Employee Name ")]
        public string FullName { get; set; }
        public int lid { get; set; }
    }
}