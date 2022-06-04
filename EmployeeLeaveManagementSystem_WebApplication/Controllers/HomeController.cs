using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using PagedList;
using PagedList.Mvc;
using EmployeeLeaveManagementSystem_WebApplication.Models;
using EmployeeLeaveManagementSystem_WebApplication.Controllers;
namespace EmployeeLeaveManagementSystem_WebApplication.Controllers
{
    public class HomeController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
     
        private List<SelectListItem> getGend()
        {
            List<SelectListItem> GenList = new List<SelectListItem>();
            GenList.Add(new SelectListItem { Text = "Male", Value = "Male" });
            GenList.Add(new SelectListItem { Text = "Female", Value = "Female" });
            GenList.Add(new SelectListItem { Text = "Other", Value = "Other" });
            return GenList;
        }


        private List<SelectListItem> getDept()
        {
            List<SelectListItem> Depart = new List<SelectListItem>();

            con.Open();
            string q;

            q = "select distinct DepartmentShortName from Department";

            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Depart.Add(new SelectListItem { Text = sdr[0].ToString(), Value = sdr[0].ToString() });
            }
            sdr.Close();
            con.Close();


            return Depart;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(Admin a)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand("select * from AdminTbl where username='" + a.AdminUserName + "' and password='" + a.Password + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
               
                Session["uid"] = dr["id"].ToString();
                Session["uname"] = dr["username"].ToString();
                return RedirectToAction("Dashboard");
            }
            else
            {
                dr.Close();
                ViewBag.res = "Not Record Found with this email and password try again";
            }
            return View();

        }


        public ActionResult PassRecovery()
        {

            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            return View();
        }

       
        [HttpGet]
        public ActionResult AddEmployee()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            else
            {
               
                ViewBag.GenList = getGend();
                ViewBag.Deprt = getDept();

                return View();
            }
        }

        [HttpPost]
        public ActionResult AddEmployee(Emp a)
        {
            if (ModelState.IsValid)
            {
                var allowedExtensions = new[] { ".bmp", ".png", ".jpg", ".gif" };

                var ext = Path.GetExtension(a.file.FileName);

                //getting  the  extension(ex-.jpg)
                if (allowedExtensions.Contains(ext))  //check  what  type of  extension
                {
                    //~/Images  is  relative  path  for  images  in  root  directory
                    var path = Path.Combine(Server.MapPath("~/Images"), a.file.FileName);
                    a.ImageURL = "~/Images/" + a.file.FileName;
                    //saving  photo  of  employee  in  the  image  folder
                    //  file.SaveAs  Saves  the  contents  of  an  uploaded  file  to  a  specified path on the Web server.
                    a.file.SaveAs(path);

                }
                else
                {
                    ViewBag.message = "Please  choose  only  Image  file";
                    return View(a);
                }
                a.Status = 0;

                SqlConnection con = new SqlConnection(constr);
                con.Open();
                //Response.Write("<script> alert('Connect with server!');</script>");
              
                string query = "insert into employee (empid,FirstName,LastName,EmailId,Password,Gender,Dob,Department,Address,City,Country,PhoneNumber,Status,RegDate,ImageUrl) values ('" + a.empid + "','" + a.FirstName + "','" + a.LastName + "','" + a.EmailId + "','" + a.Password + "','" + a.Gender + "','" + a.Dob + "','" + a.Department + "','" + a.Address + "','" + a.City + "','" + a.Country + "','" + a.PhoneNumber + "','" + a.Status + "','" + DateTime.Now + "','" + a.ImageURL + "')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script> alert('Record saved!');</script>");
                return RedirectToAction("Dashboard");

            }
            return View();


        }

        //*********************Edit Button Code*****************
        [HttpGet]
        public ActionResult EditEmployee(int aid)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string q = "Select * from employee where id ='" + aid + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            Emp a = new Emp();
            ViewBag.GenList = getGend();
            ViewBag.Deprt = getDept();
            a.Id = int.Parse(sdr["id"].ToString());
            a.empid = sdr["empid"].ToString();
            a.FirstName = sdr["FirstName"].ToString();
            a.LastName = sdr["LastName"].ToString();
            a.EmailId = sdr["EmailId"].ToString();
            a.Password = sdr["Password"].ToString();
            a.Gender = sdr["Gender"].ToString();    //problem herer
            a.Dob = DateTime.Parse(sdr["Dob"].ToString());
            a.Department = sdr["Department"].ToString();
            a.Address = sdr["Address"].ToString();
            a.City = sdr["City"].ToString();
            a.Country = sdr["Country"].ToString();
            a.PhoneNumber = sdr["PhoneNumber"].ToString();
            a.Status = int.Parse(sdr["Status"].ToString());
            a.RegDate = sdr["RegDate"].ToString();
            sdr.Close();
            con.Close();

            return View(a);
        }
        [HttpPost]
        public ActionResult EditEmployee(Emp a)
        {

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("update employee set empid='" + a.empid + "',FirstName='" + a.FirstName + "',LastName='" + a.LastName + "',EmailId='" + a.EmailId + "',Password='" + a.Password + "',Gender='" + a.Gender + "',Dob='" + a.Dob + "',Department='" + a.Department + "',Address='" + a.Address + "',City='" + a.City + "',Country='" + a.Country + "',PhoneNumber='" + a.PhoneNumber + "',Status='" + a.Status + "',RegDate='" + DateTime.Now + "' where id ='" + a.Id + "' ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageEmployee");
        }
        //*********************Edit Button Code End*************
        private List<Emp> getManageEmp()
        {            
            List<Emp> alist = new List<Emp>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();            
            string q = "Select ImageUrl,id,empid,FirstName+LastName as [Full Name],Department,dob,Status,RegDate from employee";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();            
            while (sdr.Read())
            {
                Emp a = new Emp();
                a.ImageURL = sdr["ImageUrl"].ToString();
                //a.Id = int.Parse(sdr["id"].ToString());
                a.empid = sdr["empid"].ToString();
                a.FullName = sdr["Full Name"].ToString();
                a.Department = sdr["Department"].ToString();
                a.Dob = DateTime.Parse(sdr["Dob"].ToString());
                a.Status = int.Parse(sdr["Status"].ToString());
                if (a.Status == 0)
                {
                    ViewBag.Act = 0;
                }
                else
                {
                    ViewBag.Act = 1;
                }
                a.RegDate = sdr["RegDate"].ToString();
          
                alist.Add(a);
            }

            con.Close();
            return alist;
        }
        public ActionResult ManageEmployee(int? page)
        {
            if (Session["uid"] == null && Session["uname"] == null)
            { 
                return RedirectToAction("AdminLogin");
            }
            else
            {
                List<Emp> al = getManageEmp();
                return View(al.ToPagedList(page ?? 1, 3));
            }           
        }

        [HttpGet]
        public ActionResult Delete(int aid)
        {

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete from employee where id ='" + aid + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageEmployee");
        }

        
        //*******************************************Leave Management *********************

        
        private List<ApplyLeave> getLeaveHistory()
        {


            List<ApplyLeave> alist = new List<ApplyLeave>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            
            //string q = "select a.FirstName+' '+a.LastName+'('+a.empid+')' as [Full Name],b.leaveType,b.postingdate,b.status from AllLeave_Tbl b inner join employee a on a.id = b.eid";
            string q = "SELECT AllLeave_Tbl.id as lid,employee.FirstName+' '+employee.LastName as [Full Name],employee.id,AllLeave_Tbl.LeaveType,AllLeave_Tbl.PostingDate,AllLeave_Tbl.Status, ROW_NUMBER() over (order by AllLeave_Tbl.id) as [#] from AllLeave_Tbl join employee on AllLeave_Tbl.eid=employee.id where AllLeave_Tbl.Status = 0";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
          
            while (sdr.Read())
            {
                ApplyLeave a = new ApplyLeave();
                a.id = int.Parse(sdr["#"].ToString());
                a.lid = int.Parse(sdr["lid"].ToString());
                a.FullName = sdr["Full Name"].ToString();
                a.LeaveType = sdr["LeaveType"].ToString();
                // a.ToDate = sdr["ToDate"].ToString();
                //a.FromDate = sdr["FromDate"].ToString();
                //a.Description = sdr["Description"].ToString();
                a.Postingdate = sdr["Postingdate"].ToString();
                a.Stat = int.Parse(sdr["Status"].ToString());

                if (a.Stat == 0)
                {
                    ViewBag.App = 0;
                }
                else if (a.Stat == 1)
                {
                    ViewBag.App = 1;
                }
                else
                {
                    ViewBag.App = 2;
                }

                alist.Add(a);
            }
           

            con.Close();
            return alist;
        }
        public ActionResult LeaveHistoryAdmin()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            else
            {

                List<ApplyLeave> lh = getLeaveHistory();
                return View(lh);
            }

        }
        //------------------------For Approved List-------------------------------
        private List<ApplyLeave> getLeaveHistoryAdminApp()
        {


            List<ApplyLeave> alist = new List<ApplyLeave>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();


            //string q = "select a.FirstName+' '+a.LastName+'('+a.empid+')' as [Full Name],b.leaveType,b.postingdate,b.status from AllLeave_Tbl b inner join employee a on a.id = b.eid";
            string q = "SELECT AllLeave_Tbl.id as lid,employee.FirstName+' '+employee.LastName as [Full Name],employee.id,AllLeave_Tbl.LeaveType,AllLeave_Tbl.PostingDate,AllLeave_Tbl.Status, ROW_NUMBER() over (order by AllLeave_Tbl.id) as [#] from AllLeave_Tbl join employee on AllLeave_Tbl.eid=employee.id where AllLeave_Tbl.Status = 1";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                ApplyLeave a = new ApplyLeave();
                a.id = int.Parse(sdr["#"].ToString());
                a.lid = int.Parse(sdr["lid"].ToString());
                a.FullName = sdr["Full Name"].ToString();
                a.LeaveType = sdr["LeaveType"].ToString();
                // a.ToDate = sdr["ToDate"].ToString();
                //a.FromDate = sdr["FromDate"].ToString();
                //a.Description = sdr["Description"].ToString();
                a.Postingdate = sdr["Postingdate"].ToString();
                a.Stat = int.Parse(sdr["Status"].ToString());

                if (a.Stat == 0)
                {
                    ViewBag.App = 0;
                }
                else if (a.Stat == 1)
                {
                    ViewBag.App = 1;
                }
                else
                {
                    ViewBag.App = 2;
                }

                alist.Add(a);
            }


            con.Close();
            return alist;
        }
        public ActionResult LeaveHistoryAdminApp()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            else
            {

                List<ApplyLeave> lh = getLeaveHistoryAdminApp();
                return View(lh);
            }

        }

        //-----------------------------------------------------------------------------
        //------------------------For Not Approved List-------------------------------
        private List<ApplyLeave> getLeaveHistoryAdminNtApp()
        {


            List<ApplyLeave> alist = new List<ApplyLeave>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();


            //string q = "select a.FirstName+' '+a.LastName+'('+a.empid+')' as [Full Name],b.leaveType,b.postingdate,b.status from AllLeave_Tbl b inner join employee a on a.id = b.eid";
            string q = "SELECT AllLeave_Tbl.id as lid,employee.FirstName+' '+employee.LastName as [Full Name],employee.id,AllLeave_Tbl.LeaveType,AllLeave_Tbl.PostingDate,AllLeave_Tbl.Status, ROW_NUMBER() over (order by AllLeave_Tbl.id) as [#] from AllLeave_Tbl join employee on AllLeave_Tbl.eid=employee.id where AllLeave_Tbl.Status = 2";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                ApplyLeave a = new ApplyLeave();
                a.id = int.Parse(sdr["#"].ToString());
                a.lid = int.Parse(sdr["lid"].ToString());
                a.FullName = sdr["Full Name"].ToString();
                a.LeaveType = sdr["LeaveType"].ToString();
                // a.ToDate = sdr["ToDate"].ToString();
                //a.FromDate = sdr["FromDate"].ToString();
                //a.Description = sdr["Description"].ToString();
                a.Postingdate = sdr["Postingdate"].ToString();
                a.Stat = int.Parse(sdr["Status"].ToString());

                if (a.Stat == 0)
                {
                    ViewBag.App = 0;
                }
                else if (a.Stat == 1)
                {
                    ViewBag.App = 1;
                }
                else
                {
                    ViewBag.App = 2;
                }

                alist.Add(a);
            }


            con.Close();
            return alist;
        }
        public ActionResult LeaveHistoryAdminNtApp()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            else
            {

                List<ApplyLeave> lh = getLeaveHistoryAdminNtApp();
                return View(lh);
            }

        }

        //-----------------------------------------------------------------------------
        [HttpGet]
        public ActionResult DetailsLeave(int id)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string q = "SELECT AllLeave_Tbl.id as lid,employee.FirstName+' '+employee.LastName as [Full Name],employee.EmpId,employee.id,employee.Gender,employee.Phonenumber,employee.EmailId,AllLeave_Tbl.LeaveType,AllLeave_Tbl.FromDate+' TO '+AllLeave_Tbl.ToDate as[Leave Date],AllLeave_Tbl.Description,AllLeave_Tbl.PostingDate,AllLeave_Tbl.Status,AllLeave_Tbl.AdminRemarks,AllLeave_Tbl.AdminRemarksDate from AllLeave_Tbl join employee on AllLeave_Tbl.eid=employee.id  where AllLeave_Tbl.id='" + id + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            LeaveRemarks a = new LeaveRemarks();
            a.Id = int.Parse(sdr["lid"].ToString());
            a.FullName = sdr["Full Name"].ToString();
            a.empid = sdr["empid"].ToString();
            a.Gender = sdr["Gender"].ToString();
            a.EmailId = sdr["EmailId"].ToString();
            a.PhoneNumber = sdr["PhoneNumber"].ToString();
            a.LeaveType = sdr["LeaveType"].ToString();
            a.LeaveDate = sdr["Leave Date"].ToString();
            a.PostingDate = sdr["PostingDate"].ToString();
            a.Description = sdr["Description"].ToString();
            a.AdminRemarksDate = sdr["AdminRemarksDate"].ToString();
            a.Status = int.Parse(sdr["Status"].ToString());
            if (a.Status == 0)
            {
                ViewBag.App = 0;
            }
            else if (a.Status == 1)
            {
                ViewBag.App = 1;
            }
            else
            {
                ViewBag.App = 2;
            }

            
           

            sdr.Close();
            con.Close();

            return View(a);
        }

        private List<SelectListItem> getApproval()
        {
            List<SelectListItem> AppTypeList = new List<SelectListItem>();
            AppTypeList.Add(new SelectListItem { Text = "Approved", Value = "1" });
            AppTypeList.Add(new SelectListItem { Text = "Not Approved", Value = "2" });
           
            return AppTypeList;
        }

        [HttpGet]
        public ActionResult TakeAction(int id)
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin");
            }
            else
            {
                ViewBag.ApproveType = getApproval();
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string q = "SELECT AllLeave_Tbl.id as lid,employee.FirstName + ' ' + employee.LastName as [Full Name],employee.EmpId,employee.id,employee.Gender,employee.Phonenumber,employee.EmailId,AllLeave_Tbl.LeaveType,AllLeave_Tbl.FromDate + ' TO ' + AllLeave_Tbl.ToDate as[Leave Date],AllLeave_Tbl.Description,AllLeave_Tbl.PostingDate,AllLeave_Tbl.Status,AllLeave_Tbl.AdminRemarks,AllLeave_Tbl.AdminRemarksDate from AllLeave_Tbl join employee on AllLeave_Tbl.eid = employee.id  where AllLeave_Tbl.id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                TakeAction a = new TakeAction();
                a.Id = int.Parse(sdr["lid"].ToString());

                sdr.Close();
                con.Close();

                return View(a);
            }
        }
        [HttpPost]
        public ActionResult TakeAction(TakeAction b)
        {
            int ste,red;
            if (ModelState.IsValid)
            {
                ApplyLeave a = new ApplyLeave();
                LeaveRemarks c = new LeaveRemarks();
                // TakeAction b = new TakeAction();
                
                if (b.approveoption == "0")
                {
                    ste = 0;
                    red = 0;

                }
                else if (b.approveoption == "1")
                {
                    ste = 1;
                    red = 1;

                }
                else
                {
                    ste = 2;
                    red = 2;
                }
                a.AdminRemarks = b.Description;
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand cmd = new SqlCommand("update AllLeave_Tbl set Status='" + ste + "',IsRead='" + red + "',AdminRemarks='" + a.AdminRemarks + "',AdminRemarksDate='" + DateTime.Now + "' where id ='" + b.Id + "' ", con);
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("LeaveHistoryAdmin");

            }
            return View();


        }

        public ActionResult Logout()
        {

            Session["uid"] = null;
            Session["uname"] = null;
            Session.Abandon();

            return RedirectToAction("AdminLogin");
        }
    }

    
}