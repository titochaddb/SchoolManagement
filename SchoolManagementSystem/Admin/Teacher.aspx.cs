using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class Teacher : System.Web.UI.Page
    {

        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                GetTeachers();
            }
        }

        private void GetTeachers()
        {
            DataTable dt = fn.Fetch("select row_number() over(order by(select 1)) as [Sr.No], TeacherId, [Name], DoB, Gender, Mobile, Email, [Address], [PassWord] from Teacher");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(ddlGender.SelectedValue != "0")
                {
                    string email = txtEmail.Text.Trim();
                    DataTable dt = fn.Fetch("Select * from Teacher where Email = '" + email + "' ");
                    if(dt.Rows.Count == 0)
                    {
                        string query = "Insert into Teacher values ('" + txtName.Text.Trim() + "' , '" + txtDoB.Text.Trim() + "', '" + ddlGender.SelectedValue + "' , '" + txtMobile.Text.Trim() + "' , '" + txtEmail.Text.Trim() + "', '" + txtAddress.Text.Trim() + "', '" + txtPassWord.Text.Trim() + "')";
                        fn.Query(query);
                        lblMsg.Text = "Insert Succes";
                        lblMsg.CssClass = "alert alert-success";
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtDoB.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtPassWord.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        GetTeachers();
                        
                    }
                    else
                    {
                        lblMsg.Text = "Enter <b>'" + email + "' </b> already exist";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "Gender id required";
                    lblMsg.CssClass = "alert alert-danger";
                }
                
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetTeachers();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetTeachers();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetTeachers();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int teacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);              
                string name = (row.FindControl("txtName") as TextBox).Text;
                string mobile = (row.FindControl("txtMobile") as TextBox).Text;
                string password = (row.FindControl("txtPassword") as TextBox).Text;
                string address = (row.FindControl("txtAddress") as TextBox).Text;
               
                fn.Query("update Teacher set Name = '" + name.Trim() + "', Mobile = '" + mobile.Trim() + "', Password = '" +password.Trim() + "', Address = '" + address.Trim() + "' where TeacherId = '" + teacherId + "'");
                lblMsg.Text = "Teacher update success";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeachers();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int teacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("delete from Teacher where TeacherId ='" + teacherId + "' ");
                lblMsg.Text = "Teacher delete success";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeachers();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}