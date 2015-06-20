/*  
* PURPOSE: This is the Class for WADIdentity
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Web.Security;
using System.Web;
using System.Collections.Generic;

using Administration_BE;
using Administration_BAO;


/// <summary>
/// Summary description for WADIdentity
/// </summary>
public class WADIdentity : System.Security.Principal.IIdentity {
    #region Private Members

    private int userId;
    private int groupId;
    private string usercode;

    private string LoginID;
    private string Password;
    private string AccountCode;

    private FormsAuthenticationTicket ticket;

    #endregion

    #region Constructor

    /// <summary>
    /// Public Constructor
    /// </summary>
    /// <param name="p_ticket"></param>
    public WADIdentity(FormsAuthenticationTicket p_ticket) {
        try {
            this.ticket = p_ticket; 
            string[] userData = ticket.UserData.ToString().Split('$');
            //int.TryParse(this.ticket.UserData.Substring(1, this.ticket.UserData.IndexOf("$") - 1), out userId);
            //int.TryParse(this.ticket.UserData.Substring(this.ticket.UserData.IndexOf("$") + 1, (this.ticket.UserData.Length) - (this.ticket.UserData.IndexOf("$")) - 1), out groupId);
            
            LoginID = userData[0];
            Password = userData[1];
            AccountCode = userData[2];
            groupId =Convert.ToInt32(userData[3]);

            usercode = this.ticket.Name;


        //    HttpContext.Current.Session[string.Format("Group-{0}", groupId)] = null;

        }
        catch (Exception ex) {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
    }

    #endregion

    #region Public Properties

    public User_BE User {
        get {
            User_BE p_user_BE = null;
            User_BAO user_BAO = new User_BAO();

            if (HttpContext.Current.Session[string.Format("User-{0}", userId)] == null) {
                p_user_BE = new User_BE();
                //p_user_BE.UserCode = usercode;
                p_user_BE.LoginID = LoginID;
                p_user_BE.Password = Password;
                p_user_BE.AccountCode = AccountCode;

                List<User_BE> user_BEList = user_BAO.GetUser(p_user_BE);

                HttpContext.Current.Session[string.Format("User-{0}", LoginID)] = user_BEList[0];

                //HttpContext.Current.Session[string.Format("User-{0}", userId)] = user_BEList[0];
            }

            return HttpContext.Current.Session[string.Format("User-{0}", LoginID)] as User_BE;
        }
    }

    public List<GroupRight_BE> Group {
        get {
            GroupRight_BE p_groupRight_BE = null;
            GroupRight_BAO groupRight_BAO = new GroupRight_BAO();
            //if (HttpContext.Current.Session[string.Format("Group-{0}", groupId)] == null) {}
                p_groupRight_BE = new GroupRight_BE();
                p_groupRight_BE.GroupID = groupId;
         
                List<GroupRight_BE> groupRight_BEList = groupRight_BAO.GetGroupRightFeedback(p_groupRight_BE);
                HttpContext.Current.Session[string.Format("Group-{0}", groupId)] = groupRight_BEList;
                return HttpContext.Current.Session[string.Format("Group-{0}", groupId)] as List<GroupRight_BE>;
           
        }
    }



    public List<Survey_GroupRight_BE> Survey_Group
    {
        get
        {
            Survey_GroupRight_BE p_groupRight_BE = null;
            Survey_GroupRight_BAO groupRight_BAO = new Survey_GroupRight_BAO();
           // if (HttpContext.Current.Session[string.Format("Group-{0}", groupId)] == null)
           // {
                p_groupRight_BE = new Survey_GroupRight_BE();
                p_groupRight_BE.GroupID = groupId;

                List<Survey_GroupRight_BE> groupRight_BEList = groupRight_BAO.GetGroupRight(p_groupRight_BE);
                HttpContext.Current.Session[string.Format("Group-{0}", groupId)] = groupRight_BEList;
           // }

            return HttpContext.Current.Session[string.Format("Group-{0}", groupId)] as List<Survey_GroupRight_BE>;
        }
    }





    public string AuthenticationType {
        get { return "Custom"; }
    }

    public bool IsAuthenticated {
        get { return true; }
    }

    public string Name {
        get { return this.ticket.Name; }
    }

    public FormsAuthenticationTicket Ticket {
        get { return this.ticket; }
    }

    #endregion
}

