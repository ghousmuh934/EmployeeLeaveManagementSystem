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
    public class LeaveTypeController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        // GET: LeaveType
        public ActionResult AddLeaveType()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddLeaveType(LeaveType a)
        {
            if (ModelState.IsValid)
            {

                SqlConnection con = new SqlConnection(constr);
                con.Open();
                //Response.Write("<script> alert('Connect with server!');</script>");

                string query = "insert into LeaveType_Tbl (LeaveType,Description,CreationDate)values('" + a.LeaveTyp + "','" + a.Description + "','" + DateTime.Now + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script> alert('Record saved!');</script>");
                return RedirectToAction("Dashboard", "Home");

            }
            return View();
        }
        private List<LeaveType> getManageLeaveType()
        {


            List<LeaveType> alist = new List<LeaveType>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string q = "select * from LeaveType_Tbl";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                LeaveType d = new LeaveType();

                d.id = int.Parse(sdr["id"].ToString());
                d.LeaveTyp = sdr["LeaveType"].ToString();
                d.Description = sdr["Description"].ToString();
             
                d.creationDate = sdr["CreationDate"].ToString();
                alist.Add(d);
            }

            con.Close();
            return alist;
        }
        public ActionResult ManageLeaveType(int? page)
        {
            if (Session["uid"] == null && Session["uname"] == null)
            { 
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                List<LeaveType> al = getManageLeaveType();
                return View(al.ToPagedList(page ?? 1, 3));
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete from LeaveType_Tbl where id ='" + id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageLeaveType");
        }
        [HttpGet]
        public ActionResult EditLeaveType(int id)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string q = "Select * from LeaveType_Tbl where id ='" + id + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            LeaveType d = new LeaveType();

            d.id = int.Parse(sdr["id"].ToString());
            d.LeaveTyp = sdr["LeaveType"].ToString();
            d.Description = sdr["Description"].ToString();
            d.creationDate = sdr["CreationDate"].ToString();
            sdr.Close();
            con.Close();

            return View(d);
        }
        [HttpPost]
        public ActionResult EditLeaveType(LeaveType a)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("update LeaveType_Tbl set LeaveType='" + a.LeaveTyp + "',Description='" + a.Description + "',CreationDate='" + DateTime.Now + "' where id ='" + a.id + "' ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageLeaveType");
        }
    }
}


