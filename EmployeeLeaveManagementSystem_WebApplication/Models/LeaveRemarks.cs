using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EmployeeLeaveManagementSystem_WebApplication.Models
{
    public class LeaveRemarks
    {
       
        public int Id { get; set; }
        [Display(Name = "Employee Id : ")]
        public string empid { get; set; }

        [Display(Name = "Full Name : ")]
        public string FullName { get; set; }
        
       
        [Display(Name = "Emp Email Id :")]
        public string EmailId { get; set; }

       

        [Display(Name = "Gender :")]
        public string Gender { get; set; }


       
        [Display(Name = "Phone Number :")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Leave Type :")]
        public string LeaveType { get; set; }

        [Display(Name = "Leave Date :")]
        public string LeaveDate { get; set; }

        [Display(Name = "Posting Date :")]
        public string PostingDate { get; set; }

        [Display(Name = "Description :")]
        public string Description { get; set; }



        [Display(Name = "Leave Status:")]
        public int Status { get; set; }

       
        [Display(Name = "Admin Remarks:")]
        public string AdminRemarks { get; set; }

        [Display(Name = "Admin Action taken date:")]
        public string AdminRemarksDate { get; set; }
    }
}