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
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Administration_BE;
using Administration_BAO;
using System.Diagnostics;

public partial class Module_Admin_GroupMaintenance : CodeBehindBase
{
    # region Global Variables
    int iID = 0;
    string mode = null;
    string menuRights = "A,E,D,V,L1";
    //string sAccess = null;

    private Group_BE GroupBusinessEntity = null;
    private Group_BAO GroupBusinessAccessObject = null;
    private GroupRight_BE GroupRightBusinessEntity = null;

    //   private GroupRight_BE GroupRight_BE_check = null;
    private GroupRight_BAO GroupRightBusinessAccessObject = null;
    private List<GroupRight_BE> GroupRightBusinessEntityList = null;
    private List<GroupRight_BE> GroupRightBusinessEntityListCheck = null;

    private MenuMaster_BE MenuMasterBusinessEntity = null;
    private MenuMaster_BAO MenuMasterBusinessAccessObject = null;

    string[] str = null;
    string[] str1 = null;
    string[] str2 = null;

    CodeBehindBase codeBehindBase = new CodeBehindBase();

    //DataTable dtMenu = new DataTable();
    //DataTable dtMenuRights = null;
    //DataSet dsMenu = null;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //  Label ll = (Label)this.Master.FindControl("Current_location");
        // ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
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


                //If querystrig  "Mode" contains "E" then it is in Edit mode and 
                //if "R" then Read mode and hide show controls accordingly.
                if (Request.QueryString["Mode"] != null)
                {
                    if (Request.QueryString["Mode"] == "E")//Edit mode
                    {
                        imbSave.Visible = true;
                        imbCancel.Visible = true;
                        imbBack.Visible = false;
                        lblheader.Text = "Edit Group";
                    }
                    else if (Request.QueryString["Mode"] == "R")//Read Mode.
                    {
                        imbSave.Visible = false;
                        imbCancel.Visible = false;
                        imbBack.Visible = true;
                        lblheader.Text = "View Group";
                    }
                }
                //Ig group id is not null
                if (Request.QueryString["GroupID"] != null)
                {
                    int groupID = Convert.ToInt32(Request.QueryString["GroupID"]);

                    GroupRightBusinessEntity = new GroupRight_BE();
                    GroupRightBusinessEntity.GroupID = groupID;


                    List<GroupRight_BE> GroupRightBusinessEntityList = null;
                    GroupRightBusinessAccessObject = new GroupRight_BAO();
                    //Get group rights
                    GroupRightBusinessEntityList = GroupRightBusinessAccessObject.GetGroupRight(GroupRightBusinessEntity);

                    if (GroupRightBusinessEntityList != null)
                    {
                        txtGroupName.Text = GroupRightBusinessEntityList[0].FKGroup_BE.GroupName;
                        txtDescription.Text = GroupRightBusinessEntityList[0].FKGroup_BE.Description;
                        //txtWelcomeText.Text = GroupRight_BEList[0].FKGroup_BE.WelcomeText;
                        //txtNewsText.Text = GroupRight_BEList[0].FKGroup_BE.NewsText;
                        chkIsActive.Checked = GroupRightBusinessEntityList[0].FKGroup_BE.IsActive == true ? true : false;

                        //Creating TreeView

                        CreateTreeView(GroupRightBusinessEntityList, "F");//create Tree for Feedback 360.
                        //   GroupRight_BEList = null;
                        CreateTreeView(GroupRightBusinessEntityList, "S");//create Tree for Survey.
                        CreateTreeView(GroupRightBusinessEntityList, "P");//create Tree for Personality.
                    }
                }
                else
                {
                    chkIsActive.Checked = true;
                    List<GroupRight_BE> groupRightBusinessEntityList = null;

                    //Calling the createtreeview for Feedback
                    CreateTreeView(groupRightBusinessEntityList, "F");
                    //Calling the create tree view for Survey
                    groupRightBusinessEntityList = null;
                    CreateTreeView(groupRightBusinessEntityList, "S");
                    CreateTreeView(GroupRightBusinessEntityList, "P");//create Tree for Personality.
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
    /// Save group details.
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

        GroupBusinessEntity = new Group_BE();
        GroupBusinessAccessObject = new Group_BAO();

        try
        {
            if (Request.QueryString["GroupID"] == null)
            {
                GroupBusinessEntity.GroupName = txtGroupName.Text.Trim() != "" ? GetString(txtGroupName.Text) : null;

                //to check whether entered group name already exist or not
                isGroupExist = GroupBusinessAccessObject.IsGroupExist(GroupBusinessEntity);
            }

            if (!isGroupExist)
            {
                //Processing the treeview data
                # region TreeView Data



                //DataTable dt1= (DataTable)ViewState["LocalTable"];
                //DataTable dt2= (DataTable)ViewState["Survey_LocalTable"];
                //dt2.Merge(dt1);              
                //GroupRight_BEList= (List<GroupRight_BE>)dt2;


                //string[] a3=null;
                //string[] str1=null;
                //string[] str2 = null;
                // if(ViewState["LocalTable"]!=null)


                // GroupRight_BEList = (List<GroupRight_BE>)ViewState["Get_ParentId"];

                DataTable get_pID;

                GroupRight_BAO GR_bao = new GroupRight_BAO();
                get_pID = GR_bao.get_parentid();

                //Get all group rights selected
                GroupRightBusinessEntityList = (List<GroupRight_BE>)ViewState["LocalTable"];
                for (int i = 0; i < GroupRightBusinessEntityList.Count; i++)
                {
                    int rowcount = get_pID.Rows.Count;

                    for (int gg = 0; gg < rowcount; gg++)
                    {
                        if (GroupRightBusinessEntityList[i].MenuID == Convert.ToInt32(get_pID.Rows[gg][0]))
                        {
                            GroupRightBusinessEntityList.Remove(GroupRightBusinessEntityList[i]);
                            break;
                        }

                    }

                    //     if (GroupRight_BEList[i].MenuID == 1 || GroupRight_BEList[i].MenuID == 2 || GroupRight_BEList[i].MenuID == 3 || GroupRight_BEList[i].MenuID == 4)

                }

                if (ViewState["Survey_LocalTable"] != null)
                    GroupRightBusinessEntityList.AddRange((List<GroupRight_BE>)ViewState["Survey_LocalTable"]);


                if (ViewState["Personality_LocalTable"] != null)
                    GroupRightBusinessEntityList.AddRange((List<GroupRight_BE>)ViewState["Personality_LocalTable"]);

                // if(ViewState["Survey_LocalTable"]!=null)
                // str2 = (String[])ViewState["Survey_LocalTable"];
                // str1.CopyTo(a3,0);
                // str2.CopyTo(a3,str1.Length);
                //ViewState["sur_feedbk"]=a3;
                //  GroupRight_BEList = (List<GroupRight_BE>)ViewState["sur_feedbk"];

                //GroupRight_BEList= List<GroupRight_BE>)ViewState["LocalTable"] + (List<GroupRight_BE>)ViewState["Survey_LocalTable"]

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
                            UpdateTable(tnMainMenu);//Update database
                        }
                    }
                }

                for (int k = 0; k < tvGroupRights_Survey.Nodes.Count; k++)
                {
                    TreeNode tnMainMenu = new TreeNode();
                    tnMainMenu = tvGroupRights_Survey.Nodes[k];

                    if (tnMainMenu.ChildNodes.Count > 0)
                    {
                        GetChildPermission(tnMainMenu);
                    }
                    else
                    {
                        if (tnMainMenu.Checked == true)
                        {
                            UpdateTable(tnMainMenu);//Update database
                        }
                    }
                }

                for (int k = 0; k < tvGroupRights_Personality.Nodes.Count; k++)
                {
                    TreeNode tnMainMenu = new TreeNode();
                    tnMainMenu = tvGroupRights_Personality.Nodes[k];

                    if (tnMainMenu.ChildNodes.Count > 0)
                    {
                        GetChildPermission(tnMainMenu);
                    }
                    else
                    {
                        if (tnMainMenu.Checked == true)
                        {
                            UpdateTable(tnMainMenu);//Update database
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

                GroupBusinessEntity.GroupName = txtGroupName.Text.Trim() != "" ? GetString(txtGroupName.Text) : null;


                //if (LookUp.CheckName(txtWelcomeText.Text.Trim()))
                //{
                //    Group_BE.WelcomeText = txtWelcomeText.Text.Trim();
                //}
                //else
                //{
                //    sBuilder.Append("<li>'Welcome Text' contains one or many illegal characters. Please avoid using '!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >'</li>");
                //}
                GroupBusinessEntity.Description = txtDescription.Text.Trim() != "" ? GetString(txtDescription.Text) : null;
                GroupBusinessEntity.WelcomeText = ""; //txtWelcomeText.Text.Trim() != "" ? GetString(txtWelcomeText.Text) : null;
                GroupBusinessEntity.NewsText = ""; // txtNewsText.Text.Trim() != "" ? GetString(txtNewsText.Text) : null;
                GroupBusinessEntity.IsActive = chkIsActive.Checked == true ? true : false;

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

                GroupBusinessEntity.GroupID = Request.QueryString["GroupID"] == null ? 0 : Convert.ToInt32(Request.QueryString["GroupID"].Trim());

                if (sBuilder.ToString() == "")
                {
                    pnlMsg.Visible = false;
                    lblMessage.Visible = false;

                    if (GroupBusinessEntity.GroupID == 0)
                    {
                        GroupBusinessEntity.GroupID = GroupBusinessAccessObject.AddGroup(GroupBusinessEntity);
                    }
                    else
                    {
                        GroupBusinessAccessObject.UpdateGroup(GroupBusinessEntity);
                    }

                    foreach (GroupRight_BE GroupRight_BEItem in GroupRightBusinessEntityList)
                    {
                        GroupRight_BEItem.GroupID = GroupBusinessEntity.GroupID;
                    }

                    GroupRightBusinessAccessObject = new GroupRight_BAO();

                    //string xx,xx1;
                    //      int i = 0;
                    //GroupRight_BEList_Check = GroupRight_BEList;

                    //foreach (GroupRight_BE bb in GroupRight_BEList)
                    //{
                    //    int ss = GroupRight_BEList.Count;
                    //    for (int d = 0; d < ss; d++)
                    //    {
                    //        if (bb == GroupRight_BEList_Check[i])
                    //        {
                    //            GroupRight_BEList_Check[i] = null;
                    //        }

                    //    }

                    //}
                    //GroupRight_BEList=GroupRight_BEList.Distinct().ToList();


                    GroupRightBusinessAccessObject.AddGroupRight(GroupRightBusinessEntityList);
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
            if (GroupBusinessEntity != null)
                GroupBusinessEntity = null;
            if (GroupBusinessAccessObject != null)
                GroupBusinessAccessObject = null;
            if (GroupRightBusinessEntity != null)
                GroupRightBusinessEntity = null;
            if (GroupRightBusinessAccessObject != null)
                GroupRightBusinessAccessObject = null;
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
    private void CreateTreeView(List<GroupRight_BE> groupRightBusinessEntityList, string projectType)
    {
        HandleWriteLog("Start", new StackTrace(true));

        //Gathering TreeView Data 
        //GroupMaintenanceBL Group_BAO = new GroupMaintenanceBL();
        GroupBusinessAccessObject = new Group_BAO();

        GroupBusinessEntity = new Group_BE();
        MenuMasterBusinessAccessObject = new MenuMaster_BAO();
        MenuMasterBusinessEntity = new MenuMaster_BE();

        try
        {
            MenuMasterBusinessEntity.ADEVFlag = projectType;
            List<MenuMaster_BE> menuMasterBusinessEntityList = MenuMasterBusinessAccessObject.GetMenuMaster(MenuMasterBusinessEntity);
            //  ViewState["Get_ParentId"] = menuMaster_BEList;
            if (menuMasterBusinessEntityList != null)
            {

                GroupRightBusinessEntityList = new List<GroupRight_BE>();
                List<MenuMaster_BE> parentMenuMasterBusinessEntityList = new List<MenuMaster_BE>();

                //var items = from item in MenuMaster_BEList
                //            where item.ParentID==0
                //            orderby item.ParentID
                //            select new { item.MenuID, item.Name, item.ADEVFlag };

                //foreach (var child1 in items)
                //{ 

                //}

                parentMenuMasterBusinessEntityList = GetMenuParent(menuMasterBusinessEntityList);

                foreach (MenuMaster_BE menuMasterBusinessEntityItem in parentMenuMasterBusinessEntityList)
                {
                    //Adding a New Row in the Local Table With Parent ID 0 (Root Nodes)
                    NewRow(Convert.ToInt32(menuMasterBusinessEntityItem.MenuID.ToString()), null, 0);
                    menuMasterBusinessEntityItem.ADEVFlag = projectType;
                    //Adding RootNodes to the TreeView
                    int rootID = Convert.ToInt32(menuMasterBusinessEntityItem.MenuID.ToString());
                    TreeNode tnRoot = new TreeNode();
                    tnRoot.Text = menuMasterBusinessEntityItem.Name.ToString();
                    tnRoot.Value = menuMasterBusinessEntityItem.MenuID.ToString();
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tnRoot.Target = "_blank";
                    tnRoot.ShowCheckBox = true;

                    //Calling Fuction to Add the Child Nodes
                    List<MenuMaster_BE> menuMasterBusinessEntityListItem = new List<MenuMaster_BE>();
                    menuMasterBusinessEntityListItem.Add(menuMasterBusinessEntityItem);
                    GetChildNodes(rootID, tnRoot, menuMasterBusinessEntityListItem, groupRightBusinessEntityList);
                    if (projectType == "F")
                        tvGroupRights_Feedback.Nodes.Add(tnRoot);
                    else if (projectType == "S")
                        tvGroupRights_Survey.Nodes.Add(tnRoot);
                    else if (projectType == "P")
                        tvGroupRights_Personality.Nodes.Add(tnRoot);
                    //tvGroupRights_Feedback.ExpandAll();

                    //Put tick mark on the root if all/any of the children are checked

                    TickRoot(tnRoot);
                }
            }
            //   ViewState["LocalTable"] =  dtMenu;//Keeping the DataTable in the ViewState for future use
            if (projectType == "F")
                ViewState["LocalTable"] = GroupRightBusinessEntityList;
            else if (projectType == "S")
            {
                ViewState["Survey_LocalTable"] = GroupRightBusinessEntityList;
            }
            else if (projectType == "P")
            {
                ViewState["Personality_LocalTable"] = GroupRightBusinessEntityList;
            }
        }
        catch (Exception ex)
        {
            //function to write error log entry in the database
            HandleException(ex);
        }
        finally
        {
            GroupBusinessEntity = null;
            GroupBusinessAccessObject = null;
            GroupRightBusinessEntity = null;
            GroupRightBusinessAccessObject = null;
        }
        HandleWriteLog("End", new StackTrace(true));
    }

    /// <summary>
    /// To get the Parent Menus
    /// </summary>
    /// <param name="listMenuMaster"></param>
    /// <returns></returns>
    public List<MenuMaster_BE> GetMenuParent(List<MenuMaster_BE> listMenuMaster)
    {
        HandleWriteLog("Start", new StackTrace(true));

        List<MenuMaster_BE> MenuMasterBusinessEntityList = new List<MenuMaster_BE>();

        for (int counter = 0; counter < listMenuMaster.Count; counter++)
        {
            if (listMenuMaster[counter].ParentID == null || listMenuMaster[counter].ParentID == 0)
            {
                MenuMasterBusinessEntityList.Add(listMenuMaster[counter]);
            }
        }

        HandleWriteLog("End", new StackTrace(true));
        return MenuMasterBusinessEntityList;
    }

    /// <summary>
    /// To get the child Menus
    /// </summary>
    /// <param name="ListMenuMasterBusinessEntity"></param>
    /// <returns></returns>
    public List<MenuMaster_BE> GetMenuChild(List<MenuMaster_BE> ListMenuMasterBusinessEntity, int parentID)
    {

        HandleWriteLog("Start", new StackTrace(true));

        //List<MenuMaster_BE> MenuMaster_BEList = new List<MenuMaster_BE>();
        MenuMasterBusinessEntity = new MenuMaster_BE();
        MenuMasterBusinessEntity.ADEVFlag = ListMenuMasterBusinessEntity[0].ADEVFlag;
        //Get menu details
        List<MenuMaster_BE> menuMasterBusinessEntityList = MenuMasterBusinessAccessObject.GetMenuMaster(MenuMasterBusinessEntity);
        List<MenuMaster_BE> menuMasterChildBusinessEntityList = new List<MenuMaster_BE>();

        for (int counter = 0; counter < menuMasterBusinessEntityList.Count; counter++)
        {
            if (menuMasterBusinessEntityList[counter].ParentID == parentID)
            {
                menuMasterChildBusinessEntityList.Add(menuMasterBusinessEntityList[counter]);
            }
        }

        HandleWriteLog("End", new StackTrace(true));

        return menuMasterChildBusinessEntityList;
    }

    /// <summary>
    /// Function To Insert a new Row into the Local DataTable
    /// </summary>
    private void NewRow(int menuID, string accessRight, int parentID)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //DataRow drNewRow = dtMenu.NewRow();
            //drNewRow["ID"] = p_ID;
            //drNewRow["Value"] = sValue;
            //drNewRow["ParentID"] = p_parentID;
            //dtMenu.Rows.Add(drNewRow);

            GroupRightBusinessEntity = new GroupRight_BE();
            GroupRightBusinessEntity.MenuID = menuID;
            //GroupRight_BE.AccessRights = p_accessRight;
            GroupRightBusinessEntity.AccessRights = string.Empty;

            GroupRightBusinessEntityList.Add(GroupRightBusinessEntity);

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
    private void GetChildNodes(int p_rootID, TreeNode treeMainNode, List<MenuMaster_BE> listMenuMaster,
        List<GroupRight_BE> groupRight_BEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //DataRow[] drChildMenu = null;
            //drChildMenu = dsMenu.Tables[0].Select(" ParentID=" + p_rootID);

            List<MenuMaster_BE> MenuMasterBusinessEntityList = new List<MenuMaster_BE>();
            MenuMasterBusinessEntityList = GetMenuChild(listMenuMaster, p_rootID);

            foreach (MenuMaster_BE MenuMasterBusinessEntityItem in MenuMasterBusinessEntityList)
            {
                //Adding a New Row in the Local Table With Parent ID 0 (Root Nodes)
                NewRow(Convert.ToInt32(MenuMasterBusinessEntityItem.MenuID.ToString()), null, p_rootID);

                TreeNode tnChildMenu = new TreeNode();
                tnChildMenu.Text = MenuMasterBusinessEntityItem.Name.ToString();
                tnChildMenu.Value = MenuMasterBusinessEntityItem.MenuID.ToString();
                tnChildMenu.SelectAction = TreeNodeSelectAction.None;
                tnChildMenu.Target = "_blank";

                //BindADEV(Convert.ToInt32(MenuMaster_BEItem.MenuID.ToString()), tnChildMenu, p_groupRight_BEList);

                //tnChildMenu.Checked=MenuMaster_BEItem.ADEVFlag

                //GetChildNodes(Convert.ToInt32(MenuMaster_BEItem.MenuID), tnChildMenu, p_menuMaster_BE, p_groupRight_BEList);

                if (MenuMasterBusinessEntityItem.ADEVFlag != null && MenuMasterBusinessEntityItem.ADEVFlag.ToString() != " ")//If true add ADEV attributes bellow the node
                //if ((MenuMaster_BE.ADEVFlag.ToString()) == "True")//If true add ADEV attributes bellow the node
                {
                    BindADEV(Convert.ToInt32(MenuMasterBusinessEntityItem.MenuID.ToString()), tnChildMenu, groupRight_BEList);
                }
                //else
                //{    //if (dtRightList != null && dtRightList.Rows.Count > 0)
                //    if (p_groupRight_BEList != null && p_groupRight_BEList.Count > 0)
                //    {
                //    }
                //}
                //Adding the node to the main menu
                treeMainNode.ChildNodes.Add(tnChildMenu);
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
    private void BindADEV(int rootID, TreeNode treeMainNode, List<GroupRight_BE> groupRightBEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            string Rights = "";

            if (groupRightBEList != null && groupRightBEList.Count > 0)
            {
                Rights = GetMenuAccessRights(rootID, groupRightBEList);
            }

            treeMainNode.Checked = Rights == "A" ? true : false;

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
    /// <param name="menuID"></param>
    /// <param name="groupRightBEList"></param>
    private string GetMenuAccessRights(int menuID, List<GroupRight_BE> groupRightBEList)
    {
        HandleWriteLog("Start", new StackTrace(true));
        string Rights = string.Empty;

        foreach (GroupRight_BE GroupRightBusinessEntityItem in groupRightBEList)
        {
            if (GroupRightBusinessEntityItem.MenuID == Convert.ToInt32(menuID))
            {
                Rights = GroupRightBusinessEntityItem.AccessRights;
            }
        }

        HandleWriteLog("End", new StackTrace(true));
        return (Rights);
    }

    /// <summary>
    /// Traverse the tree for New Permission list set for the group
    /// Called after Clicking the Submit Button
    /// </summary>
    private void GetChildPermission(TreeNode treeMainMenu)
    {
        HandleWriteLog("Start", new StackTrace(true));

        for (int l = 0; l < treeMainMenu.ChildNodes.Count; l++)
        {
            TreeNode tnChildMenu = treeMainMenu.ChildNodes[l];

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
    private void UpdateTable(TreeNode treeChild)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            foreach (GroupRight_BE GroupRightBusinessEntityItem in GroupRightBusinessEntityList)
            {
                if (Convert.ToString(GroupRightBusinessEntityItem.MenuID).Trim().Equals(treeChild.Value.Trim()))
                {
                    if (GroupRightBusinessEntityItem.AccessRights != null)
                    {
                        GroupRightBusinessEntityItem.AccessRights = "A";

                        //function to set AEDV flag for the parents
                        SetAEDVToParents(Convert.ToInt32(treeChild.Parent.Value.Trim()));

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
    private void SetAEDVToParents(int parentID)
    {
        HandleWriteLog("Start", new StackTrace(true));
        try
        {
            //GroupRight_BEList = (List<GroupRight_BE>)ViewState["LocalTable"];

            foreach (GroupRight_BE GroupRightBusinessEntityItem in GroupRightBusinessEntityList)
            {
                if (Convert.ToString(GroupRightBusinessEntityItem.MenuID).Trim().Equals(parentID.ToString().Trim()))
                {
                    if (GroupRightBusinessEntityItem.AccessRights != null && GroupRightBusinessEntityItem.AccessRights != "A")
                        GroupRightBusinessEntityItem.AccessRights = "A";
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
            //Loop to all elements of tree and set it's defaultvalue.
            foreach (GroupRight_BE GroupRightBusinessEntity in GroupRightBusinessEntityList)
            {
                GroupRightBusinessEntity.AccessRights = string.Empty;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #endregion

    /// <summary>
    /// Redirect to Group list page when click on previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbCancel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GroupMaintenanceList.aspx", false);
    }
}

#if CommentOut
//class Matrix
//{
//    public GroupRight_BE g1;
//    public GroupRight_BE g2;
//    public void getresult()
//    {
//        GroupRight_BE g3 = Matrix + g2;
//        // allow callers to initialize
//    }

//    // let user add matrices
    
    
//    public static Matrix operator +(Matrix mat1, Matrix mat2)
//    {
//        Matrix newMatrix = new Matrix();

//        newMatrix[mat1, mat2] = mat1[x, y] + mat2[x, y];

//        return newMatrix;
//    }
//}
#endif