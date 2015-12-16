/*  
* PURPOSE: This is the Class for Page Parser
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for PageParser
/// </summary>
public class PageParser
{


    #region "Public Constructor"

    public PageParser()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #endregion

    # region "Parse"

    /// <summary>
    /// use to parse page.
    /// </summary>
    /// <param name="PageControl"></param>
    /// <param name="sRights"></param>
    /// <param name="ForGrid"></param>

    public static void ParsePage(Control p_pageControl, string p_rights, bool p_forGrid)
    {
        try
        {
            foreach (Control contentControl in p_pageControl.Controls)
            {
                if (contentControl is ContentPlaceHolder)
                {
                    ParseControls(contentControl, p_rights, p_forGrid);
                }
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }

    }

    /// <summary>
    /// use TO parse controls
    /// </summary>
    /// <param name="cParent"></param>
    /// <param name="sRights"></param>
    /// <param name="OnlyGrid"></param>

    private static void ParseControls(Control p_parent, string p_rights, bool p_onlyGrid)
    {
        try
        {
            foreach (Control childControl in p_parent.Controls)
            {
                //Parse and Act on Page Controls
                FindControlRecursive(childControl, p_rights, p_onlyGrid);
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
    }

    /// <summary>
    /// use to find controls
    /// </summary>
    /// <param name="Root"></param>
    /// <param name="sRights"></param>
    /// <param name="ForGrid"></param>

    public static void FindControlRecursive(Control p_root, string p_rights, bool p_forGrid)
    {
        try
        {
            foreach (Control ctl in p_root.Controls)
            {
                if (ctl != null)
                {
                    if (p_forGrid == true)
                    {
                        //FieldParser.ParseControlsGrid(ctl, p_rights);
                    }
                    else
                    {
                        FieldParser.ParseControls(ctl, p_rights);
                    }
                }
                FindControlRecursive(ctl, p_rights, p_forGrid);
            }
        }
        catch (Exception ex)
        {
            CodeBehindBase codeBehindBase = new CodeBehindBase();
            codeBehindBase.HandleException(ex);
        }
    }

    #endregion
}
