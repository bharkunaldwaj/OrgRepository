using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Configuration;

using Administration_BAO;
using Administration_BE;

public partial class AjaxCall : System.Web.UI.Page
{
    WADIdentity identity;
    User_BAO user_BAO = new User_BAO();

    string connectionString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
    string[] emoticon_Text = new string[] {
            ":)" , //smile
            ":D", //big smile
            ":d", //big smile
            ":(", //sad
            ":|", //speechless
            ":@"  //angry
      
    };
    string[] emoticon_Images = new string[] {
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/smile.gif\" style=\"height: 19px; width: 19px\" />" , //smile
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/laugh.gif\" style=\"height: 19px; width: 19px\" />", //big smile
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/laugh.gif\" style=\"height: 19px; width: 19px\" />", //big smile
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/sad.gif\" style=\"height: 19px; width: 19px\" />", //sad
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/speechless.gif\" style=\"height: 19px; width: 19px\" />", //speechless
        "   <img src=\"/WebApplicationDemo/Layouts/Resources/smileys/angry.gif\" style=\"height: 19px; width: 19px\" />"  //angry
  
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;
        if (Request.QueryString["action"] == "sendchat")
        {
            sendchat();
        }
        else if (Request.QueryString["action"] == "spy")
        {
            startspy();
        }
        else if (Request.QueryString["action"] == "spyfm")
        {
            spyfm();
        }
        else if (Request.QueryString["action"] == "setsess")
        {
            setsess();
        }
        else if (Request.QueryString["action"] == "loadhist")
        {
            loadhist();
        }
        else if (Request.QueryString["action"] == "ChatClick")
        {
           
                Session["Chat"] = "1";
                identity.User.IsOnline = true;
                identity.User.LastActionTime = System.DateTime.Now;
                user_BAO.UpdateUser(identity.User);
                loadOlUser();

        }
        else if (Request.QueryString["action"] == "update")
        {
            loadOlUser();
        }
        else if (Request.QueryString["action"] == "oluser2")
        {
            oluser2();
        }
        else if (Request.QueryString["action"] == "Logout")
        {
            LogOut();
        }
       
       
     
    }

    #region Logout
    public void LogOut()
    {
        
        if (identity.User.UserID != null )
        {
            Session["Chat"] = "0";
            string connectionString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                identity.User.IsOnline = false;
                identity.User.LastActionTime = System.DateTime.Now;
                user_BAO.UpdateUser(identity.User);

                string strQuery = "delete from chat where [UserID]=" + identity.User.UserID;
                conn.Open();
                SqlCommand cmd = new SqlCommand(strQuery, conn);


                int i = cmd.ExecuteNonQuery();
            }

        }

      
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void startspy()
    {

        //loading chat for the user
        StringBuilder newitem = new StringBuilder();
        string spyfrom = Request.QueryString["spyfrom"];
        if (identity.User.UserID != null)
        {
            string us = identity.User.UserID.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string strIdleTime = System.Configuration.ConfigurationManager.AppSettings["IdleTime"].ToString();

                SqlCommand cmd = new SqlCommand("Usp_SpyMessage", conn);
                cmd.Parameters.Add(new SqlParameter("@To", SqlDbType.VarChar)).Value = us;
                cmd.Parameters.Add(new SqlParameter("@From", SqlDbType.VarChar)).Value = spyfrom;
                cmd.Parameters.Add(new SqlParameter("@IdleTime", SqlDbType.VarChar)).Value = strIdleTime;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                try
                {
                    //int i = cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        newitem.Append(reader["message"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                conn.Close();
            }
            Response.Clear();
            string emt_spy = emoticons(newitem.ToString());
            Response.Write(emt_spy);
            Response.End();
        }
        else
        {
            Response.Clear();
            Response.Write("timeout");
            Response.End();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void sendchat()
    {
        if (identity.User.UserID != null)
        {
            string from = Convert.ToString(identity.User.UserID);
            string to = Request.QueryString["to"];
            string msg = Request.QueryString["message"].Replace("'", "''");
            string message = Regex.Replace(msg, @"<(.|\n)*?>", string.Empty);
            string sUserID = Convert.ToString(identity.User.UserID);

            DateTime now = DateTime.Now;
            string time = Request.QueryString["t"];
            string ol_ID = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                string strQuery = string.Empty;
                strQuery = string.Format("insert into chat ([from],[to],message,sent,recd,time,UserID) values ('{0}','{1}','{2}',{3},0,'{4}',{5})", from, to, message, "getdate()", time, sUserID);
                strQuery += string.Format(" select UserID from [User] where UserID='{0}'", to);
                strQuery += " update [User] set LastActionTime=getdate()  where UserID='" + sUserID + "'";

                SqlCommand cmd = new SqlCommand(strQuery, conn);
                cmd.CommandTimeout = 0;
                try
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ol_ID = Convert.ToString(reader["UserID"]);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                conn.Close();

                if ((ol_ID == ""))
                {
                    Response.Clear();
                    Response.Write("<font color=\"gray\">THIS PERSON IS OFFLINE...</font>");
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.Write("");
                    Response.End();
                }
            }
        }
        else
        {
            Response.Clear();
            Response.Write("timeout");
            Response.End();

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void spyfm()
    {
        if (identity.User.UserID != null)
        {
            StringBuilder newitem = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                string strQuery = string.Empty;
                strQuery = "select [from] from chat where [to]=" + identity.User.UserID.ToString() + " and recd=0 order by ID asc ";
                strQuery += "select UserID,FName from [User]";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                cmd.CommandTimeout = 0;
                SqlDataAdapter a = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    a.Fill(ds);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            newitem.Append(dr["from"].ToString());
                        }

                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            sb.Append(dr["UserID"] + "^" + dr["FName"] + "$");
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }


            Response.Clear();
            char[] sp = { '$' };
            string uiInfo = sb.ToString();
            uiInfo = uiInfo.TrimEnd(sp);
            string stripped = Regex.Replace(newitem.ToString(), @"<(.|\n)*?>", string.Empty);
            Response.Write(stripped + ";" + uiInfo);
            Response.End();

        }
        else 
        {
            Response.Clear();
            Response.Write("timeout");
            Response.End();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void setsess()
    {
        if (identity.User.UserID != null)
        {
            Response.Clear();
            try
            {
                Response.Write(identity.User.UserID.ToString() + "^" + Session["username"]);

            }
            catch (Exception ex1)
            { }
            Response.End();
        }
        else
        {
            Response.Clear();
            Response.Write("timeout");
            Response.End();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void loadhist()
    {
        if (identity.User.UserID != null)
        {

            StringBuilder newitem = new StringBuilder();
            string from = Request.QueryString["from"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(string.Format("select message,time,[from],[to] from chat where(  ([to]='{0}' and [from]='{1}') or ([to]='{2}' and [from]='{3}')) order by sent ASC", identity.User.UserID.ToString(), from, from, identity.User.UserID.ToString()), conn);
                cmd.CommandTimeout = 0;
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string frm = Convert.ToString(reader["from"]);
                        string to = Convert.ToString(reader["to"]);
                        string msg = Convert.ToString(reader["message"]);
                        DateTime time = Convert.ToDateTime(reader["time"]);
                        if (frm == identity.User.UserID.ToString())
                        {
                            frm = "me";
                        }
                        else
                        {

                            frm = Convert.ToString(reader["from"]);
                            frm = IDname(frm);
                        }

                        newitem.Append("<div class=\"chatboxmessage\"><span class=\"chatboxmessagefrom\">" + frm + ":&nbsp;&nbsp;</span><span class=\"chatboxmessagecontent\">" + msg + "<div class=chatboxinfo>" + time + "</div></span></div>");

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                conn.Close();
            }
            Response.Clear();
            string emt_spy = emoticons(newitem.ToString());
            Response.Write(emt_spy);
            Response.End();
        }
        else
        {
            Response.Clear();
            Response.Write("timeout");
            Response.End();
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
                                    if (ts.Days==0 && ts.Minutes < Convert.ToInt32(strIdleTime))
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

                if (Request.QueryString["action"] == "update")
                {
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.Write(sb.ToString());
                }
              
                  
                
                
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
    /// <param name="emt"></param>
    /// <returns></returns>
    public string emoticons(string emt)
    {

        for (int i = 0; i < emoticon_Text.Length; i++)
        {
            emt = emt.Replace(emoticon_Text[i], emoticon_Images[i]);
        }
        return emt;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string IDname(string s)
    {
        string frm = "";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {

            conn.Open();
            SqlCommand cmd = new SqlCommand("select FName from [User] where UserID='" + s + "'", conn);
            cmd.CommandTimeout = 0;
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    frm = (reader["FName"].ToString());


                }
                reader.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
        }
        return frm;
    }

    /// <summary>
    /// 
    /// </summary>
    public void oluser2()
    {
        StringBuilder sb = new StringBuilder();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {

            conn.Open();

            string strQuery = string.Empty;
            strQuery += "select UserID,FName from [User]";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            cmd.CommandTimeout = 0;

            SqlDataAdapter a = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                a.Fill(ds);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append(dr["UserID"] + "^" + dr["FName"] + "$");
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        Response.Clear();
        char[] sp = { '$' };
        string uiInfo = sb.ToString();
        uiInfo = uiInfo.TrimEnd(sp);
        Response.Write(uiInfo);
        Response.End();

    }

}
