/*  
* PURPOSE: This is the Class for individually checking the access rights and accordindly disabling the controls
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FieldParser
/// </summary>
public class FieldParser
{
    CodeBehindBase codeBehindBase = new CodeBehindBase();

    #region "Public Constructor"

    public FieldParser()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #endregion


    # region "Parse Controls"

    /// <summary>
    /// Parsing individual controls and accordingly disabling the controls if the user does not have the rights
    /// </summary>
    /// <param name="Cc"></param>
    /// <param name="sRights"></param>

    public static void ParseControls(Control p_ctrl, string p_rights)
    {
        try
        {
            switch (p_ctrl.ToString())
            {

                case "System.Web.UI.WebControls.Button":
                    if (((Button)p_ctrl).Attributes["Permission"] != null)
                        if (!CheckPermission(((Button)p_ctrl).Attributes["Permission"].ToString(), p_rights))
                        {
                            ((Button)p_ctrl).Enabled = false;
                        }

                    break;

                case "System.Web.UI.WebControls.TextBox":
                    if (((TextBox)p_ctrl).Attributes["Permission"] != null)
                        if (!CheckPermission(((TextBox)p_ctrl).Attributes["Permission"].ToString(), p_rights))
                        {
                            ((TextBox)p_ctrl).Enabled = false;
                        }
                    break;

                case "System.Web.UI.WebControls.GridView":

                    GridView objectGrid = (GridView)p_ctrl;
                    foreach (GridViewRow Row in objectGrid.Rows)
                    {

                        foreach (Control pr in Row.Controls)
                            foreach (Control childControlgrd in pr.Controls)
                            {
                                switch (childControlgrd.ToString())
                                {
                                    case "System.Web.UI.WebControls.LinkButton":
                                        if (((LinkButton)childControlgrd).Attributes["Permission"] != null)
                                        {
                                            if (!CheckPermission(((LinkButton)childControlgrd).Attributes["Permission"].ToString(), p_rights))
                                            {
                                                ((LinkButton)childControlgrd).OnClientClick = "";
                                                ((LinkButton)childControlgrd).Enabled = false;
                                            }
                                        }
                                        break;
                                }
                            }
                    }

                    break;
                //System.Web.UI.HtmlControls.HtmlGenericControl

                case "System.Web.UI.HtmlControls.HtmlGenericControl":

                    //set Permission
                    if (((HtmlControl)p_ctrl).Attributes["Permission"] != null)
                        if (!CheckPermission(((HtmlControl)p_ctrl).Attributes["Permission"].ToString(), p_rights))
                        {
                            ((HtmlControl)p_ctrl).Disabled = true;
                        }
                    //Parse child controls
                    if (p_ctrl.Controls.Count > 0)
                        ParseHierarchy(p_ctrl, p_rights);
                    break;
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
    }

    /// <summary>
    /// Parsing Telerik Grid controls and accordingly disabling the controls if the user does not have the rights
    /// </summary>
    /// <param name="Cc"></param>
    /// <param name="sRights"></param>

    //public static void ParseControlsGrid(Control p_ctrl, string p_rights) {
    //    try {
    //        switch (p_ctrl.ToString()) {
    //            case "Telerik.Web.UI.RadGrid":

    //                RadGrid objRadGrid = (RadGrid)p_ctrl;
    //                foreach (GridDataItem item in objRadGrid.Items) {
    //                    foreach (Control pr in item.Controls)
    //                        foreach (Control childControlgrid in pr.Controls) {
    //                            switch (childControlgrid.ToString()) {
    //                                case "System.Web.UI.WebControls.ImageButton":
    //                                    if (((ImageButton)childControlgrid).Attributes["Permission"] != null) {
    //                                        if (!CheckPermission(((ImageButton)childControlgrid).Attributes["Permission"].ToString(), p_rights)) {
    //                                            ((ImageButton)childControlgrid).OnClientClick = "";
    //                                            ((ImageButton)childControlgrid).Enabled = false;
    //                                        }
    //                                    }
    //                                    break;

    //                                case "System.Web.UI.WebControls.LinkButton":
    //                                    if (((LinkButton)childControlgrid).Attributes["Permission"] != null) {
    //                                        if (!CheckPermission(((LinkButton)childControlgrid).Attributes["Permission"].ToString(), p_rights)) {
    //                                            ((LinkButton)childControlgrid).OnClientClick = "";
    //                                            ((LinkButton)childControlgrid).Enabled = false;
    //                                        }
    //                                    }
    //                                    break;
    //                            }
    //                        }
    //                }
    //                break;
    //        }
    //    }
    //    catch (Exception ex) {
    //        CodeBehindBase codeBehindBase = new CodeBehindBase();
    //        codeBehindBase.HandleException(ex);
    //    }
    //}

    /// <summary>
    /// Looping through the controls
    /// </summary>
    /// <param name="cParent"></param>
    /// <param name="sRights"></param>

    private static void ParseHierarchy(Control p_parent, string p_rights)
    {
        try
        {
            foreach (Control childControl in p_parent.Controls)
            {
                //Parse and Act on Page Controls
                ParseControls(childControl, p_rights);
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
    }


    ///<Author>SubrataM</Author>
    ///<Date>6th June 2009</Date>
    /// <summary>
    /// Function to Check the Control Rights match to the Supplied Rights for the user or not
    /// </summary>
    /// <param name="sControlRights"></param>
    /// <param name="sRightList"></param>
    /// <returns></returns>
    private static bool CheckPermission(string p_controlRights, string p_rightList)
    {
        bool returnFlag = true;
        try
        {

            if (p_controlRights.Contains(","))
            {
                string[] sRightArray = p_controlRights.Split(',');

                foreach (string str in sRightArray)
                {
                    if (!p_rightList.Contains(str))
                    {
                        returnFlag = false;
                        break;
                    }
                }
            }
            else
            {
                if (!p_rightList.Contains(p_controlRights))
                    returnFlag = false;
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
        return returnFlag;
    }

    #endregion
}

