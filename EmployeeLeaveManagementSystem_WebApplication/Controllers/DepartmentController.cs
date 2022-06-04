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
    public class DepartmentController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        // GET: Department
        public ActionResult AddDepartment()
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin","Home");
            }
            else
            {
                

                return View();
            }
        }
        [HttpPost]
        public ActionResult AddDepartment(Department a)
        {
            if (ModelState.IsValid)
            {

                SqlConnection con = new SqlConnection(constr);
                con.Open();
                //Response.Write("<script> alert('Connect with server!');</script>");

                string query = "insert into Department(DepartmentName,DepartmentShortName,DepartmentCode,CreationDate) values ('" + a.depName + "','" + a.depShortNm + "','" + a.depCode + "','" + DateTime.Now + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script> alert('Record saved!');</script>");
                return RedirectToAction("Dashboard","Home");

            }
            return View();
        }

        private List<Department> getManageDep()
        {


            List<Department> alist = new List<Department>();
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string q = "select * from department";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                Department d = new Department();
                
                d.id = int.Parse(sdr["id"].ToString());
                d.depName = sdr["DepartmentName"].ToString();
                d.depShortNm = sdr["DepartmentShortName"].ToString();
                d.depCode = sdr["DepartmentCode"].ToString();
                d.creationDate = sdr["CreationDate"].ToString();
                alist.Add(d);
            }

            con.Close();
            return alist;
        }
        public ActionResult ManageDepartment(int? page)
        {
            if (Session["uid"] == null && Session["uname"] == null)
            {

                return RedirectToAction("AdminLogin","Home");
            }
            else
            {

                List<Department> al = getManageDep();
                return View(al.ToPagedList(page ?? 1, 3));
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete from department where id ='" + id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageDepartment");
        }

        [HttpGet]
        public ActionResult EditDepartment(int id)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string q = "Select * from department where id ='" + id + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            Department d = new Department();
            d.id = int.Parse(sdr["id"].ToString());
            d.depName = sdr["DepartmentName"].ToString();
            d.depShortNm = sdr["DepartmentShortName"].ToString();
            d.depCode = sdr["DepartmentCode"].ToString();
            d.creationDate = sdr["CreationDate"].ToString();
            sdr.Close();
            con.Close();

            return View(d);
        }
        [HttpPost]
        public ActionResult EditDepartment(Department a)
        {

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("update department set DepartmentName='" + a.depName + "',DepartmentShortName='" + a.depShortNm + "',DepartmentCode='" + a.depCode + "',CreationDate='" + DateTime.Now + "' where id ='" + a.id + "' ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("ManageDepartment");
        }


    }
}