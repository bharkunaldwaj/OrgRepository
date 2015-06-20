using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using Administration_BE;
using Miscellaneous;
using System.Diagnostics;

using System.Collections.Generic;

using System.Text.RegularExpressions;



using Administration_BAO;
public partial class _Default : System.Web.UI.Page
{
    WADIdentity identity;
    User_BAO user_BAO = new User_BAO();
    CodeBehindBase codeBehindBase = new CodeBehindBase();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

    
            try
            {
                identity = this.Page.User.Identity as  WADIdentity;
                //codeBehindBase.HandleWriteLog("Start", new StackTrace(true));

                //HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath + "/Module/Chat/Chat.aspx", false);
              
                    //divMain.Style.Add("display", "block");
                    Session["Chat"] = "1";
                    identity.User.IsOnline = true;
                    identity.User.LastActionTime = System.DateTime.Now;
                    user_BAO.UpdateUser(identity.User);
                    loadOlUser();
               
              

            }
            catch (Exception ex)
            {
                //codeBehindBase.HandleException(ex);
            }
      
    }



    /// <summary>
    /// 
    /// </summary>
    public void loadOlUser()
    {
        if (identity.User.UserID != null)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;

            string connectionString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int check = 0;
                string[] Module = new string[10];
                string strIdleTime = System.Configuration.ConfigurationManager.AppSettings["IdleTime"].ToString();
                SqlDataAdapter a = new SqlDataAdapter("select * from [User] where UserID!='" + identity.User.UserID.ToString() + "' and IsOnline=1 order by Type;", conn);

                if (identity.User.Type.ToString().Trim() == "Vendor")
                    Module = new string[] { "User" };
                else if (identity.User.Type.ToString().Trim() == "User")
                    Module = new string[] { "Customer", "Vendor", "User" };
                else if (identity.User.Type.ToString().Trim() == "Customer")
                    Module = new string[] { "User" };

                DataSet ds = new DataSet();
                DataSet ds_Idle = new DataSet();
                try
                {
                    a.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int counter = 0; counter < Module.Length; counter++)
                        {

                            DataRow[] drElements = ds.Tables[0].Select("Type='" + Module[counter] + "'");

                            int length = drElements.Length;
                            if (length > 0)
                            {
                                sb.Append("<li>" + Module[counter] + "");
                                foreach (DataRow dr in drElements)
                                {
                                    TimeSpan ts = System.DateTime.Now.Subtract(Convert.ToDateTime(dr["LastActionTime"]));

                                    if (check == 0)
                                    {
                                        sb.Append("<ul style=\"list-style:disc; color:green; padding-left:35px;\">");
                                        check = 1;
                                    }

                                    string eml = dr["Email"].ToString();
                                    string clck = dr["UserID"].ToString();
                                    string Uname = dr["FName"].ToString();
                                    if (ts.Minutes < Convert.ToInt32(strIdleTime))
                                        sb.Append("<li style=\"list-style:disc; color:green;font-size:11px;text-align:left;\"><a style=\"color:Black;text-decoration:none; font-style:normal\" id=\"an" + i + "\"href=\"javascript:void(0);\" title='" + eml
                                     + "' onclick=\"startchat('" + clck + "')\">" + Uname + "</a></li>");
                                    else
                                        sb.Append("<li style=\"list-style:disc; color:#f99d39;font-size:11px;text-align:left;\"><a style=\"color:Black;text-decoration:none; font-style:italic\" id=\"an" + i + "\"href=\"javascript:void(0);\" title='" + eml
                                  + "' onclick=\"startchat('" + clck + "')\">" + Uname + "</a></li>");
                                }
                                if (check == 1)
                                {
                                    sb.Append("</ul></li></ br>");
                                    check = 0;
                                }
                            }
                        }

                    }
                  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                conn.Close();

                Response.Clear();
                Response.Write(sb.ToString());
                Response.End();
                 
               

            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
 
}




