/*  
* PURPOSE: This is the Class for SCDMPrincipal
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Security.Principal;
//using SCDM.BO.AccessManagement;
using System.Linq;

/// <summary>
/// Summary description for SCDMPrincipal
/// </summary>
public class SCDMPrincipal : IPrincipal
{
    #region Private Member Variables

    private SCDMIdentity identity;

    #endregion

    #region Constructors
    public SCDMPrincipal(SCDMIdentity p_identity) {
        this.identity = p_identity;        
    }
    #endregion

    #region Public Properties

    public System.Security.Principal.IIdentity Identity {
        get { return this.identity; }
    }

    public bool IsInRole(string role) { 
        return false; 
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Checks the permission by passing functionname
    /// </summary>
    /// <param name="functionName"></param>
    /// <returns></returns>   
    public string CanPerform(string p_functionName) {

        string returnVal = "";
        try
        {
            var items = (from item in identity.Group
                         where item.FKMenuMaster_BE.Page.ToUpper().Contains(p_functionName)
                         select new { item.AccessRights });

            foreach (var right in items)
            {
                if (right.AccessRights.Trim() == "")
                {
                    returnVal = "";
                }
                else
                    returnVal = right.AccessRights.ToString();
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
        return returnVal;
    }
    #endregion
}

