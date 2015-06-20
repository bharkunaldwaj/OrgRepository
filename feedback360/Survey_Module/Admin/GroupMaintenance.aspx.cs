/*
* PURPOSE: GroupMaintenance Page
* AUTHOR: 
* Date Of Creation: <dd/mm/yyyy>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System; 
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Administration_BE;
using Administration_BAO;
using System.Diagnostics;
using System.Data.SqlClient;

public partial class Survey_Module_Admin_GroupMaintenance : CodeBehindBase
{
    # region Global Variables
    int iID = 0;
    string mode = null;
    string menuRights = "A,E,D,V,L1";
    //string sAccess = null;

    private Survey_Group_BE Group_BE = null;
    private Survey_Group_BAO Group_BAO = null;
    private Survey_GroupRight_BE GroupRight_BE = null;
    private Survey_GroupRight_BAO GroupRight_BAO = null;
    private List<Survey_GroupRight_BE> GroupRight_BEList = null;

    private Survey_MenuMaster_BE MenuMaster_BE = null;
    private Survey_MenuMaster_BAO MenuMaster_BAO = null;

    CodeBehindBase codeBehindBase = new CodeBehindBase();

    //DataTable dtMenu = new DataTable();
    //DataTable dtMenuRights = null;
    //DataSet dsMenu = null;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        //HandleWriteLog("Start Group Maintenance", new StackTrace(true));
        if (!IsPostBack)
        {
            //txtGroupName.Focus();

            try
            {
                //if (sPageAccessRights.Contains("A") && (Request.QueryString["View"] == null))
                //  {
                //      Page.Title = "Add Group";
                //      //chkIsActive.Checked = true;
                //      ////Creating TreeView
                //      CreateTreeView();
                //  }
                //  else
                //  {
                ////      bReturnToPrevious = true;
                //  }

                

                if (Request.QueryString["Mode"] != null)
                {
                    if (Request.QueryString["Mode"] == "E")
                    {
                        imbSave.Visible = true;
                        imbCancel.Visible = true;
                        imbBack.Visible = false;
                        lblheader.Text = "Edit Group";
                    }
                    else if (Request.QueryString["Mode"] == "R")
                    {
                        imbSave.Visible = false;
                        imbCancel.Visible = false;
                        imbBack.Visible = true;
                        lblheader.Text = "View Group";
                    }
                }

                if (Request.QueryString["GroupID"] != null)
                {
                    int groupID = Convert.ToInt32(Request.QueryString["GroupID"]);

                    GroupRight_BE = new Survey_GroupRight_BE();
                    GroupRight_BE.GroupID = groupID;

                    List<Survey_GroupRight_BE> GroupRight_BEList = null;
                    GroupRight_BAO = new Survey_GroupRight_BAO();
                    GroupRight_BEList = GroupRight_BAO.GetGroupRight(GroupRight_BE);

                    if (GroupRight_BEList != null)
                    {
                        txtGroupName.Text = GroupRight_BEList[0].FKGroup_BE.GroupName;
                        txtDescription.Text = GroupRight_BEList[0].FKGroup_BE.Description;
                        //txtWelcomeText.Text = GroupRight_BEList[0].FKGroup_BE.WelcomeText;
                        //txtNewsText.Text = GroupRight_BEList[0].FKGroup_BE.NewsText;
                        chkIsActive.Checked = GroupRight_BEList[0].FKGroup_BE.IsActive == true ? true : false;

                        //Creating TreeView
                        
                        CreateTreeView(GroupRight_BEList);
                        
                    }
                }
                else
                {
                    chkIsActive.Checked = true;
                    List<Survey_GroupRight_BE> groupRight_BEList = null;

                    //Creating TreeView
                    CreateTreeView(groupRight_BEList);
                }
                txtGroupName.Focus();
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }
            finally
            {
                //to release objects
                //if (Group_BE != null)
                //    Group_BE = null;
                //if (GroupRight_BE != null)
                //    GroupRight_BE = null;
                //if (GroupRight_BAO != null)
                //    GroupRight_BAO = null;
            }

            //if (bReturnToPrevious)
            //    Response.Redirect("AuthorizationPage.aspx");
        }
        //HandleWriteLog("End Group Maintenance", new StackTrace(true));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, EventArgs e)
    {
        //HandleWriteLog("Start", new StackTrace(true));

        //For storing the redirection status        
        bool redirect = false;
        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

        bool isGroupExist = false;

        Group_BE = new Survey_Group_BE();
        Group_BAO = new Survey_Group_BAO();

        try
        {
            if (Request.QueryString["GroupID"] == null)
            {
                Group_BE.GroupName = txtGroupName.Text.Trim() != "" ? GetString(txtGroupName.Text) : null;

                //to check whether entered group name already exist or not
                isGroupExist = Group_BAO.IsGroupExist(Group_BE);
            }

            if (!isGroupExist)
            {
                //Processing the treeview data
                # region TreeView Data


                GroupRight_BEList = (List<Survey_GroupRight_BE>)ViewState["LocalTable"];
                ResetGroupRightBEList();

                for (int k = 0; k < tvGroupRights_Feedback.Nodes.Count; k++)
                {
                    TreeNode tnMainMenu = new TreeNode();
                    tnMainMenu = tvGroupRights_Feedback.Nodes[k];

                    if (tnMainMenu.ChildNodes.Count > 0)
                    {
                        GetChildPermission(tnMainMenu);
                    }
                    else
                    {
                        if (tnMainMenu.Checked == true)
                        {
                            UpdateTable(tnMainMenu);
                        }
                    }
                }

                # endregion

                //Precessing all details
                # region Non-TreeView data

                // Add/Edit Group to database 


                //if (LookUp.CheckName(txtGroupName.Text.Trim()))
                //{
                //    Group_BE.GroupName= txtGroupName.Text.Trim();
                //}
                //else
                //{
                //    sBuilder.Append("<ul><li>'Group Name' contains one or many illegal characters. Please avoid using '!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >'</li>");
                //}

                Group_BE.GroupName = txtGroupName.Text.Trim() != "" ? GetString(txtGroupName.Text) : null;


                //if (LookUp.CheckName(txtWelcomeText.Text.Trim()))
                //{
                //    Group_BE.WelcomeText = txtWelcomeText.Text.Trim();
                //}
                //else
                //{
                //    sBuilder.Append("<li>'Welcome Text' contains one or many illegal characters. Please avoid using '!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >'</li>");
                //}
                Group_BE.Description = txtDescription.Text.Trim() != "" ? GetString(txtDescription.Text) : null;
                Group_BE.WelcomeText = ""; //txtWelcomeText.Text.Trim() != "" ? GetString(txtWelcomeText.Text) : null;
                Group_BE.NewsText = ""; // txtNewsText.Text.Trim() != "" ? GetString(txtNewsText.Text) : null;
                Group_BE.IsActive = chkIsActive.Checked == true ? true : false;

                //if (Request.QueryString["GroupID"] != null)
                //{
                //    //byte[] encodedDataAsBytes4 = System.Convert.FromBase64String(Request.QueryString["ID"].Trim());
                //    //iID = Convert.ToInt32(System.Text.Encoding.Unicode.GetString(encodedDataAsBytes4));
                //    Group_BE.GroupID = Convert.ToInt32(Request.QueryString["GroupID"].Trim());
                //}
                //else
                //{
                //    Group_BE.GroupID = 0;
                //}

                Group_BE.GroupID = Request.QueryString["GroupID"] == null ? 0 : Convert.ToInt32(Request.QueryString["GroupID"].Trim());

                if (sBuilder.ToString() == "")
                {
                    pnlMsg.Visible = false;
                    lblMessage.Visible = false;

                    if (Group_BE.GroupID == 0)
                    {
                        Group_BE.GroupID = Group_BAO.AddGroup(Group_BE);
                    }
                    else
                    {
                        Group_BAO.UpdateGroup(Group_BE);
                    }

                    foreach (Survey_GroupRight_BE GroupRight_BEItem in GroupRight_BEList)
                    {
                        GroupRight_BEItem.GroupID = Group_BE.GroupID;
                    }

                    GroupRight_BAO = new Survey_GroupRight_BAO();
                    GroupRight_BAO.AddGroupRight(GroupRight_BEList);
                }
                else
                {
                    pnlMsg.Visible = true;
                    Label lblError = new Label();
                    lblError.Text = sBuilder.ToString().Trim();
                    pnlMsg.Controls.Add(lblError);
                }
                # endregion
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Group name not available!!!";
                lblMessage.Style.Add("color", "red");
            }
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
        finally
        {
            //to release objects
            if (Group_BE != null)
                Group_BE = null;
            if (Group_BAO != null)
                Group_BAO = null;
            if (GroupRight_BE != null)
                GroupRight_BE = null;
            if (GroupRight_BAO != null)
                GroupRight_BAO = null;
        }
        //Redirecting to ViewGroups Page
        //if (bRedirect)
        //    Response.Redirect("ViewGroups.aspx");

        //For building menu as per user changes in rights
        //MasterPage masterPage = (MasterPage)Page.Master;
        //masterPage.BuildMenu();
        
        Response.Redirect("GroupMaintenanceList.aspx", false);

        //HandleWriteLog("End", new StackTrace(true));
    }

    #region Private Functions

    /// <summary>
    /// Function to Create TreeView for User
    /// </summary>
    private void CreateTreeView(List<Survey_GroupRight_BE> p_groupRight_BEList)
    {

        HandleWriteLog("Start", new StackTrace(true));

        //Gathering TreeView Data 
        //GroupMaintenanceBL Group_BAO = new GroupMaintenanceBL();
        Group_BAO = new Survey_Group_BAO();

        Group_BE = new Survey_Group_BE();
        MenuMaster_BAO = new Survey_MenuMaster_BAO();
        MenuMaster_BE = new Survey_MenuMaster_BE();

        try
        {
            List<Survey_MenuMaster_BE> menuMaster_BEList = MenuMaster_BAO.GetMenuMaster(MenuMaster_BE);

            if (menuMaster_BEList != null)
            {

                GroupRight_BEList = new List<Survey_GroupRight_BE>();
                List<Survey_MenuMaster_BE> parentMenuMaster_BEList = new List<Survey_MenuMaster_BE>();

                //var items = from item in MenuMaster_BEList
                //            where item.ParentID==0
                //            orderby item.ParentID
                //            select new { item.MenuID, item.Name, item.ADEVFlag };

                //foreach (var child1 in items)
                //{ 

                //}
                
                parentMenuMaster_BEList = GetMenuParent(menuMaster_BEList);

                foreach (Survey_MenuMaster_BE menuMaster_BEItem in parentMenuMaster_BEList)
                {
                    //Adding a New Row in the Local Table With Parent ID 0 (Root Nodes)
                    NewRow(Convert.ToInt32(menuMaster_BEItem.MenuID.ToString()), null, 0);

                    //Adding RootNodes to the TreeView
                    int rootID = Convert.ToInt32(menuMaster_BEItem.MenuID.ToString());
                    TreeNode tnRoot = new TreeNode();
                    tnRoot.Text = menuMaster_BEItem.Name.ToString();
                    tnRoot.Value = menuMaster_BEItem.MenuID.ToString();
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tnRoot.Target = "_blank";
                    tnRoot.ShowCheckBox = true;

                    //Calling Fuction to Add the Child Nodes
                    List<Survey_MenuMaster_BE> menuMaster_BEListItem = new List<Survey_MenuMaster_BE>();
                    menuMaster_BEListItem.Add(menuMaster_BEItem);
                    GetChildNodes(rootID, tnRoot, menuMaster_BEListItem, p_groupRight_BEList);

                    tvGroupRights_Feedback.Nodes.Add(tnRoot);
                    //tvGroupRights_Feedback.ExpandAll();

                    //Put tick mark on the root if all/any of the children are checked
                    
                    TickRoot(tnRoot);
                    
                }
            }
            //ViewState["LocalTable"] =  dtMenu;//Keeping the DataTable in the ViewState for future use
            ViewState["LocalTable"] = GroupRight_BEList;//Keeping the DataTable in the ViewState for future use
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
        finally
        {
            Group_BE = null;
            Group_BAO = null;
            GroupRight_BE = null;
            GroupRight_BAO = null;
        }
        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// To get the Parent Menus
    /// </summary>
    /// <param name="p_MenuMaster_BE"></param>
    /// <returns></returns>
    public List<Survey_MenuMaster_BE> GetMenuParent(List<Survey_MenuMaster_BE> p_MenuMaster_BE)
    {

        HandleWriteLog("Start", new StackTrace(true));

        List<Survey_MenuMaster_BE> MenuMaster_BEList = new List<Survey_MenuMaster_BE>();

        for (int counter = 0; counter < p_MenuMaster_BE.Count; counter++)
        {
            if (p_MenuMaster_BE[counter].ParentID == null || p_MenuMaster_BE[counter].ParentID == 0)
            {
                MenuMaster_BEList.Add(p_MenuMaster_BE[counter]);
            }
        }
        HandleWriteLog("End", new StackTrace(true));
        return MenuMaster_BEList;

    }

    /// <summary>
    /// To get the child Menus
    /// </summary>
    /// <param name="p_MenuMaster_BE"></param>
    /// <returns></returns>
    public List<Survey_MenuMaster_BE> GetMenuChild(List<Survey_MenuMaster_BE> p_MenuMaster_BE, int p_parentID)
    {

        HandleWriteLog("Start", new StackTrace(true));

        //List<MenuMaster_BE> MenuMaster_BEList = new List<MenuMaster_BE>();
        MenuMaster_BE = new Survey_MenuMaster_BE();

        List<Survey_MenuMaster_BE> menuMaster_BEList = MenuMaster_BAO.GetMenuMaster(MenuMaster_BE);
        List<Survey_MenuMaster_BE> menuMasterChild_BEList = new List<Survey_MenuMaster_BE>();
        for (int counter = 0; counter < menuMaster_BEList.Count; counter++)
        {
            if (menuMaster_BEList[counter].ParentID == p_parentID)
            {
                menuMasterChild_BEList.Add(menuMaster_BEList[counter]);
            }
        }
        HandleWriteLog("End", new StackTrace(true));
        return menuMasterChild_BEList;
    }

    /// <summary>
    /// Function To Insert a new Row into the Local DataTable
    /// </summary>
    private void NewRow(int p_ID, string p_accessRight, int p_parentID)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //DataRow drNewRow = dtMenu.NewRow();
            //drNewRow["ID"] = p_ID;
            //drNewRow["Value"] = sValue;
            //drNewRow["ParentID"] = p_parentID;
            //dtMenu.Rows.Add(drNewRow);

            GroupRight_BE = new Survey_GroupRight_BE();
            GroupRight_BE.MenuID = p_ID;
            //GroupRight_BE.AccessRights = p_accessRight;
            GroupRight_BE.AccessRights = string.Empty;

            GroupRight_BEList.Add(GroupRight_BE);

        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);

        }
        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// Function to Traverse the tree and put tick mark on the root if all/any of the chieldren are checked
    /// </summary>
    private void TickRoot(TreeNode tnRootNode)
    {
        HandleWriteLog("Start", new StackTrace(true));
        TreeNode tnTemp = tnRootNode;
        try
        {
            if (tnRootNode.ChildNodes.Count > 0)
            {
                foreach (TreeNode tn in tnRootNode.ChildNodes)
                {
                    if (tn.ChildNodes.Count > 0)
                        TickRoot(tn);
                    else
                    {
                        if (tn.Checked)//Check the parents if child is checked
                        {
                            TreeNode tnTempRoot = tnRootNode;
                            while (tnTempRoot != null)
                            {
                                tnTempRoot.Checked = true;
                                tnTempRoot = tnTempRoot.Parent;
                            }
                        }
                    }
                }
            }
            HandleWriteLog("End", new StackTrace(true));
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }

    }

    /// <summary>
    /// Function to retrive the clildnode/s and Insert them into the existing tree.
    /// i.e. Under the parent node supplied
    /// </summary>
    private void GetChildNodes(int p_rootID, TreeNode p_tnMainNode, List<Survey_MenuMaster_BE> p_menuMaster_BE, List<Survey_GroupRight_BE> p_groupRight_BEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //DataRow[] drChildMenu = null;
            //drChildMenu = dsMenu.Tables[0].Select(" ParentID=" + p_rootID);

            List<Survey_MenuMaster_BE> MenuMaster_BEList = new List<Survey_MenuMaster_BE>();
            MenuMaster_BEList = GetMenuChild(p_menuMaster_BE, p_rootID);

            foreach (Survey_MenuMaster_BE MenuMaster_BEItem in MenuMaster_BEList)
            {
                //Adding a New Row in the Local Table With Parent ID 0 (Root Nodes)
                NewRow(Convert.ToInt32(MenuMaster_BEItem.MenuID.ToString()), null, p_rootID);

                TreeNode tnChildMenu = new TreeNode();
                tnChildMenu.Text = MenuMaster_BEItem.Name.ToString();
                tnChildMenu.Value = MenuMaster_BEItem.MenuID.ToString();
                tnChildMenu.SelectAction = TreeNodeSelectAction.None;
                tnChildMenu.Target = "_blank";
                
                //BindADEV(Convert.ToInt32(MenuMaster_BEItem.MenuID.ToString()), tnChildMenu, p_groupRight_BEList);

                //tnChildMenu.Checked=MenuMaster_BEItem.ADEVFlag

                //GetChildNodes(Convert.ToInt32(MenuMaster_BEItem.MenuID), tnChildMenu, p_menuMaster_BE, p_groupRight_BEList);

                if (MenuMaster_BEItem.ADEVFlag != null && MenuMaster_BEItem.ADEVFlag.ToString() == "1")//If true add ADEV attributes bellow the node
                //if ((MenuMaster_BE.ADEVFlag.ToString()) == "True")//If true add ADEV attributes bellow the node
                {
                    BindADEV(Convert.ToInt32(MenuMaster_BEItem.MenuID.ToString()), tnChildMenu, p_groupRight_BEList);
                }
                //else
                //{    //if (dtRightList != null && dtRightList.Rows.Count > 0)
                //    if (p_groupRight_BEList != null && p_groupRight_BEList.Count > 0)
                //    {
                //    }
                //}
                //Adding the node to the main menu
                p_tnMainNode.ChildNodes.Add(tnChildMenu);
            }
            HandleWriteLog("End", new StackTrace(true));
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }

    }

    /// <summary>
    /// Adding ADEV Nodes to the Leaf Nodes supplied
    /// </summary>
    private void BindADEV(int p_rootID, TreeNode p_tnMainNode, List<Survey_GroupRight_BE> p_groupRightBEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            string Rights = "";

            if (p_groupRightBEList != null && p_groupRightBEList.Count > 0)
            {
                Rights = GetMenuAccessRights(p_rootID, p_groupRightBEList);
            }

            p_tnMainNode.Checked = Rights == "A" ? true : false;
            
            //List<FieldRight_BE> FieldRight_BEList = null;
            //FieldRight_BAO FeildRight_BAO = new FieldRight_BAO();
            //FieldRight_BEList = FeildRight_BAO.GetFieldRight();

            //for (int i = 0; i < FieldRight_BEList.Count; i++)
            //{
            //    if (FieldRight_BEList[i].MenuID == "" || FieldRight_BEList[i].MenuID.Contains("(" + p_rootID + ")"))
            //    {
            //        TreeNode tnChildMenu = new TreeNode();
            //        tnChildMenu.Text = FieldRight_BEList[i].Name.ToString();
            //        tnChildMenu.Value = FieldRight_BEList[i].Value.ToString();
            //        tnChildMenu.SelectAction = TreeNodeSelectAction.None;

            //        //if menu have some rights
            //        if (Rights != "" && Rights.Contains(tnChildMenu.Value.Trim()))
            //            tnChildMenu.Checked = true;

            //        tnChildMenu.Target = "_blank";
            //        p_tnMainNode.ChildNodes.Add(tnChildMenu);
            //    }
            //}
            HandleWriteLog("End", new StackTrace(true));
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
    }

    /// <summary>
    /// Function to get Menu Access Rights
    /// </summary>
    /// <param name="p_menuID"></param>
    /// <param name="p_groupRightBEList"></param>
    private string GetMenuAccessRights(int p_menuID, List<Survey_GroupRight_BE> p_groupRightBEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        string Rights = string.Empty;
        foreach (Survey_GroupRight_BE GroupRight_BEItem in p_groupRightBEList)
        {
            if (GroupRight_BEItem.MenuID == Convert.ToInt32(p_menuID))
            {
                Rights = GroupRight_BEItem.AccessRights;
            }
        }
        HandleWriteLog("End", new StackTrace(true));
        return (Rights);
    }

    /// <summary>
    /// Traverse the tree for New Permission list set for the group
    /// Called after Clicking the Submit Button
    /// </summary>
    private void GetChildPermission(TreeNode p_tnMainMenu)
    {
        HandleWriteLog("Start", new StackTrace(true));
        for (int l = 0; l < p_tnMainMenu.ChildNodes.Count; l++)
        {
            TreeNode tnChildMenu = p_tnMainMenu.ChildNodes[l];

            if (tnChildMenu.Checked == true)
            {
                UpdateTable(tnChildMenu);
            }
            
            //if (tnChildMenu.ChildNodes.Count > 0)
            //{
            //    GetChildPermission(tnChildMenu);
            //}
            //else
            //{
            //    if (tnChildMenu.Checked == true)
            //    {
            //        UpdateTable(tnChildMenu);
            //    }
            //}
        }

        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// Updating the Local Table with the New Permission Details
    /// Called after clicking the Submit Button
    /// </summary>
    private void UpdateTable(TreeNode p_tnChild)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            foreach (Survey_GroupRight_BE GroupRight_BEItem in GroupRight_BEList)
            {
                if (Convert.ToString(GroupRight_BEItem.MenuID).Trim().Equals(p_tnChild.Value.Trim()))
                {
                    if (GroupRight_BEItem.AccessRights != null)
                    {
                        GroupRight_BEItem.AccessRights = "A";

                        //function to set AEDV flag for the parents
                        SetAEDVToParents(Convert.ToInt32(p_tnChild.Parent.Value.Trim()));

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// Function to set the Parent's Value field to A@E@D@V for those who are checked
    /// </summary>
    /// <param name="ParentID"></param>
    private void SetAEDVToParents(int p_parentID)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //GroupRight_BEList = (List<GroupRight_BE>)ViewState["LocalTable"];

            foreach (Survey_GroupRight_BE GroupRight_BEItem in GroupRight_BEList)
            {
                if (Convert.ToString(GroupRight_BEItem.MenuID).Trim().Equals(p_parentID.ToString().Trim()))
                {
                    if(GroupRight_BEItem.AccessRights!=null && GroupRight_BEItem.AccessRights != "A")
                    GroupRight_BEItem.AccessRights = "A";
                }
            }
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// Function to reset gropurights BEList object
    /// </summary>
    private void ResetGroupRightBEList()
    {
        try
        {
            foreach (Survey_GroupRight_BE GroupRight_BE in GroupRight_BEList)
            {
                GroupRight_BE.AccessRights = string.Empty;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #endregion

    protected void imbCancel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GroupMaintenanceList.aspx", false);
    }

   
}
