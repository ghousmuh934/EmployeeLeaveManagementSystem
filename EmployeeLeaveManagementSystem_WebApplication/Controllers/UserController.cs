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

namespace EmployeeLeaveManagementSystem_WebApplication.Controllers
{
    public class UserController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        // GET: User
        private List<SelectListItem> getGend()
        {
            List<SelectListItem> GenTypeList = new List<SelectListItem>();
            GenTypeList.Add(new SelectListItem { Text = "Male", Value = "Male" });
            GenTypeList.Add(new SelectListItem { Text = "Female", Value = "Female" });
            GenTypeList.Add(new SelectListItem { Text = "Other", Value = "Other" });
            return GenTypeList;
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
        private List<SelectListItem> getLeave()
        {
            List<SelectListItem> LeaveTypList = new List<SelectListItem>();

            con.Open();
            string q;

            q = "select distinct LeaveType from LeaveType_Tbl";

            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                LeaveTypList.Add(new SelectListItem { Text = sdr[0].ToString(), Value = sdr[0].ToString() });
            }
            sdr.Close();
            con.Close();


            return LeaveTypList;
        }
        public ActionResult Empdashboard()
        {
            if (Session["uid"] == null && Session["email"] == null)
            {

                return RedirectToAction("UserLogin");
            }
            return View();
        }
        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(Login u)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand("select * from employee where emailid ='" + u.UserEmail + "' and password='" + u.Password + "' ", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                Session["uid"] = dr["id"].ToString();
                Session["email"] = dr["emailid"].ToString();
                Session["Name"] = dr["FirstName"].ToString();
                Session["Image"] = dr["ImageURl"].ToString();


                return RedirectToAction("Empdashboard");

            }
            else
            {
                dr.Close();
                ViewBag.res = "Not Record Found with this email and password try again";
            }
            return View();

        }
        [HttpGet]
        public ActionResult EmpProfile()
        {
            if (Session["uid"] == null && Session["email"] == null)
            {

                return RedirectToAction("UserLogin");
            }
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string q = "Select * from Employee where id ='" + Session["uid"] + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            Emp a = new Emp();
            ViewBag.Gendr = getGend();
            ViewBag.Deprt = getDept();
            a.ImageURL = sdr["ImageUrl"].ToString();
            a.Id = int.Parse(sdr["id"].ToString());
            a.empid = sdr["empid"].ToString();
            a.FirstName = sdr["FirstName"].ToString();
            a.LastName = sdr["LastName"].ToString();
            a.EmailId = sdr["EmailId"].ToString();
            a.Password = sdr["Password"].ToString();
            a.Gender = sdr["Gender"].ToString();
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
        public ActionResult EmpProfile(Emp e)
        {
           
              var allowedExtensions = new[] { ".bmp", ".png", ".jpg", ".gif" };

            var ext = Path.GetExtension(e.file.FileName);

            //getting  the  extension(ex-.jpg)
            if (allowedExtensions.Contains(ext))  //check  what  type of  extension
            {
                //~/Images  is  relative  path  for  images  in  root  directory
                var path = Path.Combine(Server.MapPath("~/Images"), e.file.FileName);
                e.ImageURL = "~/Images/" + e.file.FileName;
                //saving  photo  of  employee  in  the  image  folder
                //  file.SaveAs  Saves  the  contents  of  an  uploaded  file  to  a  specified path on the Web server.
                e.file.SaveAs(path);

            }
            else
            {
                ViewBag.message = "Please  choose  only  Image  file";
                return View(e);
            }
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("update employee set empid='" + e.empid + "',FirstName='" + e.FirstName + "',LastName='" + e.LastName + "',EmailId='" + e.EmailId + "',Gender='" + e.Gender + "',Dob='" + e.Dob + "',Department='" + e.Department + "',Address='" + e.Address + "',City='" + e.City + "',Country='" + e.Country + "',PhoneNumber='" + e.PhoneNumber + "',ImageUrl='" + e.ImageURL + "' where id ='" + Session["uid"] + "'", con);
            
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("EmpDashboard");
            
        }

       

        [HttpGet]
        public ActionResult AppNewLeave()
        {
            if (Session["uid"] == null && Session["email"] == null)
            {

                return RedirectToAction("UserLogin");
            }
            else
            {
                ViewBag.LType = getLeave();
                return View();
            }
        }
        [HttpPost]
        public ActionResult AppNewLeave(ApplyLeave a)
        {
            if (ModelState.IsValid)
            {
                a.Status = 0;
                a.IsRead = 0;
                

                SqlConnection con = new SqlConnection(constr);
                con.Open();
                //Response.Write("<script> alert('Connect with server!');</script>");

                string query = "insert into AllLeave_Tbl (LeaveType,ToDate,FromDate,Description,PostingDate,Status,IsRead,eid) values('" + a.LeaveType + "','" + a.ToDate + "','" + a.FromDate + "','" + a.Description + "','" + DateTime.Now + "','" + a.Status + "','" + a.IsRead + "','" + Session["uid"] + "')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script> alert('Record saved!');</script>");
                return RedirectToAction("EmpDashboard");

            }
            return View();


        }

        private List<ApplyLeave> getManageLeaveHistory()
        {


            List<ApplyLeave> alist = new List<ApplyLeave>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string q = "select LeaveType,ToDate,FromDate,Description,PostingDate,AdminRemarks,Status from AllLeave_Tbl  where eid='" + Session["uid"] + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                ApplyLeave a = new ApplyLeave();
                //a.id = int.Parse(sdr["id"].ToString());
                a.LeaveType = sdr["LeaveType"].ToString();
                a.ToDate = sdr["ToDate"].ToString();
                a.FromDate = sdr["FromDate"].ToString();
                a.Description = sdr["Description"].ToString();
                a.Postingdate = sdr["PostingDate"].ToString();
                a.AdminRemarks = sdr["AdminRemarks"].ToString();
             


                a.Status = int.Parse(sdr["Status"].ToString());
                if (a.Status == 0)
                {
                    ViewBag.App = 0;
                }
                else if(a.Status==1)
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
        public ActionResult LeaveHistory(int? page)
        {

            if (Session["uid"] == null && Session["email"] == null)
            {

                return RedirectToAction("Login");
            }
            else
            {

                List<ApplyLeave> al = getManageLeaveHistory();
                return View(al.ToPagedList(page ?? 1, 5));
            }

        }

    }
}