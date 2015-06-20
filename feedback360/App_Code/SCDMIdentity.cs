/*  
* PURPOSE: This is the Class for SCDMIdentity
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
/// Summary description for SCDMIdentity
/// </summary>
public class SCDMIdentity : System.Security.Principal.IIdentity {
    #region Private Members

    private int userId;
    private int groupId;
    private string usercode;

    private FormsAuthenticationTicket ticket;

    #endregion

    #region Constructor

    /// <summary>
    /// Public Constructor
    /// </summary>
    /// <param name="p_ticket"></param>
    public SCDMIdentity(FormsAuthenticationTicket p_ticket) {
        try {
            this.ticket = p_ticket;
            int.TryParse(this.ticket.UserData.Substring(1, this.ticket.UserData.IndexOf("$") - 1), out userId);
            int.TryParse(this.ticket.UserData.Substring(this.ticket.UserData.IndexOf("$") + 1, (this.ticket.UserData.Length) - (this.ticket.UserData.IndexOf("$")) - 1), out groupId);
            usercode = this.ticket.Name;
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
                p_user_BE.UserCode = usercode;
                List<User_BE> user_BEList = user_BAO.GetUser(p_user_BE);

                HttpContext.Current.Session[string.Format("User-{0}", userId)] = user_BEList[0];
            }

            return HttpContext.Current.Session[string.Format("User-{0}", userId)] as User_BE;
        }
    }

    public List<GroupRight_BE> Group {
        get {
            GroupRight_BE p_groupRight_BE = null;
            GroupRight_BAO groupRight_BAO = new GroupRight_BAO();
            if (HttpContext.Current.Session[string.Format("Group-{0}", groupId)] == null) {
                p_groupRight_BE = new GroupRight_BE();
                p_groupRight_BE.GroupID = groupId;

                List<GroupRight_BE> groupRight_BEList = groupRight_BAO.GetGroupRight(p_groupRight_BE);
                HttpContext.Current.Session["Group"] = groupRight_BEList;
            }

            return HttpContext.Current.Session["Group"] as List<GroupRight_BE>;
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

